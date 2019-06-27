using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateGrid : EditorWindow
{

    [MenuItem("Tools/CreateGrid")]
    static void MyCreateGrid()
    {
        //创建窗口
        Rect wr = new Rect(600, 600, 600, 600);
        CreateGrid window = (CreateGrid)EditorWindow.GetWindowWithRect(typeof(CreateGrid), wr, true, "CreateGrid");
        window.Show();
    }

    string gridName;
    GameObject prefab;
    Transform parent;
    int elementLenght = 1;

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("地图元素:", GUILayout.Width(55));
        prefab = (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), true, GUILayout.Width(200));

        EditorGUILayout.LabelField("Parent:", GUILayout.Width(50));
        parent = (Transform)EditorGUILayout.ObjectField(parent, typeof(Transform), false, GUILayout.Width(200));

        GUILayout.EndHorizontal();
        EditorGUILayout.Space();//空一行

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("名字:", GUILayout.Width(30));
        gridName = EditorGUILayout.TextField(gridName, GUILayout.Width(200));

        EditorGUILayout.LabelField("元素长度:", GUILayout.Width(60));
        string tempLenght = string.Empty;
        tempLenght = EditorGUILayout.TextField(tempLenght, GUILayout.Width(200));
        if(int.TryParse(tempLenght, out elementLenght))
        {
            throw new System.Exception("输入的长度字符不是数字！");
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();//空一行
        if (GUILayout.Button("生成测试GO", GUILayout.ExpandWidth(true)))
        {
            parent.name = gridName;
            GameObject tempGO = Instantiate(prefab);
            tempGO.transform.SetParent(parent);
        }
    }
}
