using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace YummyGame.Framework
{
    public static class Utility
    {
        /// <summary>
        /// 对文件流进行MD5加密
        /// </summary>
        public static string MD5Stream(Stream stream)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(stream); 
            byte[] b = md5.Hash;
            md5.Clear();
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i<b.Length; i++)
            {
                sb.Append(b[i].ToString("X2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 对文件进行MD5加密
        /// </summary>
        public static string MD5Stream(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open,FileAccess.Read))
            {
                return MD5Stream(stream); 
            }
        }

        /// <summary>
        /// 獲取文件大小
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static long GetFileLength(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open,FileAccess.Read))
            {
                return stream.Length;
            }
        }

        public static void CopyFile(string source,string des)
        {
            FileInfo file = new FileInfo(source);
            if (file.Exists)
            {
                string dir = Path.GetDirectoryName(des);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                file.CopyTo(des, true);
            }
        }

        public static void CreateDirectoryEmpty(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }

        public static void CreateFile(string path)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            fs.Close();
        }

        public static string PathCombile(params string[] paths)
        {
            string str = Path.Combine(paths);
            return str.Replace("\\", "/");
        }

        public static string NormalPathToUnity(string path)
        {
            return PathCombile("Assets", path.Substring(Application.dataPath.Length + 1));
        }

        public static string UnityToNormalPath(string path)
        {
            return PathCombile(Application.dataPath, path.Substring(7));
        }

        public static void CopyDirectory(string sourceDir,string targetDir)
        {
            if (!Directory.Exists(sourceDir)) return;
            if (Directory.Exists(targetDir)) Directory.Delete(targetDir,true);
            Directory.CreateDirectory(targetDir);
            string[] files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string relativepath = file.Substring(sourceDir.Length + 1);
                string targetFile = Utility.PathCombile(targetDir, relativepath);
                CopyFile(file, targetFile);
            }
        }
    }

}

