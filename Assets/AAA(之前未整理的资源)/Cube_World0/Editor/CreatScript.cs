using UnityEditor;
using UnityEngine;
using System.IO;

public class CreatScript : EditorWindow
{
    private static string scriptPath;
    private static string constScriptPath;
    private static string className;
    private static GameObject go;


    [MenuItem("Tools/CreatScript")]
    public static void CreateScript()
    {
        CreatScript window = (CreatScript) GetWindow(typeof(CreatScript));
        window.Show();
    }

    private void OnGUI()
    {
        LoadPathData();
        CreateDragLable("拖拽脚本生成路径:", ref scriptPath);
        CreateDragLable("常量脚本路径:", ref constScriptPath);
        CreateLable("脚本类名", ref className);
        CreateGameObjectLable("View物体", ref go);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("创建脚本", GUILayout.Width(100)))
        {
            if (className == null)
            {
                Debug.LogError("类名不能为空");
                return;
            }

            WriteScript();
        }

        if (GUILayout.Button("保存数据", GUILayout.Width(100)))
        {
            SavePathData();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void CreateDragLable(string labelName, ref string path)
    {
        GUILayout.Label(labelName);
        Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(300));
        path = EditorGUI.TextField(rect, path);
        MyDragAndDrop(rect, ref path);
    }

    private void CreateLable(string labelName, ref string path)
    {
        GUILayout.Label(labelName);
        Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(200));
        path = EditorGUI.TextField(rect, path);
    }

    private void CreateGameObjectLable(string labelName, ref GameObject path)
    {
        path = (GameObject) EditorGUILayout.ObjectField(labelName, path, typeof(GameObject), true);
    }


    private void MyDragAndDrop(Rect rect, ref string path)
    {
        if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited) &&
            rect.Contains(Event.current.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                path = DragAndDrop.paths[0];
        }
    }

    private void SavePathData()
    {
        CreatScriptData data = CreateInstance<CreatScriptData>();
        data.path = scriptPath;
        data.constScriptPath = constScriptPath;
        AssetDatabase.CreateAsset(data, "Assets/MMIUGUI/Editor/pathData.asset");
    }

    private void LoadPathData()
    {
        var data = AssetDatabase.LoadAssetAtPath<CreatScriptData>("Assets/MMIUGUI/Editor/pathData.asset");
        if (data != null)
        {
            scriptPath = data.path;
            constScriptPath = data.constScriptPath;
        }
    }

    private void WriteScript()
    {
    }
}