InGameDebugConsole如果使用新的输入系统，则需要做以下修改：
* DebugLogManager.cs 头部添加`#define ENABLE_INPUT_SYSTEM`
* 在PackageManager中导入InputSystem
* 在InGameDebugConsole这个插件的eventSystem中修改Input

修改InGameDebugConsole的asmdef

    {
        "name": "IngameDebugConsole.Runtime",
        "references": ["Unity.InputSystem"]
    }


为了简便期间，改完一次之后，可以将全部东西保存下来，下次可以直接导入，不用再改这么多地方了。  