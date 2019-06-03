using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace YummyGame.Framework
{
    public class ExportTableWindow:EditorWindow
    {
        private string[] files;
        Vector2 scroll;
        [MenuItem("Yummy/窗体/表格")]
        public static void OpenWindow()
        {
            ExportTableWindow window = EditorWindow.GetWindow<ExportTableWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            _init();
        }

        private void OnGUI()
        {
            if (files == null) return;
            scroll = EditorGUILayout.BeginScrollView(scroll);
            if (GUILayout.Button("全部"))
            {
                foreach (var file in files)
                {
                    ExportTable.ExportExcelForm(file);
                }
            }
            foreach (var file in files)
            {
                string showPath = file.Substring(Config.ExcelTablePath.Length + 1);
                if (GUILayout.Button(showPath))
                {
                    ExportTable.ExportExcelForm(file);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void _init()
        {
            if (!Directory.Exists(Config.ExcelTablePath)) return;
            files = Directory.GetFiles(Config.ExcelTablePath, "*.xlsx", SearchOption.AllDirectories);
        }
    }
}
