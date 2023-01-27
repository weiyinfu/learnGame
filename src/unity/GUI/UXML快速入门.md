# 如何快速学习新版UI？
要想使用UI Toolkit，需要使用UnityEditor2022。  
查看菜单windows/UI Toolkit/Samples，里面带着一些样例。这些样例主要学习UXML的写法。


# 如何快速创建一个使用新版UI的EditorWindow？
右键create，选择UIElements/EditorWindow，就会弹窗提示创建三个文件。

# UXML概览
在生成的脚本中，可以发现UXML文件是动态加载并渲染的。  
```c#

public class one : EditorWindow
{
    [MenuItem("Window/UIElements/one")]
    public static void ShowExample()
    {
        one wnd = GetWindow<one>();
        wnd.titleContent = new GUIContent("one");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scenes/新版UI/Editor/one.uxml");
        VisualElement labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scenes/新版UI/Editor/one.uss");
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.styleSheets.Add(styleSheet);
        root.Add(labelWithStyle);
    }
}
```

# 如何在runtime UI中使用UXML？
使用UXML有两种方式：在editor UI中、在runtime UI中。  
在editor中，使用EditorWindow的rootVisualElement，添加UXML资源。  
在runtime中，添加UIDocument类型的元素，然后为UI Document类型的元素设置UXML文件。  

# label+text
```csharp
var label = new Label("App ID");
var input = new TextField("App ID");
input.value = Gs.appId;
input.RegisterValueChangedCallback(e => { Gs.appId = e.newValue; });
root.Add(label);
root.Add(input);
```
Label就可以省略了，可以简写为
```csharp
var input = new TextField("App ID");
input.value = Gs.appId;
input.RegisterValueChangedCallback(e => { Gs.appId = e.newValue; });
root.Add(input);
```