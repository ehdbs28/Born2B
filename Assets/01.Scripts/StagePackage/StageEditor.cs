using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using StageDefine;
using Random = UnityEngine.Random;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UIElements.Button;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class CellVisual : VisualElement
{
    private Vector2Int index;
    public Vector2Int Index => index;
    public int textureIdx = -1;
    public string current;
    Func<string, Vector2Int, string> cellChangeCallBack;
    private StageEditor stageEditor;
    private int maxIdx => stageEditor.chapterData.stageObjectSlotList.Count - 1;

    public CellVisual(Vector2 pos, Vector2 size, Vector2Int index, string crt,
                      Func<string, Vector2Int, string> cellChangeCallBack, Vector2 winsize, StageEditor editor)
    {
        this.index = index;
        this.cellChangeCallBack = cellChangeCallBack;
        current = crt;

        style.width = size.x;
        style.height = size.y;

        style.position = Position.Absolute;
        transform.position = pos + new Vector2(1920, 1080 / 2) / 4 + new Vector2(0, 100);
        stageEditor = editor;

        RegisterCallback<PointerDownEvent>(HandleClick);

        ChangeVisual(crt);
    }

    private void HandleClick(PointerDownEvent evt)
    {
        if (evt.button == 0)
        {
            if (textureIdx < maxIdx)
            {
                textureIdx++;
            }
            else
            {
                textureIdx = -1;
            }
        }

        if (evt.button == 1)
        {
            if (textureIdx > -1)
            {
                textureIdx--;
            }
            else
            {
                textureIdx = maxIdx;
            }
        }

        if (evt.button == 2 && current == "PT")
        {
            if (stageEditor.onPortalEdit)
            {
                stageEditor.EndPortalEdit(this);
            }
            else
            {
                stageEditor.StartPortalEdit(this);
            }

            return;
        }

        if (textureIdx == -1)
        {
            current = string.Empty;
        }
        else
        {
            current = stageEditor.chapterData.stageObjectSlotList[textureIdx].objectName;
        }

        cellChangeCallBack(current, index);
        ChangeVisual(current);
    }

    public void ChangeVisual(string value)
    {
        var old = style.backgroundImage;

        string texPath;
        if (value == string.Empty)
        {
            texPath = "NormalSlot";
        }
        else
        {
            texPath = value;
        }

        old.value =
        Background.FromTexture2D(Resources.Load<Texture2D>($"StageEditorTexture/{texPath}"));

        style.backgroundImage = old;
    }
}

public class StageEditor : EditorWindow
{
    private DropdownField chapterDropdwon;

    private Button btnSave;
    private Button btnCreate;
    private TextField widthField;
    private TextField heightField;
    private TextField mapNameField;

    private List<CellVisual> cellVisuals = new();
    private static StageDataSO data;
    public ChapterDataSO chapterData;
    private string[,] mapData = new string[0, 0];

    private PortalDataSO portalData;
    private CellVisual selectPortal;
    public bool onPortalEdit;

    [MenuItem("Window/StageEditor")]
    public static void OpenWindow()
    {
        data = null;
        CreateWindow<StageEditor>();
    }

    private static void OpenWindow(StageDataSO dataSo)
    {
        data = dataSo;
        CreateWindow<StageEditor>();
    }

    public void StartPortalEdit(CellVisual portalCell)
    {
        onPortalEdit = true;

        selectPortal = portalCell;
    }

    public void EndPortalEdit(CellVisual portalCell)
    {
        onPortalEdit = false;

        Color color = Random.ColorHSV();
        portalCell.style.color = color;
        portalCell.style.color = color;
        selectPortal.style.color = color;

        if (portalData == null)
        {
            portalData = ScriptableObject.CreateInstance<PortalDataSO>();
            AssetDatabase.CreateAsset(portalData, $"Assets/Resources/PortalData/{mapNameField.value}_portalData.asset");
        }

        portalData.SetPortalLinkData(selectPortal.Index, portalCell.Index);

        EditorUtility.SetDirty(portalData);
        AssetDatabase.SaveAssets();

        data.SetPortalData(portalData);

        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
    }

