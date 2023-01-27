using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEditor;
using UnityEngine;

namespace PlatformSdk.Editor
{
    enum AppRegion
    {
        Boe,
        Cn,
        Hw,
        Old,
        Custom,
    }

    public enum AndroidArchitecture : uint
    {
        /// <summary>
        ///   <para>Invalid architecture.</para>
        /// </summary>
        None = 0,

        /// <summary>
        ///   <para>32-bit ARM architecture.</para>
        /// </summary>
        ARMv7 = 1,

        /// <summary>
        ///   <para>64-bit ARM architecture.</para>
        /// </summary>
        ARM64 = 2,

        /// <summary>
        ///   <para>32-bit Intel architecture.</para>
        /// </summary>
        X86 = 4,

        /// <summary>
        ///   <para>64-bit Intel architecture.</para>
        /// </summary>
        X86_64 = 8,

        /// <summary>
        ///   <para>All architectures.</para>
        /// </summary>
        All = 4294967295, // 0xFFFFFFFF
    }

    class PlatformSdkConfig : ScriptableObject
    {
        public AppRegion appRegion;
        public string appId;
        public string packageName;
        public string productName;
        public BuildTargetGroup buildTarget;
        public AndroidSdkVersions targetSdkVersion;
        public AndroidSdkVersions minSdkVersion;
        public ScriptingImplementation scriptBackend;
        public AndroidArchitecture targetArchitectures;
        public AndroidBuildType androidBuildType;
        public int bundleVersionCode;
        public string bundleVersion;
        public bool usePicoXr;
    }

    class SdkRegionConfig
    {
        public string appId;
        public string packageName;
        public string productName;
        public AppRegion region;

        public SdkRegionConfig(AppRegion region, string appId, string packageName, string productName)
        {
            this.region = region;
            this.appId = appId;
            this.packageName = packageName;
            this.productName = productName;
        }
    }

    [CustomEditor(typeof(PlatformSdkConfig))]
    public class PlatformSdkConfigEditor : UnityEditor.Editor
    {
        private static SdkRegionConfig[] apps =
        {
            new SdkRegionConfig(
                AppRegion.Boe,
                "xxxx",
                "com.xxx.platform",
                "PlatformDemoBoe"
            ),
            new SdkRegionConfig(AppRegion.Cn,
                "xxxx",
                "com.xxxx.platform",
                "PlatformDemoOnline"
            ),
            new SdkRegionConfig(AppRegion.Hw,
                "xxxx",
                "com.xxxx.platformhw",
                "PlatformDemoHw"
            ),
            new SdkRegionConfig(AppRegion.Old,
                "xxxx",
                "com.bytedance.platform",
                "PlatformDemo"
            )
        };

        private static Dictionary<AppRegion, SdkRegionConfig> region2app = new Dictionary<AppRegion, SdkRegionConfig>();

        static PlatformSdkConfigEditor()
        {
            foreach (var i in apps)
            {
                region2app[i.region] = i;
            }
        }

        [MenuItem("快速运行/设置")]
        public static void Config()
        {
            var obj = CreateInstance<PlatformSdkConfig>();
            obj.appId = PXR_PlatformSetting.Instance.appID;
            updateConfigByAppId(obj);
            obj.buildTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            obj.targetSdkVersion = PlayerSettings.Android.targetSdkVersion;
            obj.minSdkVersion = PlayerSettings.Android.minSdkVersion;
            obj.targetArchitectures = getFirst(PlayerSettings.Android.targetArchitectures);
            obj.bundleVersionCode = PlayerSettings.Android.bundleVersionCode;
            obj.scriptBackend = PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android);
            obj.bundleVersion = PlayerSettings.bundleVersion;
            obj.productName = PlayerSettings.productName;
            obj.packageName = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
            obj.androidBuildType = EditorUserBuildSettings.androidBuildType;
            obj.usePicoXr = false;
            Selection.activeObject = obj;
        }

        private static AndroidArchitecture getFirst(UnityEditor.AndroidArchitecture targetArchitectures)
        {
            if (targetArchitectures.HasFlag(UnityEditor.AndroidArchitecture.ARM64))
            {
                return AndroidArchitecture.ARM64;
            }

            if (targetArchitectures.HasFlag(UnityEditor.AndroidArchitecture.ARMv7))
            {
                return AndroidArchitecture.ARMv7;
            }

            return AndroidArchitecture.None;
        }

        static void updateConfigByAppId(PlatformSdkConfig obj)
        {
            foreach (var i in apps)
            {
                if (i.appId == obj.appId)
                {
                    obj.appRegion = i.region;
                    obj.packageName = i.packageName;
                    obj.productName = i.productName;
                    return;
                }
            }

            obj.appRegion = AppRegion.Custom;
        }

        static void updateConfigByRegion(PlatformSdkConfig obj)
        {
            obj.appRegion = AppRegion.Hw;
            if (region2app.TryGetValue(obj.appRegion, out SdkRegionConfig conf))
            {
                obj.appId = conf.appId;
                obj.packageName = conf.packageName;
                obj.productName = conf.productName;
            }
        }

        public override void OnInspectorGUI()
        {
            var obj = Selection.activeObject as PlatformSdkConfig;
            updateConfigByRegion(obj);
            PXR_PlatformSetting.Instance.appID = obj.appId;
            EditorUserBuildSettings.selectedBuildTargetGroup = obj.buildTarget;
            PlayerSettings.Android.targetSdkVersion = obj.targetSdkVersion;
            PlayerSettings.Android.minSdkVersion = obj.minSdkVersion;
            PlayerSettings.Android.targetArchitectures = (UnityEditor.AndroidArchitecture) obj.targetArchitectures;
            PlayerSettings.Android.bundleVersionCode = obj.bundleVersionCode;
            PlayerSettings.SetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup, obj.scriptBackend);
            PlayerSettings.bundleVersion = obj.bundleVersion;
            PlayerSettings.productName = obj.productName;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, obj.packageName);
            EditorUserBuildSettings.androidBuildType = obj.androidBuildType;
            base.OnInspectorGUI();
        }

        [MenuItem("快速运行/编译配置场景")]
        public static void BuildAllScene()
        {
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "a.apk", BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("快速运行/编译当前场景")]
        public static void BuildCurrentScene()
        {
            BuildPipeline.BuildPlayer(new EditorBuildSettingsScene[] { }, "a.apk", BuildTarget.Android, BuildOptions.None);
        }
    }
}