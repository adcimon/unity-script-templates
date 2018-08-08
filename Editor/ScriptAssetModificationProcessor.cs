using System.IO;
using UnityEngine;
using UnityEditor;

public class ScriptAssetModificationProcessor : UnityEditor.AssetModificationProcessor
{
    private static void OnWillCreateAsset( string assetName )
    {
        assetName = assetName.Replace(".meta", "");
        int index = assetName.LastIndexOf(".");
        if( index < 0 )
        {
            return;
        }

        string extension = assetName.Substring(index);
        if( extension != ".cs" )
        {
            return;
        }

        index = Application.dataPath.LastIndexOf("Assets");
        string assetPath = Application.dataPath.Substring(0, index) + assetName;
        if( !File.Exists(assetPath) )
        {
            return;
        }

        string fileContent = File.ReadAllText(assetPath);
        fileContent = fileContent.Replace("#TARGETCLASS#", assetName.Replace("Assets/", "").Replace("Inspector.cs", ""));

        File.WriteAllText(assetPath, fileContent);
        AssetDatabase.Refresh();
    }
}