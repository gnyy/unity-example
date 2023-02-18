#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.Void UnityEngine.AssetBundle::.ctor()
extern void AssetBundle__ctor_m12989CA081324BB49ED893BDA5E3B4E758D61410 (void);
// 0x00000002 T UnityEngine.AssetBundle::LoadAsset(System.String)
// 0x00000003 UnityEngine.Object UnityEngine.AssetBundle::LoadAsset(System.String,System.Type)
extern void AssetBundle_LoadAsset_m021FE0B52DD660E54AE4C225D9AE66147902B8FE (void);
// 0x00000004 UnityEngine.Object UnityEngine.AssetBundle::LoadAsset_Internal(System.String,System.Type)
extern void AssetBundle_LoadAsset_Internal_mD096392756815901FE982C1AF64DDF0846551433 (void);
static Il2CppMethodPointer s_methodPointers[4] = 
{
	AssetBundle__ctor_m12989CA081324BB49ED893BDA5E3B4E758D61410,
	NULL,
	AssetBundle_LoadAsset_m021FE0B52DD660E54AE4C225D9AE66147902B8FE,
	AssetBundle_LoadAsset_Internal_mD096392756815901FE982C1AF64DDF0846551433,
};
static const int32_t s_InvokerIndices[4] = 
{
	5490,
	0,
	2008,
	2008,
};
static const Il2CppTokenRangePair s_rgctxIndices[1] = 
{
	{ 0x06000002, { 0, 2 } },
};
extern const uint32_t g_rgctx_T_t8F5466E687C8861B9EA9A0932E573160D9356A57;
extern const uint32_t g_rgctx_T_t8F5466E687C8861B9EA9A0932E573160D9356A57;
static const Il2CppRGCTXDefinition s_rgctxValues[2] = 
{
	{ (Il2CppRGCTXDataType)1, (const void *)&g_rgctx_T_t8F5466E687C8861B9EA9A0932E573160D9356A57 },
	{ (Il2CppRGCTXDataType)2, (const void *)&g_rgctx_T_t8F5466E687C8861B9EA9A0932E573160D9356A57 },
};
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_UnityEngine_AssetBundleModule_CodeGenModule;
const Il2CppCodeGenModule g_UnityEngine_AssetBundleModule_CodeGenModule = 
{
	"UnityEngine.AssetBundleModule.dll",
	4,
	s_methodPointers,
	0,
	NULL,
	s_InvokerIndices,
	0,
	NULL,
	1,
	s_rgctxIndices,
	2,
	s_rgctxValues,
	NULL,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};
