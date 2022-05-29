# ScriptableObject
使用ScriptableObject可以把对象以资源的形式进行存储。  

# Unity编辑器扩展
三个重要的类：MenuItem、EditorWindow、ScriptableWizard。  
```plain
MenuItem(string itemName,bool isValidateFunction,int priority)
```

我知道我们通常使用MenuItem常常使用的是它的第一个参数，即定义一个菜单项的名称，我们可以使用”/”这样的分隔符来表示菜单的层级，MenuItem需要配合一个静态方法来使用，可以理解为当我们点击当前定义的菜单后就会去执行静态方法中的代码，因此MenuItem常常可以帮助我们做些编辑器扩展开发的工作。

# unity中有两个一模一样的菜单项
先导入谁就是谁生效
```
[UnityEditor.MenuItem("Pico/Platform/Edit Settings")]
public static void Edit()
{
    Debug.Log("Two");
    // UnityEditor.Selection.activeObject = PlatformSettings.Instance;
}
[UnityEditor.MenuItem("Pico/Platform/Edit Settings")]
public static void EditHaha()
{
    Debug.Log("One");
    // UnityEditor.Selection.activeObject = PlatformSettings.Instance;
}
```
# HideInInspector
在变量上使用这个属性，可以让public的变量在Inspector上隐藏，也就是无法在Editor中进行编辑。

# Header
```
public class ExampleClass : MonoBehaviour {
    [Header("生命值")]
    public int CurrentHP = 0;
    public int MaxHP = 100;

    [Header("魔法值")]
    public int CurrentMP = 0;
    public int MaxMP = 0;
}
```

# RangeAttribute
在int或者float类型上使用，限制输入值的范围
```
public class TestRange : MonoBehaviour
{
    [Range(0, 100)] public int HP;
}
```


# TextAreaAttribute
该属性可以把string在Inspector上的编辑区变成一个TextArea。
例子：
```
public class TestTextAreaAttributeByLvmingbei : MonoBehaviour {
    [TextArea]
    public string mText;
}

```
# TooltipAttribute
这个属性可以为变量上生成一条tip，当鼠标指针移动到Inspector上时候显示。
```
public class TestTooltipAttributeByLvmingbei : MonoBehaviour {
    [Tooltip("This year is 2015!")]
    public int year = 0;
}
```
