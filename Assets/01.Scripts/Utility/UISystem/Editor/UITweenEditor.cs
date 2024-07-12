using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UITween), true)]
    public class UITweenEditor : Editor
    {
        private UITweener _tweener;
        private UITween _tween;
        private UIComponentEditor _componentEditor;

        private int Index => _tweener == null ? 0 : _tweener.tweens.IndexOf(_tween);
        private bool _isFold;

        public void Init(UITweener tweener, UITween tween, UIComponentEditor componentEditor)
        {
            _tweener = tweener;
            _tween = tween;
            _componentEditor = componentEditor;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, new string[] { "m_Script" });
            serializedObject.ApplyModifiedProperties();
        }

        public void DrawTweenEditor()
        {
            EditorGUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(10);
                    _isFold = EditorGUILayout.Foldout(_isFold, _tween.GetType().Name);
                    GUILayout.Space(200);
                    if (GUILayout.Button("↑"))
                    {
                        UpBtnHandler();
                    }

                    if (GUILayout.Button("↓"))
                    {
                        DownButtonHandler();
                    }

                    if (GUILayout.Button("Remove"))
                    {
                        RemoveBtnHandler();
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (_isFold)
                {
                    GUILayout.Space(10);
                    OnInspectorGUI();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void UpBtnHandler()
        {
            if (Index - 1 < 0)
            {
                return;
            }

            _tweener.tweens.Swap(Index, Index - 1);
        }

        private void DownButtonHandler()
        {
            if (Index + 1 >= _tweener.tweens.Count)
            {
                return;
            }

            _tweener.tweens.Swap(Index, Index + 1);
        }

        private void RemoveBtnHandler()
        {
            _componentEditor.RemoveTween(_tweener, _tween);
        }
    }
