using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class data1AssetPostprocessor : AssetPostprocessor
{
    private static readonly string filePath = "Assets/ExcelData/Data.xlsx";
    private static readonly string assetFilePath = "Assets/ExcelData/data1.asset";
    private static readonly string sheetName = "data1";

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (!filePath.Equals(asset))
                continue;

            data1 data = (data1)AssetDatabase.LoadAssetAtPath(assetFilePath, typeof(data1));
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<data1>();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }

            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<data1Data>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<data1Data>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath(assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty(obj);
            }
        }
    }
}
