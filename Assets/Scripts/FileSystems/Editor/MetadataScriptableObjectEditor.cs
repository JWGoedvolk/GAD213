using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using JW.GPG.CloudSave;

[CustomEditor(typeof(MetaDataScriptableObject))]
public class MetadataScriptableObjectEditor : Editor
{
    public long NowTick = 0;
    public override void OnInspectorGUI()
    {
        MetaDataScriptableObject metadata = (MetaDataScriptableObject)target;
        if (metadata.tick == 0)
        {
            metadata.SetupLocalMetaData();
        }
        DrawDefaultInspector();
        EditorGUILayout.LongField("Current Tick", NowTick);


        if(GUILayout.Button("Export to Json"))
        {
            ExportToJson(metadata);
            EditorUtility.SetDirty(metadata);
        }
        if (GUILayout.Button("Get current tick"))
        {
            UpdateNowTick();
            EditorUtility.SetDirty(this);
        }
    }

    private void ExportToJson(MetaDataScriptableObject metadata)
    {
        UpdateNowTick();
        MetadataFile metadataFile = new MetadataFile
        {
            ticks = NowTick,
            fileLink = metadata.associatedFileLink
        };
        Debug.Log(metadataFile.ticks);

        string json = JsonUtility.ToJson(metadataFile, true);
        Directory.CreateDirectory(Application.streamingAssetsPath);
        File.WriteAllText(metadata.LocalMetadataFilePath, json);
        metadata.tick = NowTick;
        Debug.Log($"Metadata exported at: {metadata.LocalMetadataFilePath}");
    }

    private void UpdateNowTick()
    {
        NowTick = DateTime.Now.Ticks;
    }
}
