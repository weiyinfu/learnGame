# imGUI的frameBox
```plain
GUIStyle style = "frameBox";
style.fixedWidth = frameWidth;
EditorGUILayout.BeginVertical(style);
```

# imGUI的启用禁用
IMGUI是一种函数式语法，设置全局变量，然后调用Button函数即可。
```plain
GUI.enabled = hasSomethingToFix;
if (GUILayout.Button(strApplyButtonText[(int) language], GUILayout.Width(130)))
{
    this.ApplyRecommendConfig();
}

GUI.enabled = true;

```
# toggle如何设置labelWidth
```plain
float originalValue = EditorGUIUtility.labelWidth;
EditorGUIUtility.labelWidth = 250;
field.value = EditorGUILayout.Toggle(field.value);
EditorGUIUtility.labelWidth = originalValue;
```