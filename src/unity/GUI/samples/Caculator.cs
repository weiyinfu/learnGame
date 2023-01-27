using UnityEditor;
using UnityEngine;
/*
* 本例实现一个计算器，用户点击菜单弹出计算机面板，可以在计算器面板里面进行一些计算
* 首先定义一个ScriptableObject，表示这个Object是一个可以序列化的东西
* 然后定义一个CustomEditor，让这个Editor重写类X的Inspector。
*/
namespace PlatformSdk.Editor
{
    class X : ScriptableObject
    {
        public string text;
    }

    [CustomEditor(typeof(X))]
    public class Caculator : UnityEditor.Editor
    {
        /*
        * 当点击菜单的时候，设置全局变量Selection的activeObject，这样就会弹出Caculator这个类所定制的Inspection面板。
        * 这与选中Unity中的其它游戏物体的时候表现是一样的。
        */
        [MenuItem("计算器/计算")]
        public static void myCalculator()
        {
            var obj = CreateInstance<X>();
            obj.name = "计算器";
            Selection.activeObject = obj;
        }

        private static string operators = "+-*/";

        string[][] a = new[]
        {
            new[] {"1", "2", "3", "+"},
            new[] {"4", "5", "6", "-"},
            new[] {"7", "8", "9", "*"},
            new[] {"0", "C", "=", "/"},
        };

        public override void OnInspectorGUI()
        {
            var obj = Selection.activeObject as X;
            EditorGUILayout.LabelField(obj.text);
            foreach (var i in a)
            {
                EditorGUILayout.BeginHorizontal();
                foreach (var j in i)
                {
                    if (GUILayout.Button($"{j}"))
                    {
                        onClick(j);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        static void onClick(string s)
        {
            var x = Selection.activeObject as X;
            if (s.Length == 1 && s[0] >= '0' && s[0] <= '9' || "+-*/".Contains(s))
            {
                x.text += $"{s[0]}";
            }

            if (s.Equals("C"))
            {
                x.text = "";
            }

            if (s.Equals("="))
            {
                if (ExpressionEvaluator.Evaluate(x.text, out int v))
                {
                    x.text = $"{v}";
                }
            }
        }
    }
}