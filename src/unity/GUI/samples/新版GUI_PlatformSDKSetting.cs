using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PlatformSdk.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

class SdkPreset
{
    public string appId;
    public string packageName;
    public string productName;
    public string name;

    public static SdkPreset make(string name, string appId, string packageName, string productName)
    {
        var x = new SdkPreset();
        x.name = name;
        x.appId = appId;
        x.packageName = packageName;
        x.productName = productName;
        return x;
    }
}

class PresetLoader
{
    static string filepath = "Assets/Resources/SdkPresetList.json";

    public static List<SdkPreset> getDefaultPreset()
    {
        return new List<SdkPreset>
        {
            SdkPreset.make(
                "BOE",
                "45f1e68fae48ad1e65ac7f55a714f009",
                "com.bytedance.platform",
                "PlatformDemoBoe"
            ),
            SdkPreset.make(
                "线上",
                "b564ba1e2d3c8e39ee62041c4c5e757a",
                "com.bytedance.newonline",
                "PlatformDemoOnline"
            ),
            SdkPreset.make(
                "Old",
                "platform4unity_test",
                "com.bytedance.platform",
                "PlatformDemo"
            )
        };
    }

    public static List<SdkPreset> load()
    {
        if (File.Exists(filepath))
        {
            return JsonConvert.DeserializeObject<List<SdkPreset>>(File.ReadAllText(filepath));
        }
        else
        {
            var a = getDefaultPreset();
            var content = JsonConvert.SerializeObject(a, Formatting.Indented);
            File.WriteAllText(filepath, content);
            return a;
        }
    }
}

public class PlatformSdkSettings2 : EditorWindow
{
    [MenuItem("快速运行2/配置")]
    public static void ShowExample()
    {
        var wnd = GetWindow<PlatformSdkSettings2>();
        wnd.titleContent = new GUIContent("PlatformSDK快速配置");
    }

    [MenuItem("快速运行2/编译选中场景")]
    public static void BuildAllScene()
    {
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "a.apk", BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("快速运行2/编译当前场景")]
    public static void BuildCurrentScene()
    {
        BuildPipeline.BuildPlayer(new EditorBuildSettingsScene[] { }, "a.apk", BuildTarget.Android, BuildOptions.None);
    }

    string getScriptPath(string fileName)
    {
        string[] paths = AssetDatabase.FindAssets(fileName);
        if (paths.Length > 1)
        {
            Debug.LogError("multi file is found");
            return null;
        }

        if (paths.Length == 0)
        {
            Debug.LogError($"cannot find {fileName}");
            return null;
        }

        string realPath = AssetDatabase.GUIDToAssetPath(paths[0]);
        Debug.Log($"getScriptPath {fileName} {realPath}");
        return Path.GetDirectoryName(realPath);
    }

    private TextField appId;
    private TextField productName;
    private TextField packageName;

