using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace YummyGame.Framework
{
    public class TexturePacker
    {
        [MenuItem("Yummy/自动打图集")]
        public static void CreataAtlas()
        {
            string atlasRelativePath = Utility.PathCombile("Assets", Config.AtlasRoot);
            string atlasPath = Utility.PathCombile(Application.dataPath, Config.AtlasRoot);

            if (!Directory.Exists(atlasPath)) return;
            string[] dirs = Directory.GetDirectories(atlasPath,"*",SearchOption.AllDirectories);
            foreach (var dir in dirs)
            {
                _createOneAtlas(dir);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/图集/打包指定文件夹")]
        public static void CreataAtlasFolder()
        {
            string[] guids = Selection.assetGUIDs;
            foreach (var guid in guids)
            {
                string path = Utility.UnityToNormalPath(
                    AssetDatabase.GUIDToAssetPath(guid));
                _createOneAtlas(path);
            }
        }

        private static void _createOneAtlas(string path)
        {
            string[] files = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly);
            
            List<Sprite> m_sprites = new List<Sprite>();
            foreach (var file in files)
            {
                string pngPath = Utility.NormalPathToUnity(file);
                Sprite s= AssetDatabase.LoadAssetAtPath<Sprite>(pngPath);
                if (s == null)
                {
                    Debug.Log("格式不正确：" + pngPath);
                    continue;
                }
                m_sprites.Add(s);
            }
            if (m_sprites.Count == 0) return;
            
            SpriteAtlas atlas = new SpriteAtlas();
            SpriteAtlasPackingSettings packSetting = new SpriteAtlasPackingSettings()
            {
                blockOffset = 1,
                enableRotation = false,
                enableTightPacking = false,
                padding = 2,
            };
            atlas.SetPackingSettings(packSetting);

            SpriteAtlasTextureSettings textureSetting = new SpriteAtlasTextureSettings()
            {
                readable = false,
                generateMipMaps = false,
                sRGB = true,
                filterMode = FilterMode.Bilinear,
            };
            atlas.SetTextureSettings(textureSetting);

            TextureImporterPlatformSettings platformSetting = new TextureImporterPlatformSettings()
            {
                maxTextureSize = 2048,
                format = TextureImporterFormat.Automatic,
                crunchedCompression = true,
                textureCompression = TextureImporterCompression.Compressed,
                compressionQuality = 50,
            };
            atlas.SetPlatformSettings(platformSetting);
            atlas.Add(m_sprites.ToArray());
            string createPath = Utility.PathCombile(Utility.NormalPathToUnity(path)
                ,Path.GetFileName(path))
                + ".spriteatlas";
            AssetDatabase.DeleteAsset(createPath);
            AssetDatabase.CreateAsset(atlas, createPath);
        }
    }
}
