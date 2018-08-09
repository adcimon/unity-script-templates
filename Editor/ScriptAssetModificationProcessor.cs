using System.IO;
using UnityEngine;
using UnityEditor;

public class ScriptAssetModificationProcessor : UnityEditor.AssetModificationProcessor
{
    private static void OnWillCreateAsset( string assetName )
    {
        // Remove the '.meta' extension.
        assetName = assetName.Replace(".meta", "");
        int index = assetName.LastIndexOf(".");
        if( index < 0 )
        {
            return;
        }

        // Check the file extension.
        string extension = assetName.Substring(index);
        if( extension != ".cs" )
        {
            return;
        }

        // Get the absolute path.
        index = Application.dataPath.LastIndexOf("Assets");
        string path = Application.dataPath.Substring(0, index) + assetName;
        if( !File.Exists(path) )
        {
            return;
        }

        // String substitution.
        string content = File.ReadAllText(path);
        index = assetName.LastIndexOf("/");
        content = content.Replace("#TARGETCLASS#", assetName.Substring(index + 1, assetName.Length - index - 1).Replace("Inspector.cs", ""));
        File.WriteAllText(path, content);

        AssetDatabase.Refresh();
    }
}