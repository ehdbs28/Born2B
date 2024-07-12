using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DefaultAsset))]
public class FolderEditor : Editor
{
    private bool _isValidFolder;
    private AssetImporter _importer;

    private FolderCommentWrapper _commentWrapper;
    
    private string _inputComment;
    private string _inputAuthor;

    private const string CommentFieldPlaceHolderText = "주석을 입력하세요.";
    private const string AuthorFieldPlaceHolderText = "작성자를 입력하세요.";

    private void OnEnable()
    {
        var path = AssetDatabase.GetAssetPath(target);
        _isValidFolder = AssetDatabase.IsValidFolder(path);
        _importer = AssetImporter.GetAtPath(path);

        if (string.IsNullOrEmpty(_importer.userData))
        {
            _commentWrapper = new FolderCommentWrapper();
            return;
        }
        
        var jsonData = JsonUtility.FromJson<FolderCommentWrapper>(_importer.userData);
        _commentWrapper = jsonData ?? new FolderCommentWrapper();
    }

    private void OnDisable()
    {
        var jsonText = JsonUtility.ToJson(_commentWrapper);
        _importer.userData = jsonText;
    }

    public override void OnInspectorGUI()
    {
        if (_isValidFolder)
        {
            DrawFolderInspector();
        }
        else
        {
            base.OnInspectorGUI();
        }
    }

    private void DrawFolderInspector()
    {
        GUI.enabled = true;
        
        GUILayout.Space(10);
        
        // Write Comment Info
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(5);
            
            var inspectorWidth = EditorGUIUtility.currentViewWidth - 12.5f;
            
            const float firstElementWidthRatio = 0.7f;

            var firstElementWidth = inspectorWidth * firstElementWidthRatio;
            var remainWidth = inspectorWidth * (1f - firstElementWidthRatio);
            
            DrawTextField(ref _inputComment, CommentFieldPlaceHolderText, GUILayout.Width(firstElementWidth));
            DrawTextField(ref _inputAuthor, AuthorFieldPlaceHolderText, GUILayout.Width(remainWidth));
            
            GUILayout.Space(5);
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(5);
            if (GUILayout.Button("Create New Comment"))
            {
                CreateNewComment();
            }
            GUILayout.Space(5);
        }
        GUILayout.EndHorizontal();
        
        GUILayout.Space(5);
        DrawHorizontalLine(Color.gray);
        GUILayout.Space(5);
        
        
        // Draw Comments
        GUILayout.Label("Comments", new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 16
        });
        GUILayout.Space(5);

        for (var i = 0; i < _commentWrapper.comments.Count; i++)
        {
            var comment = _commentWrapper.comments[i];
            DrawComment(comment.comment, comment.author, i);
            GUILayout.Space(2.5f);
        }
    }

    private void CreateNewComment()
    {
        if (_inputComment == CommentFieldPlaceHolderText || _inputAuthor == AuthorFieldPlaceHolderText)
        {
            Debug.LogWarning("비어 있는 필드를 입력해주세요.");
            return;
        }
        _commentWrapper.comments.Add(new FolderComment{comment = _inputComment, author = _inputAuthor});
    }

    private void DrawComment(string comment, string author, int index)
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        {
            GUILayout.BeginHorizontal();
            {
                var infoIcon = EditorGUIUtility.IconContent("console.infoicon");
                var iconRect = GUILayoutUtility.GetRect(30, 30, GUILayout.ExpandWidth(false));
                GUI.DrawTexture(iconRect, infoIcon.image);
                
                GUILayout.Space(10);
                
                GUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField(comment, new GUIStyle(EditorStyles.whiteLabel)
                    {
                        fontSize = 14,
                        wordWrap = true
                    });
                    GUILayout.Space(3);
                    EditorGUILayout.LabelField($"[ {author} ]", new GUIStyle(EditorStyles.miniLabel));
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                var inspectorWidth = EditorGUIUtility.currentViewWidth - 12.5f;
            
                const float emptyRatio = 0.7f;

                var emptyWidth = inspectorWidth * emptyRatio;
                var buttonWidth = inspectorWidth * (1f - emptyRatio);

                GUILayout.Space(emptyWidth);
                
                if (GUILayout.Button("Remove Comment", GUILayout.Width(buttonWidth)))
                {
                    _commentWrapper.comments.RemoveAt(index);
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private void DrawHorizontalLine(Color color, int thickness = 1, int padding = 10)
    {
        var rect = EditorGUILayout.GetControlRect(false, thickness + padding);
        rect.height = thickness;
        rect.y += padding / 2f;
        EditorGUI.DrawRect(rect, color);
    }

    private void DrawTextField(ref string refText, string placeHolderText, params GUILayoutOption[] layoutOptions)
    {
        var textFieldStyle = new GUIStyle(GUI.skin.textField);
        if (string.IsNullOrEmpty(refText))
        {
            textFieldStyle.normal.textColor = Color.gray;
            refText = placeHolderText;
        }
        
        EditorGUI.BeginChangeCheck();
        refText = EditorGUILayout.TextArea(refText, textFieldStyle, layoutOptions);
        if (EditorGUI.EndChangeCheck())
        {
            if (refText == placeHolderText)
            {
                refText = "";
            }
        }

        if (!EditorGUIUtility.editingTextField && string.IsNullOrEmpty(refText))
        {
            refText = placeHolderText;
        }
    }
}