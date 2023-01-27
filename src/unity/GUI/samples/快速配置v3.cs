using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Unity.XR.PXR;
using UnityEditor;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;
using UnityEngine.XR.Management;

namespace PlatformSdk.Editor
{
    class PlatformSdkSetting : ScriptableObject
    {
        public string one;
        public List<SdkPreset> presets;
    }

    public class SdkPreset : ScriptableObject
    {
        public string appId;
        public string packageName;
        public string productName;
        public string name;
        public bool valid;

        public static SdkPreset Get(string name, string appId, string packageName, string productName)
        {
            var obj = CreateInstance<SdkPreset>();
            obj.name = name;
            obj.appId = appId;
            obj.packageName = packageName;
            obj.productName = productName;
            obj.valid = true;
            return obj;
        }
    }

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

        static XRManagerSettings GetXrSettings()
        {
            XRGeneralSettings generalSettings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Android);
            if (generalSettings == null) return null;
            var assignedSettings = generalSettings.AssignedSettings;
            return assignedSettings;
        }

        static PXR_Loader GetPxrLoader()
        {
            var x = GetXrSettings();
            if (x == null) return null;
            foreach (var i in x.activeLoaders)
            {
                if (i is PXR_Loader)
                {
                    return i as PXR_Loader;
                }
            }

            return null;
        }

        public static bool UsePicoXr
        {
            get { return GetPxrLoader() != null; }
            set
            {
                var x = GetXrSettings();
                if (x == null) return;
                var loader = GetPxrLoader();
                if (value == false)
                {
                    if (loader == null)
                    {
                    }
                    else
                    {
                        x.TryRemoveLoader(loader);
                    }
                }
                else
                {
                    if (loader == null)
                    {
                        var res = XRPackageMetadataStore.AssignLoader(x, nameof(PXR_Loader), BuildTargetGroup.Android);
                        Debug.Log($"设置XR{res} {value}");
                    }
                    else
                    {
                    }
                }
            }
        }
    }

    public class PresetConfig : ScriptableObject
    {
        public List<SdkPreset> presets;
    }

    [CustomEditor(typeof(PresetConfig))]
    public class PresetConfigEditor : UnityEditor.Editor
    {
        static string filepath = "Assets/Resources/SdkPresets.json";

        public static List<SdkPreset> getDefaultPreset()
        {
            return new List<SdkPreset>
            {
                SdkPreset.Get(
                    "BOE",
                    "45f1e68fae48ad1e65ac7f55a714f009",
                    "com.bytedance.platform",
                    "PlatformDemoBoe"
                ),
                SdkPreset.Get("CN",
                    "2b252742cda0c1fd4407929ddf4c2c27",
                    "com.bytedance.platform",
                    "PlatformDemoOnline"
                ),
                SdkPreset.Get("海外",
                    "825a7f400bcb10cb27b1ed7dd9269d2a",
                    "com.bytedance.platformhw",
                    "PlatformDemoHw"
                ),
                SdkPreset.Get("Old",
                    "platform4unity_test",
                    "com.bytedance.platform",
                    "PlatformDemo"
                )
            };
        }

        public override void OnInspectorGUI()
        {
            var obj = Selection.activeObject as PresetConfig;
            foreach (var i in obj.presets)
            {
                if (!i.valid) continue;
                EditorGUILayout.LabelField("Name");
                i.name = EditorGUILayout.TextField(i.name);
                EditorGUILayout.LabelField($"App Id");
                i.appId = EditorGUILayout.TextField(i.appId);
                EditorGUILayout.LabelField($"Package Name");
                i.packageName = EditorGUILayout.TextField(i.packageName);
                EditorGUILayout.LabelField($"Product Name");
                i.productName = EditorGUILayout.TextField(i.productName);
                if (GUILayout.Button("删除"))
                {
                    i.valid = false;
                }

                EditorGUILayout.Separator();
            }

            if (GUILayout.Button("添加"))
            {
                Debug.Log("添加Preset");
                obj.presets.Add(SdkPreset.Get("", "", "", ""));
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("重置"))
            {
                obj.presets = getDefaultPreset();
            }

            if (GUILayout.Button("重载"))
            {
                Selection.activeObject = load();
            }

            if (GUILayout.Button("保存"))
            {
                this.save();
            }

            EditorGUILayout.EndHorizontal();
        }

        public static PresetConfig load()
        {
            if (File.Exists(filepath))
            {
                PresetConfig obj = JsonConvert.DeserializeObject<PresetConfig>(File.ReadAllText(filepath));
                return obj;
            }
            else
            {
                var obj = CreateInstance<PresetConfig>();
                obj.presets = getDefaultPreset();
                return obj;
            }
        }

        void save()
        {
            Debug.Log($"保存配置");
            var obj = Selection.activeObject as PresetConfig;
            obj.presets = obj.presets.Where(x => x.valid).ToList();
            var ans = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(filepath, ans);
        }
    }

    [CustomEditor(typeof(PlatformSdkSetting))]
    public class PlatformSdkSettingEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            Debug.Log($"配置OnEnable");
        }

        static void usePreset(SdkPreset sdkPreset)
        {
            Gs.appId = sdkPreset.appId;
            Gs.packageName = sdkPreset.packageName;
            Gs.productName = sdkPreset.productName;
        }

        public override void OnInspectorGUI()
        {
            var obj = Selection.activeObject as PlatformSdkSetting;
            {
                EditorGUILayout.LabelField("预设");
                EditorGUILayout.BeginHorizontal();
                int cnt = 0;
                int rowCount = 3;
                foreach (var i in obj.presets)
                {
                    cnt++;
                    if (GUILayout.Button(i.name))
                    {
                        Debug.Log($"点击{JsonConvert.SerializeObject(i)}");
                        usePreset(i);
                    }

                    if (cnt % rowCount == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            //appId
            {
                EditorGUILayout.LabelField("App Id");
                Gs.appId = EditorGUILayout.TextField(Gs.appId);
                EditorGUILayout.Separator();
            }

            //ProductName
            {
                EditorGUILayout.LabelField("Product Name");
                Gs.productName = EditorGUILayout.TextField(Gs.productName);
                EditorGUILayout.Separator();
            }

            //PackageName
            {
                EditorGUILayout.LabelField("Package Name");
                var x = EditorGUILayout.TextField(Gs.packageName);
                PlayerSettings.SetApplicationIdentifier(EditorUserBuildSettings.selectedBuildTargetGroup, x);
                EditorGUILayout.Separator();
            }


            //Build Target
            {
                EditorGUILayout.LabelField("Build Target");
                Gs.buildTargetGroup = (BuildTargetGroup) EditorGUILayout.EnumPopup(Gs.buildTargetGroup);
                EditorGUILayout.Separator();
            }


            //Min SDK Version
            {
                EditorGUILayout.LabelField("Min SDK Version");
                Gs.minSdkVersion = (AndroidSdkVersions) EditorGUILayout.EnumFlagsField(Gs.minSdkVersion);
                EditorGUILayout.Separator();
            }

            //Target SDK Version
            {
                EditorGUILayout.LabelField("Target SDK Version");
                Gs.targetSdkVersion = (AndroidSdkVersions) EditorGUILayout.EnumPopup(Gs.targetSdkVersion);
                EditorGUILayout.Separator();
            }


            //Target Architectures
            {
                EditorGUILayout.LabelField("Target Architecture");
                Gs.targetArchitectures = (AndroidArchitecture) EditorGUILayout.EnumPopup(getFirst(Gs.targetArchitectures));
                EditorGUILayout.Separator();
            }


            //Bundle Version
            {
                EditorGUILayout.LabelField("Bundle Version");
                Gs.bundleVersion = EditorGUILayout.TextField(Gs.bundleVersion);
                EditorGUILayout.Separator();
            }

            //bundleVersionCode
            {
                EditorGUILayout.LabelField("Bundle Version Code");
                Gs.bundleVersionCode = EditorGUILayout.IntField(Gs.bundleVersionCode);
                EditorGUILayout.Separator();
            }

            //ScriptBackend
            {
                EditorGUILayout.LabelField("Script Backend");
                Gs.scriptBackend = (ScriptingImplementation) EditorGUILayout.EnumPopup(Gs.scriptBackend);
                EditorGUILayout.Separator();
            }


            //AndroidBuildType
            {
                EditorGUILayout.LabelField("Android Build Type");
                Gs.androidBuildType = (AndroidBuildType) EditorGUILayout.EnumPopup(Gs.androidBuildType);
                EditorGUILayout.Separator();
            }
            //UsePicoXr
            {
                EditorGUILayout.LabelField("Use Pico XR");
                Gs.UsePicoXr = EditorGUILayout.Toggle(Gs.UsePicoXr);
            }
            //场景编译
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("编译选中场景"))
                {
                    MenuManager.BuildAllScene();
                }

                if (GUILayout.Button("编译当前场景"))
                {
                    MenuManager.BuildCurrentScene();
                }

                EditorGUILayout.EndHorizontal();
            }


            EditorGUILayout.Separator();
        }

        private static AndroidArchitecture getFirst(AndroidArchitecture targetArchitectures)
        {
            if (targetArchitectures.HasFlag(AndroidArchitecture.ARM64))
            {
                return AndroidArchitecture.ARM64;
            }

            if (targetArchitectures.HasFlag(AndroidArchitecture.ARMv7))
            {
                return AndroidArchitecture.ARMv7;
            }

            return AndroidArchitecture.None;
        }
    }

    class MenuManager
    {
        [MenuItem("快速运行/配置")]
        public static void v2()
        {
            var conf = PresetConfigEditor.load();
            Debug.Log(JsonConvert.SerializeObject(conf.presets));
            var obj = ScriptableObject.CreateInstance<PlatformSdkSetting>();
            obj.name = "PlatformSDK快速配置";
            obj.presets = conf.presets;
            Selection.activeObject = obj;
            Debug.Log("配置PlatformSdkEditor");
        }

        [MenuItem("快速运行/编辑预设")]
        public static void editorPreset()
        {
            var obj = PresetConfigEditor.load();
            obj.name = "编辑预设";
            Selection.activeObject = obj;
        }


        [MenuItem("快速运行/编译选中场景")]
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