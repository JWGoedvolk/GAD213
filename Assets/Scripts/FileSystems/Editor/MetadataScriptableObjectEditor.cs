using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MetaDataScriptableObject))]
public class MetadataScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MetaDataScriptableObject metadata = (MetaDataScriptableObject)target;

        if(GUILayout.Button("Export to Json"))
        {
            ExportToJson(metadata);
            EditorUtility.SetDirty(metadata);
        }
    }

    private void ExportToJson(MetaDataScriptableObject metadata)
    {
        MetadataFile metadataFile = new MetadataFile
        {
            version = metadata.version,
            fileLink = metadata.associatedFileLink
        };

        string json = JsonUtility.ToJson(metadataFile, true);
        Directory.CreateDirectory(Application.streamingAssetsPath);
        File.WriteAllText(metadata.LocalMetadataFilePath, json);
        Debug.Log($"Metadata exported at: {metadata.LocalMetadataFilePath}");
    }
}
