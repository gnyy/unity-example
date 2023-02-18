using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SpatialTracking;
using Google.XR.Cardboard;
using Google.Maps.Examples;

public class UIManager : MonoBehaviour
{
    public GameObject pauseNotification;
    public Text rfidData;
    public Text playerData;
    public Player player;
    public UDPMessenger udpMessenger;
    public bool isCalibrating;
    //public Vector3 offsetRotation;
    //private Quaternion previousCameraRotation;
    // Start is called before the first frame update
    void Start()
    {
        udpMessenger = GameObject.Find("UDPMessenger").GetComponent<UDPMessenger>();
        if (player == null)
            Debug.Log("Cannot find player");
        if (udpMessenger == null)
            Debug.Log("Cannot find UDP Messenger");

        isCalibrating = false;
        pauseNotification.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 tempPosition = player.transform.position;
        //Vector3 tempPlayerPos;
        //Quaternion tempRotation = Camera.main.transform.rotation;
        //Quaternion tempPlayerRot;

        if (Api.IsGearButtonPressed)
        {
            Debug.Log("Cardboard Trigger Pressed.");

            if (isCalibrating == false)
            {
                isCalibrating = true;
                this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            }
            else
                isCalibrating = false;


            if (pauseNotification.activeSelf == false)
                pauseNotification.SetActive(true);
            else
                pauseNotification.SetActive(false);

        }

        if (isCalibrating)
        {
            Debug.Log("Calibrating Now.");

            if (Api.IsTriggerPressed)
            {
                player.transform.Rotate(0, -1, 0, Space.Self); //Player turns left by one degree per press
            }
        }
        else
        {
            rfidData.text = "RFID Data: " + udpMessenger.rfidData;
            playerData.text = "Player Location: (" + this.player.playerLatLng.Lat + ", " + this.player.playerLatLng.Lng + "), Position: ("
                + this.player.transform.position.x + ", " + this.player.transform.position.z + ")";
            GameObject TanglangMap = GameObject.Find("TanglangCity");
            playerData.text += "(map tranform: " + TanglangMap.transform.position.x+","+TanglangMap.transform.position.z+")";

        }

        //else
        //{
        //    if (pauseNotification.activeSelf == true)
        //    {
        //        Debug.Log("End Calibrating.");
        //        pauseNotification.SetActive(false);
        //    }
        //}

        if (Api.IsCloseButtonPressed)
        {
            Application.Quit();
        }

        //previousCameraRotation = tempRotation;

    }

}
