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
    public int textureIdx;
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

        ChangeVisual(textureIdx);
    }

    private void HandleClick(PointerDownEvent evt)
    {
        if (evt.button == 0)
        {
            Debug.Log($"{stageEditor}");
            Debug.Log($"{stageEditor.chapterData}");
            Debug.Log($"{stageEditor.chapterData.stageObjectSlotList}");
            if (textureIdx < maxIdx)
            {
                textureIdx++;
                current = stageEditor.chapterData.stageObjectSlotList[textureIdx].objectName;
            }
            else
            {
                textureIdx = 0;
            }
        }

        if(evt.button == 1)
        {
            if(textureIdx > 0)
            {
                textureIdx--;
            }
            else
            {
                textureIdx = maxIdx;
            }
        }

        if(evt.button == 2 && textureIdx == 3)
        {
            if(stageEditor.onPortalEdit)
            {
                stageEditor.EndPortalEdit(this);
            }
            else
            {
                stageEditor.StartPortalEdit(this);
            }

            return;
        }

        cellChangeCallBack(current, index);
        ChangeVisual(current);
    }

    public void ChangeVisual(string value)
    {
        var old = style.backgroundImage;
        old.value = 
        Background.FromTexture2D(Resources.Load<Texture2D>($"StageEditorTexture/{value}"));
        style.backgroundImage = old;
    }
}

public class StageEditor : EditorWindow
{
    private DropdownField chapterDropdwon;

    private Toggle randomToggle;
    private Button randomButton;
    private TextField normalGridPercent;
    private TextField blockGridPercent;
    private TextField spikeGridPercent;
    private TextField portalGridPercent;

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
        List<string> chapterNameList = new List<string>();

        foreach(var type in Enum.GetValues(typeof(ChapterType)))
        {
            chapterNameList.Add(type.ToString());
        }

        chapterDropdwon = new DropdownField("챕터 선택", chapterNameList, 0);
        chapterDropdwon.RegisterValueChangedCallback(HandleChapterTypeChanged);

        chapterData = Resources.Load<ChapterDataSO>($"ChapterData/{ChapterType.Forest}");
        Debug.Log($"CahapterData/{ChapterType.Forest}");
        Debug.Log(chapterData);

        normalGridPercent = new TextField("그리드 생성 확률");
        blockGridPercent = new TextField("블록 생성 확률");
        spikeGridPercent = new TextField("가시 생성 확률");
        portalGridPercent = new TextField("포털 생성 확률");

        widthField = new TextField("스테이지 가로 길이");
        heightField = new TextField("스테이지 세로 길이");
        mapNameField = new TextField("스테이지SO 파일 이름");

        btnSave = new Button(HandleSave);
        btnSave.text = "스테이지 데이터 저장";

        btnCreate = new Button(HandleCreate);
        btnCreate.text = "스테이지 데이터 생성";

        rootVisualElement.Add(chapterDropdwon);
        rootVisualElement.Add(btnSave);
        rootVisualElement.Add(btnCreate);

        rootVisualElement.Add(mapNameField);
        rootVisualElement.Add(widthField);
        rootVisualElement.Add(heightField);
        rootVisualElement.Add(randomToggle);

        if (data != null)
        {
            Open();
        }
    }

    private void HandleChapterTypeChanged(ChangeEvent<string> evt)
    {
        chapterData = Resources.Load<ChapterDataSO>($"CahapterData/{evt.newValue}");

        foreach (var item in cellVisuals)
        {
            item.current = evt.newValue;
            item.textureIdx = 0;
            item.ChangeVisual(0);
        }
    }

    private void HandleUseRandomMap(ChangeEvent<bool> value)
    {
        if(value.newValue)
        {
            rootVisualElement.Add(normalGridPercent);
            rootVisualElement.Add(blockGridPercent);
            rootVisualElement.Add(spikeGridPercent);
            rootVisualElement.Add(portalGridPercent);
            rootVisualElement.Add(randomButton);
        }
        else
        {
            rootVisualElement.Remove(normalGridPercent);
            rootVisualElement.Remove(blockGridPercent);
            rootVisualElement.Remove(spikeGridPercent);
            rootVisualElement.Remove(portalGridPercent);
            rootVisualElement.Remove(randomButton);
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
