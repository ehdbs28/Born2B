using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolManager))]
public class PoolManagerEditor : Editor
{
    private PoolManager _poolManager;

    private string _enumScriptName;
    private string _enumScriptCreatePath;

    private string _fullPath;

    private void OnEnable()
    {
        _poolManager = (PoolManager)target;

        _enumScriptName = "PoolingItemType";
        _enumScriptCreatePath = $"{Application.dataPath}/01.Scripts/Utils/PoolingSystem/Enum";
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.Space(10);
        GUILayout.Label("Create Pool Item Enum Script");
        
        _enumScriptName = EditorGUILayout.TextField("", _enumScriptName);
        GUILayout.Space(5);

        if (GUILayout.Button("Select Asset Generate Folder"))
        {
            _enumScriptCreatePath = EditorUtility.OpenFolderPanel("Select Enum Script Generate Folder", "", "");
        }

        _fullPath = $"{_enumScriptCreatePath}/{_enumScriptName}.cs";
        
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            GUILayout.Label($"Full Path : [ {_fullPath} ]");
        }
        EditorGUILayout.EndVertical();
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("Generate"))
        {
            GenerateEnumScript();
        }
    }

    private void GenerateEnumScript()
    {
        if (File.Exists(_fullPath))
        {
            File.Delete(_fullPath);
        }
        var file = File.Create(_fullPath);
        file.Close();

        var streamWriter = new StreamWriter(_fullPath);
        {
            streamWriter.WriteLine($"public enum {_enumScriptName}");
            streamWriter.WriteLine("{");
            foreach (var pair in _poolManager.PoolingLists.SelectMany(poolingList => poolingList.poolableItems))
            {
                var prefabName = pair.prefab.name;
                streamWriter.WriteLine($"\t{prefabName},");
            }
            streamWriter.WriteLine("}");
        }
        streamWriter.Flush();
        streamWriter.Close();
        
        AssetDatabase.Refresh();
    }
}