using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Google.Maps.Coord;
using Google.Maps.Event;
using Google.Maps.Examples.Shared;
using UnityEngine.Android;

namespace Google.Maps.Examples
{
    public class Player : MonoBehaviour
    {
        public LatLng playerLatLng;
        public LatLng playerRelativeOffset;
        //This is not real GPS location, but the relative coordinates offset for calculating player-defined location from current location.
        //Should be set to (0, 0) when initialized;
        public Vector3 playerCoord;
        public BaseMapLoader BaseMapLoader;
        public UDPMessenger rfidMessenger;

        private string previousReadTag = "";
        //private Quaternion lastPlayerRotation;
        private bool isPlayerRotationSet = false;

        bool firstrun = true;
        public GameObject TanglangMap;

        // Start is called before the first frame update
        void Start()
        {
            //Require GPS permission and start location service.
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
            }
            StartCoroutine(LocationService());

            rfidMessenger = GameObject.Find("UDPMessenger").GetComponent<UDPMessenger>();
            if (rfidMessenger == null)
            {
                Debug.LogError("Failed to find RFID Messenger!");
            }

            TanglangMap = GameObject.Find("TanglangCity");
        }


        //Read position data
        IEnumerator LocationService()
        {
            Debug.Log("start location service");

            // First, check if user has location service enabled
            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("GPS access is not granted");
                yield break;
            }

            // Start service before querying location
            Input.location.Start(0.1f, 0.1f); //public void Start(float desiredAccuracyInMeters, float updateDistanceInMeters);
            //desiredAccuracyInMeters - desired service accuracy in meters. Values like 5-10 could be used for getting best accuracy. Default value is 10 meters. 
            //updateDistanceInMeters - the minimum distance (measured in meters) a device must move laterally before Input.location property is updated. Default is 10 meters. 
            Debug.Log("starting...");
            // Wait until service initializes
            int maxWait = 3;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                Debug.Log(maxWait);
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                Debug.Log("Timed out");
                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determine device location");
                yield break;
            }
            else
            {
                // Access granted and location value could be retrieved
                if (Input.compass.enabled != true)
                    Input.compass.enabled = true;
                Debug.Log("Compass True Heading:" + Input.compass.trueHeading);

            }
        }


        // Update is called once per frame
        private void Update()
        {
            //float angle = Quaternion.Angle(this.transform.rotation, lastPlayerRotation);
            if (Input.location.status == LocationServiceStatus.Running)
            {
                if (firstrun)
                {
                    Debug.Log("LatLng:"+ Input.location.lastData.latitude+","+ Input.location.lastData.longitude);
                    playerLatLng = new LatLng(Input.location.lastData.latitude, Input.location.lastData.longitude);

                    //float x = (float)((Input.location.lastData.latitude - 22.58645) * 3210 / 0.02242);
                    //float z = (float)((Input.location.lastData.longitude - 113.97615) * 3210 / 0.03169);

                    float posX = (float)((playerLatLng.Lng - 113.97615) * 3210 / 0.03169) - 150;  //15是地图左边缘与轴的距离
                    float posZ = (float)((playerLatLng.Lat - 22.58645) * 2550 / 0.02242) - 65;  //20是地图下边缘与轴的距离

                    TanglangMap.transform.position = new Vector3(-1 * posX, TanglangMap.transform.position.y, -1 * posZ);
                    this.GetComponent<Rigidbody>().useGravity = true;
                    firstrun = false;
                }
                else { 
                    if (BaseMapLoader != null)
                    {
                        //Calculate the relative offset player coordinates from current GPS real location to RFID - specified location.
                        if (rfidMessenger.rfidTag.Latitude != null && rfidMessenger.rfidTag.Longitude != null)
                        {
                            //Debug.Log("RFID Tag Contains Lat and Lng:" + rfidMessenger.rfidTag.Latitude + ", " + rfidMessenger.rfidTag.Longitude);
                            if (rfidMessenger.rfidTag.Latitude <= 90 && -90 <= rfidMessenger.rfidTag.Latitude && rfidMessenger.rfidTag.Longitude <= 180 && -180 <= rfidMessenger.rfidTag.Longitude)
                            {
                                //Check if a pair of valid coordinates being specified 
                                if (playerRelativeOffset.Equals(null) || rfidMessenger.rfidTag.EpcName != previousReadTag)
                                {
                                    //Debug.Log("First Time Calculate Location Offset.");
                                    //{
                                    playerRelativeOffset = new LatLng((double)rfidMessenger.rfidTag.Latitude - Input.location.lastData.latitude,
                                        (double)rfidMessenger.rfidTag.Longitude - Input.location.lastData.longitude);
                                    previousReadTag = rfidMessenger.rfidTag.EpcName;
                                    Debug.Log("Player Relative Offset is set to: " + playerRelativeOffset.Lat + ", " + playerRelativeOffset.Lng);
                                    //}
                                }
                            }
                            else
                                Debug.LogError("Invalid Location Coordinates!");
                        }
                        else
                            playerRelativeOffset = new LatLng(0, 0);

                        playerLatLng = new LatLng(Input.location.lastData.latitude + playerRelativeOffset.Lat, Input.location.lastData.longitude + playerRelativeOffset.Lng);

                        playerCoord = BaseMapLoader.MapsService.Projection.FromLatLngToVector3(playerLatLng);
                        this.transform.position = new Vector3(playerCoord.x, this.transform.position.y, playerCoord.z);
                        //Debug.Log("Player position set: (" + this.transform.position.x + "," + this.transform.position.y + "," + this.transform.position.z + ")");
                        if (Input.compass.trueHeading != 0f && isPlayerRotationSet == false)
                        {
                            //this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.y + 90f + Input.compass.trueHeading, 0);
                            this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.y + Input.compass.trueHeading, 0);
                            isPlayerRotationSet = true;
                            //Debug.Log("Player's rotation is set!!!");
                            //lastPlayerRotation = this.transform.rotation;
                        }
                        //Debug.Log("Compass trueheading: " + Input.compass.trueHeading);
                    }
                    else
                        Debug.Log("Base Map Loader cannot be found");
                }
            }
            //else
            //
            //Debug.Log("Location Service is not Running");
        }



    }
}
