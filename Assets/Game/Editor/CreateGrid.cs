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

    void OnGUI()
    {
        SpawnGround();
        SpawnWall();
        EnterSpawn();
    }

    string gridName;
    GameObject prefab;
    GameObject parent;
    int elementLenghtSide = 1;

    int width = 0;
    int length = 0;

    string strWidth = string.Empty;
    string strLenght = string.Empty;
    string tempLenght = string.Empty;

    void EnterSpawn()
    {
        EditorGUILayout.Space();//空一行
        if (GUILayout.Button("生成测试GO", GUILayout.ExpandWidth(true)))
        {
            parent.name = gridName;
            GameObject tempGO;

            StrConvertInt32(tempLenght, out elementLenghtSide);
            StrConvertInt32(strLenght, out length);
            StrConvertInt32(strWidth, out width);
            StrConvertInt32(strNoWall, out noWallCount);
            StrConvertInt32(strWallHeight, out wallHeight);

            if (length <= 0 || width <= 0) return;

            #region 生成地板、围墙
            GameObject wallParent = new GameObject("WallParent");
            wallParent.transform.SetParent(parent.transform);
            GameObject groundParent = new GameObject("GroundParent");
            groundParent.transform.SetParent(parent.transform);
            int lengthStartIndex = ((length / 2)+1) - (int)(noWallCount / 2.0f);
            int lengthEndIndex = ((length / 2+1)) + (int)(noWallCount / 2.0f);

            int widthhStartIndex = ((width / 2) + 1) - (int)(noWallCount / 2.0f);
            int widthEndIndex = ((width / 2) + 1) + (int)(noWallCount / 2.0f);

            for (int i = 0; i < length; i++)//x
            {
                for (int j = 0; j < width; j++)//y
                {
                    tempGO = Instantiate(prefab);
                    tempGO.transform.SetParent(groundParent.transform);
                    tempGO.transform.localPosition = new Vector3(elementLenghtSide * i, 0, elementLenghtSide * j);

                    if ((i == 0 || i == length - 1) || (j == 0 || j == width - 1))
                    {
                        if ((i >= lengthStartIndex - 1 && i <= lengthEndIndex - 1) || (j >= widthhStartIndex - 1 && j <= widthEndIndex - 1)) continue;
                        wallParent.transform.localPosition = Vector3.zero;
                        tempGO = Instantiate(wallPrefab);
                        tempGO.transform.SetParent(wallParent.transform);
                        tempGO.transform.localPosition = new Vector3(elementLenghtSide * i, wallHeight, elementLenghtSide * j);
                    }
                }
            }
            #endregion
        }
    }

    void SpawnGround()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("地图元素:", GUILayout.Width(55));
        prefab = (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), true, GUILayout.Width(200));

        EditorGUILayout.LabelField("Parent:", GUILayout.Width(50));
        parent = (GameObject)EditorGUILayout.ObjectField(parent, typeof(GameObject), true, GUILayout.Width(200));

        GUILayout.EndHorizontal();
        EditorGUILayout.Space();//空一行

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("名字:", GUILayout.Width(30));
        gridName = EditorGUILayout.TextField(gridName, GUILayout.Width(200));

        EditorGUILayout.LabelField("元素长度:", GUILayout.Width(60));
        tempLenght = EditorGUILayout.TextField(tempLenght, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();//空一行

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("地图长度:", GUILayout.Width(60));
        strLenght = EditorGUILayout.TextField(strLenght, GUILayout.Width(200));

        EditorGUILayout.LabelField("地图宽度:", GUILayout.Width(60));
        strWidth = EditorGUILayout.TextField(strWidth, GUILayout.Width(200));

        GUILayout.EndHorizontal();
    }

    GameObject wallPrefab;

    string strNoWall = string.Empty;
    int noWallCount = 0;

    string strWallHeight = string.Empty;
    int wallHeight = 0;
    void SpawnWall()
    {
        EditorGUILayout.Space();//空一行

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("围墙元素:", GUILayout.Width(55));
        wallPrefab = (GameObject)EditorGUILayout.ObjectField(wallPrefab, typeof(GameObject), true, GUILayout.Width(200)); ;

        //EditorGUILayout.LabelField("Parent:", GUILayout.Width(50));
        //parent = (GameObject)EditorGUILayout.ObjectField(parent, typeof(GameObject), true, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();//空一行
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("不生成墙的长度:", GUILayout.Width(80));
        strNoWall = EditorGUILayout.TextField(strNoWall, GUILayout.Width(200));
        EditorGUILayout.LabelField("地板高度:", GUILayout.Width(50));
        strWallHeight = EditorGUILayout.TextField(strWallHeight, GUILayout.Width(200));
        GUILayout.EndHorizontal();
    }

    void StrConvertInt32(string str, out int num)
    {
        if (!int.TryParse(str, out num))
        {
            throw new System.Exception(str + ":输入的字符不是数字！");
        }
    }
}
