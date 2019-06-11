using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ImporterSpriterSetting : EditorWindow
{

    [MenuItem("Yummy/快速设置精灵")]
    static void ImporterSpriterSettingWindow()
    {
        //创建窗口
        Rect wr = new Rect(500, 500, 500, 500);
        ImporterSpriterSetting window = (ImporterSpriterSetting)EditorWindow.GetWindowWithRect(typeof(ImporterSpriterSetting), wr, false, "ImporterSpriterSetting");
        window.Show();
    }

    TextureImporterType textureType = TextureImporterType.Sprite;
    SpriteImportMode spriteMode = SpriteImportMode.Single;
    string packingTag = string.Empty;
    bool sRGBTexture = true;
    bool alphaIsTransparency = true;
    bool mipmapEnabled = false;
    TextureWrapMode wrapMode = TextureWrapMode.Clamp;
    FilterMode filterMode = FilterMode.Bilinear;

    TextureImporterFormat textureImporterFormatAndroid = TextureImporterFormat.ETC2_RGBA8;
    TextureImporterFormat textureImporterFormatiPhone = TextureImporterFormat.PVRTC_RGBA4;
    TextureImporterFormat textureImporterFormatStandalone = TextureImporterFormat.DXT5;

    void OnGUI()
    {

        textureType = (TextureImporterType)EditorGUILayout.EnumPopup("Texture Type", textureType);
        EditorGUILayout.Space();//空一行

        spriteMode = (SpriteImportMode)EditorGUILayout.EnumPopup("Sprite Mode", spriteMode);
        EditorGUILayout.Space();//空一行

        packingTag = EditorGUILayout.TextField("Packing Tag", packingTag);
        EditorGUILayout.Space();//空一行

        sRGBTexture = EditorGUILayout.Toggle("S RGB Texture", sRGBTexture);
        EditorGUILayout.Space();//空一行

        alphaIsTransparency = EditorGUILayout.Toggle("Alpha Is Transparency", alphaIsTransparency);
        EditorGUILayout.Space();//空一行

        mipmapEnabled = EditorGUILayout.Toggle("Generate Mip Map", mipmapEnabled);
        EditorGUILayout.Space();//空一行

        wrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup("Wrap Mode", wrapMode);
        EditorGUILayout.Space();//空一行

        filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", filterMode);
        EditorGUILayout.Space();//空一行

        textureImporterFormatAndroid = (TextureImporterFormat)EditorGUILayout.EnumPopup("Texture Importer Format For Android", textureImporterFormatAndroid);
        EditorGUILayout.Space();//空一行

        textureImporterFormatiPhone = (TextureImporterFormat)EditorGUILayout.EnumPopup("Texture Importer Format For iPhone", textureImporterFormatiPhone);
        EditorGUILayout.Space();//空一行

        textureImporterFormatStandalone = (TextureImporterFormat)EditorGUILayout.EnumPopup("Texture Importer Format For Standalone", textureImporterFormatStandalone);
        EditorGUILayout.Space();//空一行



        if (GUILayout.Button("设置Sprite", GUILayout.ExpandWidth(true)))
        {
            Object[] targetObj = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            if (targetObj != null && targetObj.Length > 0)
            {
                for (int i = 0; i < targetObj.Length; i++)
                {
                    if (targetObj[i] is Texture)
                    {
                        string path = AssetDatabase.GetAssetPath(targetObj[i]);
                        TextureImporter texture = AssetImporter.GetAtPath(path) as TextureImporter;
                        texture.textureType = textureType;
                        texture.spriteImportMode = spriteMode;
                        texture.spritePixelsPerUnit = 100;
                        texture.sRGBTexture = sRGBTexture;
                        texture.alphaIsTransparency = alphaIsTransparency;
                        texture.filterMode = filterMode;
                        texture.mipmapEnabled = mipmapEnabled;
                        texture.wrapMode = wrapMode;

                        TextureImporterPlatformSettings platformSetting = new TextureImporterPlatformSettings();
                        platformSetting.name = "Android";
                        platformSetting.format = textureImporterFormatAndroid;
                        platformSetting.overridden = true;
                        platformSetting.maxTextureSize = 2048;
                        texture.SetPlatformTextureSettings(platformSetting);

                        TextureImporterPlatformSettings platformSettingIP = new TextureImporterPlatformSettings();
                        platformSettingIP.name = "iPhone";
                        platformSettingIP.format = textureImporterFormatiPhone;
                        platformSettingIP.overridden = true;
                        platformSettingIP.maxTextureSize = 2048;
                        texture.SetPlatformTextureSettings(platformSettingIP);

                        TextureImporterPlatformSettings platformSettingStandalone = new TextureImporterPlatformSettings();
                        platformSettingStandalone.name = "Standalone";
                        platformSettingStandalone.format = textureImporterFormatStandalone;
                        platformSettingStandalone.overridden = true;
                        platformSettingStandalone.maxTextureSize = 2048;
                        texture.SetPlatformTextureSettings(platformSettingStandalone);

                        AssetDatabase.ImportAsset(path);
                    }
                }
            }
        }
    }
}
