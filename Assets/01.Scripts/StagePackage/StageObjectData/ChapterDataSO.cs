using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Chapter/Data")]
public class ChapterDataSO : ScriptableObject
{
    [field: SerializeField] public CellInstance chapterCellPrefab_T1 { get; private set; }
    [field: SerializeField] public CellInstance chapterCellPrefab_T2 { get; private set; }
    public List<StageObjectSlot> stageObjectSlotList = new();
    public List<StageDataSO> stages;
    private Dictionary<string, (StageObjectType, StageObjectSO)> _nameToObjectDataDic = new();
    public string[] chapterName = new string[2];

    private void OnEnable()
    {
        _nameToObjectDataDic.Clear();

        foreach (var slot in stageObjectSlotList)
        {

            _nameToObjectDataDic.Add(slot.objectName, (slot.objectType, slot.objectData));
        }
    }

    public (StageObjectType, StageObjectSO) GetStageDataByName(string name)
    {
        return _nameToObjectDataDic[name];
    }

    public StageDataSO GetStageByName(string name)
    {

        return stages.Find(x => x.name == name);

    }

    public StageDataSO GetStageByIndex(int idx)
    {

        return stages[idx];

    }

}
