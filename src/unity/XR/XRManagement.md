# XR 配置插件

如果直接使用AssignedSettings可能没有东西，需要使用特定平台的

```
var m=XRGeneralSettings.Instance.Manager;
m.TrySetLoaders(new List<XRLoader>());
var se=XRGeneralSettings.Instance.AssignedSettings;
```

正确的设置XR的方法：

```plain
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
```

XR Plugin Management文档：<https://docs.unity3d.com/Packages/com.unity.xr.management@4.0/manual/EndUser.html>