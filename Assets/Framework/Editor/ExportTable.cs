using Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using System;
using System.Reflection;


namespace YummyGame.Framework
{
    public class ExportTable
    {

        static string space = "    ";


        public static void ExportExcelForm(string filePath)
        {
            string[] files = Directory.GetFiles(Config.ExcelTablePath, "*.xlsx", SearchOption.AllDirectories);
            DataSet ds = OpenExcel(filePath);
            WriteLuaTable(ds, Path.GetFileNameWithoutExtension(filePath));
            AssetDatabase.Refresh();
        }


        static void WriteLuaTable(DataSet data, string fileName)
        {
            int rows = data.Tables[0].Rows.Count;
            int columns = data.Tables[0].Columns.Count;
            if (rows <= 3) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--this is exported by tools "+DateTime.Now.ToString());
            sb.AppendLine("local data = {");
            
            for (int i = 3; i < rows; i++)
            {
                StringBuilder subData = new StringBuilder();
                for (int j = 0; j < columns; j++)
                {
                    string attrname = data.Tables[0].Rows[1][j].ToString();
                    string attrvalue = data.Tables[0].Rows[i][j].ToString();
                    string attrtype = data.Tables[0].Rows[2][j].ToString();

                    if (string.IsNullOrEmpty(attrname) || string.IsNullOrEmpty(attrvalue) ||
                        string.IsNullOrEmpty(attrtype)) continue;
                    subData.Append(attrname + "=");
                    if (attrtype == "string")
                    {
                        subData.AppendFormat("'{0}', ", attrvalue);
                    }
                    else
                    {
                        subData.AppendFormat("{0}, ", attrvalue);
                    }
                }
                sb.AppendLine(string.Format("{2}[{0}] = {{{1}}},",i - 2, subData.ToString(),space));
            }
            sb.AppendLine("}");
            sb.Append("return data;");

            string outputPath = Path.Combine(Application.dataPath, Config.LuaSource, Config.LuaTableSource,fileName);
            outputPath = Path.ChangeExtension(outputPath, ".lua");

            string dir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            using(FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                byte[] write = Encoding.UTF8.GetBytes(sb.ToString());
                fs.Write(write, 0, write.Length);
            }
        }

        public static DataSet OpenExcel(string path)
        {
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result = excelReader.AsDataSet();
            stream.Dispose();
            return result;
        }
    }
}

