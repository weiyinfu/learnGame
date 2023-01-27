using UnityEditor;
using UnityEngine;

public class GameEditorWindow : EditorWindow
{
    [MenuItem("我的/游戏难度编辑")]
    static void Init()
    {
        var window = GetWindow<GameEditorWindow>();
        window.title = "XX GM指令";
        window.Show();
    }

    private int newExp = 0, newMoney = 0, newVip = 0, newVp = 0, newCoin = 0, newSpirts = 0;
    private int maxHp = 0, maxVp = 0, maxHurt = 0;

    private int nMapId = 0;

    public void OnGUI()
    {
        EditorGUILayout.LabelField("== 加数值 指令 ==");

        GUILayout.BeginHorizontal();
        GUILayout.Label("经验:");
        newExp = EditorGUILayout.IntField(newExp, GUILayout.ExpandWidth(true), GUILayout.MinHeight(20));
        if (GUILayout.Button("加经验", GUILayout.MinWidth(100), GUILayout.MaxHeight(20)))
        {
            Debug.Log("加经验");
            // AddExp(newExp);
        }

        //-------
        GUILayout.Label("VIP钱:");
        newVip = EditorGUILayout.IntField(newVip, GUILayout.ExpandWidth(true), GUILayout.MinHeight(20));
        if (GUILayout.Button("加VIP", GUILayout.MinWidth(100), GUILayout.MaxHeight(20)))
        {
            Debug.Log("加VIP");
            // AddVip(newVip);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("金币:");
        newCoin = EditorGUILayout.IntField(newCoin);
        if (GUILayout.Button("加金币", GUILayout.MinWidth(100), GUILayout.MaxHeight(20)))
        {
            // AddCoin(newCoin);
            Debug.Log("加金币");
        }

        //-------
        GUILayout.Label("元宝");
        newMoney = EditorGUILayout.IntField(newMoney);
        if (GUILayout.Button("加元宝", GUILayout.MinWidth(100), GUILayout.MaxHeight(20)))
        {
            Debug.Log("加元宝");
            // AddMoney(newMoney);
        }

        GUILayout.EndHorizontal();
        //后面继续....
    }
}