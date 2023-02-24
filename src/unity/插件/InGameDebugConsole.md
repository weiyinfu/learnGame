InGameDebugConsole如果使用新的输入系统，则需要做以下修改：
* 在PackageManager中导入InputSystem
* 在InGameDebugConsole这个插件的eventSystem中修改Input

修改InGameDebugConsole的asmdef

    {
        "name": "IngameDebugConsole.Runtime",
        "references": ["Unity.InputSystem"]
    }



为了简便期间，改完一次之后，可以将全部东西保存下来，下次可以直接导入，不用再改这么多地方了。  

# 宏
在DebugLogManager.cs文件头部有一个宏`#define ENABLE_INPUT_SYSTEM`。这个宏的作用是开启InputSystem。  
一旦在InGameDebugConsole的references里面添加了Unity.InputSystem，这个宏就自动定义了。  
```csharp
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif
#if UNITY_EDITOR && UNITY_2021_1_OR_NEWER
using Screen = UnityEngine.Device.Screen; // To support Device Simulator on Unity 2021.1+
#endif
```
#define ENABLE_INPUT_SYSTEM