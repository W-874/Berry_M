#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BM.Utils.Tools
{
    [UnityEditor.AssetImporters.ScriptedImporter(1, ".pdrf")]
    public class PdrfImporter : UnityEditor.AssetImporters.ScriptedImporter
    {
        public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
        {
            var pdrfTxt = File.ReadAllText(ctx.assetPath);
            var assetsText = new TextAsset(pdrfTxt);
            ctx.AddObjectToAsset("main obj", assetsText);
            ctx.SetMainObject(assetsText);
        }
    }
    public class PdrfPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string str in importedAssets)
            {
                if (str.EndsWith(".pdrf"))
                {
                    var obj = AssetDatabase.LoadAssetAtPath<Object>(str);
                    AssetDatabase.SetLabels(obj, new string[] { "BM Chart Format" });
                }
            }
        }
    }
}
#endif