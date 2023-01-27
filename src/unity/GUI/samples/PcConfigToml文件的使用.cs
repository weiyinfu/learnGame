using System;
using System.IO;
using Tommy;
using Unity.XR.PXR;
using UnityEditor;
using UnityEngine;

namespace Pico.Platform.Editor
{
    /// <summary>
    /// Unity Setting Getter and Setter
    /// </summary>
    class Gs
    {
        public static string appId
        {
            get { return PXR_PlatformSetting.Instance.appID; }
            set { PXR_PlatformSetting.Instance.appID = value; }
        }

        public static string productName
        {
            get { return PlayerSettings.productName; }
            set { PlayerSettings.productName = value; }
        }

        public static string packageName
        {
            get { return PlayerSettings.GetApplicationIdentifier(EditorUserBuildSettings.selectedBuildTargetGroup); }
            set { PlayerSettings.SetApplicationIdentifier(EditorUserBuildSettings.selectedBuildTargetGroup, value); }
        }

        public static BuildTargetGroup buildTargetGroup
        {
            get { return EditorUserBuildSettings.selectedBuildTargetGroup; }
            set { EditorUserBuildSettings.selectedBuildTargetGroup = value; }
        }

        public static AndroidSdkVersions minSdkVersion
        {
            get { return PlayerSettings.Android.minSdkVersion; }
            set { PlayerSettings.Android.minSdkVersion = value; }
        }

        public static AndroidSdkVersions targetSdkVersion
        {
            get { return PlayerSettings.Android.targetSdkVersion; }
            set { PlayerSettings.Android.targetSdkVersion = value; }
        }

        public static AndroidArchitecture targetArchitectures
        {
            get { return PlayerSettings.Android.targetArchitectures; }
            set { PlayerSettings.Android.targetArchitectures = value; }
        }

        public static string bundleVersion
        {
            get { return PlayerSettings.bundleVersion; }
            set { PlayerSettings.bundleVersion = value; }
        }

        public static int bundleVersionCode
        {
            get { return PlayerSettings.Android.bundleVersionCode; }
            set { PlayerSettings.Android.bundleVersionCode = value; }
        }

        public static ScriptingImplementation scriptBackend
        {
            get { return PlayerSettings.GetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup); }
            set { PlayerSettings.SetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup, value); }
        }

        public static AndroidBuildType androidBuildType
        {
            get { return EditorUserBuildSettings.androidBuildType; }
            set { EditorUserBuildSettings.androidBuildType = value; }
        }
    }

    public enum Region
    {
        cn = 0,
        i18n = 1,
    }

    public class PcConfig : ScriptableObject
    {
        public Region region = Region.cn;
        public string accessToken = "";
        internal TomlTable conf = new TomlTable();
        internal bool hasError = false;
    }

    [CustomEditor(typeof(PcConfig))]
    public class PcConfigEditor : UnityEditor.Editor
    {
        static string filepath = "Assets/Resources/PicoSdkPCConfig.toml";

        public override void OnInspectorGUI()
        {
            var x = Selection.activeObject as PcConfig;
            if (x.hasError)
            {
                EditorGUILayout.LabelField("配置文件有误，请检查配置");
                return;
            }

            base.OnInspectorGUI();
            this.save();
        }

        public static PcConfig load()
        {
            var obj = CreateInstance<PcConfig>();
            obj.hasError = false;
            try
            {
                if (File.Exists(filepath))
                {
                    using (StreamReader reader = File.OpenText(filepath))
                    {
                        // Parse the table
                        TomlTable table = TOML.Parse(reader);
                        obj.conf = table;
                        obj.accessToken = table["account"]["access_token"];
                        if (Region.TryParse(table["general"]["region"], true, out obj.region))
                        {
                        }
                        else
                        {
                            obj.hasError = true;
                            Debug.LogError($"unknown region");
                        }
                    }
                }
                else
                {
                    obj.conf = new TomlTable();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                obj.hasError = true;
            }

            return obj;
        }

        public void save()
        {
            var obj = Selection.activeObject as PcConfig;
            if (obj.hasError)
            {
                return;
            }

            var conf = obj.conf;

            conf["general"]["region"] = obj.region.ToString();
            conf["account"]["access_token"] = obj.accessToken;
            conf["package"]["package_name"] = Gs.packageName;
            conf["package"]["package_version_code"] = Gs.bundleVersionCode;
            conf["package"]["packageVersionName"] = Gs.bundleVersion;
            using (var cout = File.CreateText(filepath))
            {
                conf.WriteTo(cout);
            }
        }
    }
}