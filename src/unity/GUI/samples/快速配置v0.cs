using System;
using Newtonsoft.Json;
using Unity.XR.PXR;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

/**
* 使用base.OnInspectorGUI()显示一个ScriptableObject。
* 在OnInspectorGUI里面可以读取PlatformSdkConfig中的变量
*/
namespace Editor
{
    enum AppRegion
    {
        Boe,
        Cn,
        Hw,
    }

    class PlatformSdkConfig : ScriptableObject
    {
        public AppRegion AppRegion;
        public string appId;
        public string packageName;
        public BuildTarget buildTarget;
        public AndroidSdkVersions targetSdkVersion;
        public AndroidSdkVersions minSdkVersion;
        public AndroidArchitecture targetArchitectures;
        public string wsaArchitecture;
        public AndroidBuildType androidBuildType;
    }

    [CustomEditor(typeof(PlatformSdkConfig))]
    public class PlatformSdkConfigEditor : UnityEditor.Editor
    {
        [MenuItem("快速运行/设置")]
        public static void R()
        {
            var obj = ScriptableObject.CreateInstance<PlatformSdkConfig>();
            obj.appId = PXR_PlatformSetting.Instance.appID;
            obj.buildTarget = EditorUserBuildSettings.activeBuildTarget;
            obj.wsaArchitecture = EditorUserBuildSettings.wsaArchitecture;
            obj.targetSdkVersion = PlayerSettings.Android.targetSdkVersion;
            obj.minSdkVersion = PlayerSettings.Android.minSdkVersion;
            obj.targetArchitectures = PlayerSettings.Android.targetArchitectures;
            obj.packageName = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
            obj.androidBuildType = EditorUserBuildSettings.androidBuildType;
            Debug.Log($"{JsonConvert.SerializeObject(obj)}");
            Selection.activeObject = obj;
        }

        public override void OnInspectorGUI()
        {
            Debug.Log($"OnInspectorGUI {Time.frameCount} {Time.time}");
            var obj = Selection.activeObject as PlatformSdkConfig;
            PXR_PlatformSetting.Instance.appID = obj.appId;
            base.OnInspectorGUI();
        }

        private void OnEnable()
        {
        }
    }
}