    private void OnEnable()
    {
        List<string> typeList = new List<string>();

        foreach(var type in Enum.GetValues(typeof(ChapterType)))
        {
            typeList.Add(type.ToString());
        }

        chapterDropdwon = new DropdownField("챕터 선택", typeList, 0);
        chapterDropdwon.RegisterCallback<ChangeEvent<string>>((evt)=>
        {
            chapterData = Resources.Load<ChapterDataSO>($"ChapterData/{evt.newValue}");

            foreach (var cell in cellVisuals)
            {
                cell.textureIdx = -1;
                cell.current = string.Empty;
                cell.ChangeVisual(string.Empty);
            }
        });

        chapterData = Resources.Load<ChapterDataSO>($"ChapterData/{ChapterType.Forest}");

        widthField = new TextField("스테이지 가로 길이");
        heightField = new TextField("스테이지 세로 길이");
        mapNameField = new TextField("스테이지SO 파일 이름");

        btnSave = new Button(HandleSave);
        btnSave.text = "스테이지 데이터 저장";

        btnCreate = new Button(HandleCreate);
        btnCreate.text = "스테이지 데이터 생성";

        VisualElement ve = new VisualElement();
        ve.style.height = EditorGUIUtility.singleLineHeight;
        rootVisualElement.Add(ve);

        rootVisualElement.Add(chapterDropdwon);
        rootVisualElement.Add(btnSave);
        rootVisualElement.Add(btnCreate);

        rootVisualElement.Add(mapNameField);
        rootVisualElement.Add(widthField);
        rootVisualElement.Add(heightField);

        if (data != null)
        {
            Open();
        }
    }

    [OnOpenAsset]
    public static bool OpenSO(int instanceId, int line)
    {
        if (EditorUtility.InstanceIDToObject(instanceId) as StageDataSO)
        {
            OpenWindow(EditorUtility.InstanceIDToObject(instanceId) as StageDataSO);
            return true;
        }

        return false;
    }

    private void HandleCreate()
    {
        data = ScriptableObject.CreateInstance<StageDataSO>();
        AssetDatabase.CreateAsset(data, $"Assets/Resources/StageData/{mapNameField.value}.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(data);

        foreach (var item in cellVisuals)
        {
            rootVisualElement.Remove(item);
        }

        cellVisuals.Clear();

        int width = int.Parse(widthField.value);
        int height = int.Parse(heightField.value);
        Vector2 size = new Vector2(50, 50);

        mapData = new string[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                Vector2Int index = new Vector2Int(i, j);

                var pos = (new Vector2(width, height) / 2) + index * 60;

                var cell = new CellVisual(pos, size, index, string.Empty, ChangeCell, maxSize, this);
                rootVisualElement.Add(cell);
                cellVisuals.Add(cell);
            }
        }
    }

    private void Open()
    {
        foreach (var item in cellVisuals)
        {
            rootVisualElement.Remove(item);
        }

        cellVisuals.Clear();

        Vector2 size = new Vector2(30, 30);

        mapData = data.GetData();
        int width = mapData.GetLength(0);
        int height = mapData.GetLength(1);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2Int index = new Vector2Int(i, j);

                var pos = (new Vector2(width, height) / 2) + index * 40;

                var cell = new CellVisual(pos, size, index, mapData[i, j], ChangeCell, maxSize, this);
                rootVisualElement.Add(cell);
                cellVisuals.Add(cell);
            }
        }
    }

    private void HandleSave()
    {
        data.SetUp(mapData);
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
    }

    private string ChangeCell(string value, Vector2Int index)
    {
        mapData[index.x, index.y] = value;

        return mapData[index.x, index.y];
    }
}