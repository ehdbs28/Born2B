using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIComponent), true)]
public class UIComponentEditor : Editor
{
    public UIComponent Component { get; private set; }

    private List<Type> _tweenTypes; // READONLY
    private Dictionary<UITween, UITweenEditor> _uiTweenEditors; // READONLY

    private void OnEnable()
    {
        if (target == null)
        {
            return;
        }

        Component = (UIComponent)target;

        _tweenTypes = new List<Type>();
        _uiTweenEditors = new Dictionary<UITween, UITweenEditor>();

        Init();
    }

    public override void OnInspectorGUI()
    {
        var tweenData = Component.tweenData;
        base.OnInspectorGUI();
        if (tweenData is null && Component.tweenData is not null)
        {
            Init();
        }

        if (Component.tweenData != null)
        {
            DrawTweenerInspector("Appear Tweener", ref Component.tweenData.appearTweener);
            DrawTweenerInspector("Disappear Tweener", ref Component.tweenData.disappearTweener);
        }
        else
        {
            DrawCreateDataButton();
        }
    }

    private void Init()
    {
        if (Component.tweenData != null)
        {
            InitLoadDictionary(Component.tweenData.appearTweener);
            InitLoadDictionary(Component.tweenData.disappearTweener);
        }

        LoadUITweenType();
    }

    private string _createPath;
    private void DrawCreateDataButton()
    {
        GUILayout.Space(20);

        var assetPath = $"{_createPath}/{Component.gameObject.name}TweenData.asset";
        
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            GUILayout.Label($"Create Path : [ {assetPath} ]");
        }
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Select Create Folder"))
        {
            _createPath = EditorUtility.OpenFolderPanel("Select Create Folder", "", "");
        }
        
        if (GUILayout.Button("Create New Data"))
        {
            CreateNewTweenData(assetPath);
        }
    }

    private void CreateNewTweenData(string assetPath)
    {
        var keyword = "Assets";
        var index = assetPath.IndexOf(keyword, StringComparison.Ordinal);
        
        if (index != -1)
        {
            assetPath = assetPath.Substring(index);
        }

        if (string.IsNullOrEmpty(assetPath))
        {
            Debug.LogWarning("You must select create asset folder");
            return;
        }
        
        var uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

        var asset = ScriptableObject.CreateInstance<UIComponentTweenData>();
        asset.appearTweener = new UITweener();
        asset.disappearTweener = new UITweener();

        AssetDatabase.CreateAsset(asset, uniqueAssetPath);
        EditorUtility.SetDirty(asset);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

        Component.tweenData = asset;
        EditorUtility.SetDirty(Component);
    }

    private void DrawTweenerInspector(string title, ref UITweener tweener)
    {
        EditorGUILayout.Space(10);
        GUILayout.Label(title, EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.Space(2);

            EditorGUILayout.Space(1f);
            var idx = 0;
            while (idx < tweener.tweens.Count)
            {
                _uiTweenEditors[tweener.tweens[idx++]].DrawTweenEditor();
            }

            EditorGUILayout.Space(1f);

            EditorGUILayout.BeginHorizontal();
            {
                if (Application.isPlaying && tweener.tweens.Count > 0)
                {
                    if (GUILayout.Button("Preview"))
                    {
                        if (!tweener.IsSet)
                        {
                        }
                        tweener.Init(Component);
                        tweener.PlayTween();
                    }
                }

                if (GUILayout.Button("Add Animation Clip"))
                {
                    GetAnimationTypeMenu(tweener, out var menu);
                    menu.ShowAsContext();
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(2);
        }
        EditorGUILayout.EndVertical();
    }

    private void GetAnimationTypeMenu(UITweener tweener, out GenericMenu menu)
    {
        menu = new GenericMenu();
        foreach (var type in _tweenTypes)
        {
            menu.AddItem(new GUIContent(type.Name), false, () => { AddTween(tweener, GenerateTweenAsset(type)); });
        }
    }

    private UITween GenerateTweenAsset(Type type)
    {
        var tween = ScriptableObject.CreateInstance(type);
        tween.name = type.ToString();
        AssetDatabase.AddObjectToAsset(tween, Component.tweenData);
        EditorUtility.SetDirty(Component.tweenData);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        return (UITween)tween;
    }

    private void AddTween(UITweener tweener, UITween clip)
    {
        tweener.tweens.Add(clip);
        AddTweenToEditor(tweener, clip);

        EditorUtility.SetDirty(Component.tweenData);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    private void AddTweenToEditor(UITweener tweener, UITween tween)
    {
        var editor = (UITweenEditor)CreateEditor(tween, typeof(UITweenEditor));
        editor.Init(tweener, tween, this);
        _uiTweenEditors.Add(tween, editor);
    }

    public void RemoveTween(UITweener tweener, UITween tween)
    {
        tweener.tweens.Remove(tween);
        _uiTweenEditors.Remove(tween);

        Undo.DestroyObjectImmediate(tween);
        EditorUtility.SetDirty(Component.tweenData);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    private void InitLoadDictionary(UITweener tweener)
    {
        for (var i = 0; i < tweener.tweens.Count; i++)
        {
            AddTweenToEditor(tweener, tweener.tweens[i]);
        }
    }

    private void LoadUITweenType()
    {
        var assembly = Assembly.GetAssembly(typeof(UITween));
        var types = assembly.GetTypes();

        foreach (var type in types)
        {
            if (type.IsSubclassOf(typeof(UITween)))
            {
                _tweenTypes.Add(type);
            }
        }
    }
}