    public void CreateGUI()
    {
        var path = getScriptPath("PlatformSdkSetting2");
        Debug.Log(path);
        VisualElement root = rootVisualElement;
        bool showPreset = true;
        //预设
        if (showPreset)
        {
            var presetList = PresetLoader.load();
            Debug.Log($"presetList.Length={presetList.Count}");
            var presetGroup = new VisualElement();
            presetGroup.style.display = DisplayStyle.Flex;
            presetGroup.style.flexWrap = Wrap.Wrap;
            presetGroup.style.flexDirection = FlexDirection.Row;
            foreach (var preset in presetList)
            {
                var button = new Button(() =>
                {
                    this.productName.value = preset.productName;
                    this.packageName.value = preset.packageName;
                    this.appId.value = preset.appId;
                });
                button.text = preset.name;
                button.style.display = DisplayStyle.Flex;
                presetGroup.Add(button);
            }

            root.Add(presetGroup);
        }

        //appId
        {
            var input = new TextField("App ID");
            input.value = Gs.appId;
            input.RegisterValueChangedCallback(e => { Gs.appId = e.newValue; });
            root.Add(input);
            this.appId = input;
        }
        //productName
        {
            var input = new TextField("Product Name");
            input.value = Gs.productName;
            input.RegisterValueChangedCallback(e => { Gs.productName = e.newValue; });
            root.Add(input);
            this.productName = input;
        }
        //packageName
        {
            var input = new TextField("Package Name");
            input.value = Gs.packageName;
            input.RegisterValueChangedCallback(e => { Gs.packageName = e.newValue; });
            root.Add(input);
            this.packageName = input;
        }
        //buildTarget
        {
            var input = new EnumField("Build Target", Gs.buildTargetGroup);
            input.RegisterValueChangedCallback(e => { Gs.buildTargetGroup = (BuildTargetGroup) e.newValue; });
            root.Add(input);
        }
        //MinSDKVersion
        {
            var input = new EnumField("Min SDK Version", Gs.minSdkVersion);
            input.RegisterValueChangedCallback(e => { Gs.minSdkVersion = (AndroidSdkVersions) e.newValue; });
            root.Add(input);
        }
        //Target SDK version
        {
            var input = new EnumField("Target SDK Version", Gs.targetSdkVersion);
            input.RegisterValueChangedCallback(e => { Gs.targetSdkVersion = (AndroidSdkVersions) e.newValue; });
            root.Add(input);
        }
        //BundleVersion
        {
            var input = new TextField("Bundle Version");
            input.value = Gs.bundleVersion;
            input.RegisterValueChangedCallback(e => { Gs.bundleVersion = e.newValue; });
            root.Add(input);
        }
        //BundleVersionCode
        {
            var input = new IntegerField("Bundle Version Code");
            input.value = Gs.bundleVersionCode;
            input.RegisterValueChangedCallback(e => { Gs.bundleVersionCode = e.newValue; });
            root.Add(input);
        }
        //Script Backend
        {
            var input = new EnumField("Script Backend", Gs.scriptBackend);
            input.RegisterValueChangedCallback(e => { Gs.scriptBackend = (ScriptingImplementation) e.newValue; });
            root.Add(input);
        }
        //Target Architecture
        {
            var input = new EnumField("Target Architecture", Gs.targetArchitectures);
            input.RegisterValueChangedCallback(e => { Gs.targetArchitectures = (AndroidArchitecture) e.newValue; });
            root.Add(input);
        }
        //Android Build Type
        {
            var input = new EnumField("Android Build Type", Gs.androidBuildType);
            input.RegisterValueChangedCallback(e => { Gs.androidBuildType = (AndroidBuildType) e.newValue; });
            root.Add(input);
        }
        //UI Orientation
        {
            var input = new EnumField("UI Orientation", Gs.UIOrientation);
            input.RegisterValueChangedCallback(e => { Gs.UIOrientation = (UIOrientation) e.newValue; });
            root.Add(input);
        }
        //Use PicoXR
        {
            var input = new Toggle("Use PICO XR");
            input.value = Gs.UsePicoXr;
            input.RegisterValueChangedCallback(e => { Gs.UsePicoXr = e.newValue; });
            root.Add(input);
        }
        //Use Custom Keystore
        {
            var input = new Toggle("Use Custom Keystore");
            input.value = Gs.useCustomKeystore;
            var keystoreGroup = new VisualElement();

            input.RegisterValueChangedCallback(e =>
            {
                Gs.useCustomKeystore = e.newValue;
                if (Gs.useCustomKeystore)
                {
                    keystoreGroup.style.display = DisplayStyle.Flex;
                }
                else
                {
                    keystoreGroup.style.display = DisplayStyle.None;
                }
            });
            root.Add(input);
            //Keystore name
            {
                var x = new TextField("Keystore Name");
                x.value = Gs.keystoreName;
                x.RegisterValueChangedCallback(e => { Gs.keystoreName = e.newValue; });
                keystoreGroup.Add(x);
            }
            //Keystore password
            {
                var x = new TextField("Keystore Password");
                x.value = Gs.keystorePass;
                x.RegisterValueChangedCallback(e => { Gs.keystorePass = e.newValue; });
                keystoreGroup.Add(x);
            }
            //Keyalias name
            {
                var x = new TextField("Keyalias name");
                x.value = Gs.keyaliasName;
                x.RegisterValueChangedCallback(e => { Gs.keyaliasName = e.newValue; });
                keystoreGroup.Add(x);
            }
            //Keyalias password
            {
                var x = new TextField("Keyalias Password");
                x.value = Gs.keyaliasPass;
                x.RegisterValueChangedCallback(e => { Gs.keyaliasPass = e.newValue; });
                keystoreGroup.Add(x);
            }
            root.Add(keystoreGroup);
            if (Gs.useCustomKeystore)
            {
                keystoreGroup.style.display = DisplayStyle.Flex;
            }
            else
            {
                keystoreGroup.style.display = DisplayStyle.None;
            }
        }

        //Buttons
        {
            var footer = new VisualElement();
            footer.style.display = DisplayStyle.Flex;
            footer.style.flexDirection = FlexDirection.Row;
            footer.style.alignItems = Align.Center;
            footer.style.justifyContent = Justify.SpaceAround;
            var compileScene = new Button(() => { BuildAllScene(); });
            compileScene.text = "编译选中场景";
            var compileCurrent = new Button(() => { BuildCurrentScene(); });
            compileCurrent.text = "编译当前场景";
            footer.Add(compileScene);
            footer.Add(compileCurrent);
            root.Add(footer);
        }
    }
}