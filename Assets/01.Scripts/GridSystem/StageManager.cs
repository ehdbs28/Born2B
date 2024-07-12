using Singleton;
using StageDefine;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using Object = UnityEngine.Object;

public partial class StageManager : MonoSingleton<StageManager>
{

    private const int GRID_JOB_FPS_COUNT = 2;

    [SerializeField] private GridData _data;
    [SerializeField] private float _cellSize = 0.5f;
    [SerializeField] private Transform _movementPivot;
    [SerializeField] private GameObject _mask;
    [Header("Movement")]
    [SerializeField] private GridInputSO _gridInput;

    private Dictionary<Guid, CellInstance> _instanceContainer = new Dictionary<Guid, CellInstance>();
    private GridMovement _movement;
    private NativeHashMap<Guid, float3> _cellPositions;
    private NativeArray2D<Cell> _grid;
    private NativeList<Cell> _cells;
    private JobHandle _handle;
    private bool _isJobStarted;
    private int _fpsCount;
    [SerializeField] private ChapterDataSO _currentChapterData; //나중에 시얼라이즈 필드 지우기

    private void Update()
    {

        if (_movement != null)
        {

            _movement.Update();

        }

    }
    private void LateUpdate()
    {

        CheckEndJob();

    }

    public void SetChapterData(ChapterDataSO chapterData)
    {
        _currentChapterData = chapterData;
    }
    private void CheckEndJob()
    {

        if (!_isJobStarted) return;
        _fpsCount++;

        if (_fpsCount >= GRID_JOB_FPS_COUNT)
        {

            _handle.Complete();

            bool tileSetValue = false;
            for (int x = 0; x < _data.width; x++)
            {

                for (int y = 0; y < _data.height; y++)
                {

                    var ins = _instanceContainer[_grid[x, y].guid];
                    ins.transform.position = ConvertGridPoint(new Vector2(x, y));

                }

            }

            DestroyNotSelectCell();

            _isJobStarted = false;
            _cellPositions.Dispose();
            _fpsCount = 0;

            TurnManager.Instance.EndCurrentTurn();

        }

    }
    private void DestroyNotSelectCell()
    {

        foreach (var item in _cells)
        {

            if (!_grid.Constain(item))
            {

                Destroy(_instanceContainer[item.guid].gameObject);

                if (item.unitKey != Guid.Empty)
                {

                    CellObjectManager.Instance.RemoveCellObjectInstance(item.unitKey);

                }

                _instanceContainer.Remove(item.guid);

            }

        }

        _cells.Clear();

        for (int x = 0; x < _data.width; x++)
        {

            for (int y = 0; y < _data.height; y++)
            {

                var c = _grid[x, y];
                c.position = new int2(x, y);
                _grid[x, y] = c;
                _instanceContainer[c.guid].CellData = c;
                _cells.Add(_grid[x, y]);

                if (c.unitKey != Guid.Empty)
                {

                    var obj = CellObjectManager.Instance.GetCellObject(c.unitKey);
                    var ins = CellObjectManager.Instance.GetCellObjectInstance(c.unitKey);

                    if (ins != null)
                    {

                        ins.isClone = false;

                    }
                    else
                    {

                        c.unitKey = Guid.Empty;
                        _grid[x, y] = c;

                    }

                    if (obj != null)
                    {

                        obj.position = c.position;

                    }

                }


            }

        }

        //CellObjectManager.Instance.MovementCellObject();

    }
    private void HandleMoveCancel()
    {

        _movement = null;

    }
    private Cell? FindCloestCell(Vector2 pos)
    {

        float min = float.MaxValue;
        Cell? cell = null;

        foreach (var item in _instanceContainer.Values)
        {

            var dest = Vector2.Distance(item.transform.position, pos);

            if (dest < min && dest <= 0.5f)
            {

                min = dest;
                cell = _grid[item.CellData.position];

            }

        }

        return cell;

    }
    private void HandleMovementEnd(LineType moveLine)
    {

        _cellPositions = new NativeHashMap<Guid, float3>(_instanceContainer.Count, Allocator.TempJob);

        foreach (var item in _instanceContainer)
        {

            _cellPositions.Add(item.Key, item.Value.transform.position);

        }

        var job = new CellRearrangementJob
        {

            cells = _cells,
            grid = _grid,
            cellPositons = _cellPositions,
            cellSize = _cellSize,
            data = _data,
            lineType = moveLine

        };

        _handle = job.Schedule();
        _isJobStarted = true;
        _movement.Dispose();
        _movement = null;

    }
    private void SetUpCells()
    {
        bool tileValue = false;

        for (int x = 0; x < _data.width; x++)
        {
            tileValue = !tileValue;
            for (int y = 0; y < _data.height; y++)
            {

                _grid[x, y] = new Cell(new(x, y));
                _cells.Add(_grid[x, y]);
                var ins = CreateCellInstance(_grid[x, y]);
            }
        }

    }
    private CellInstance CreateCellInstance(in Cell cell)
    {

        var pos = ConvertGridPoint(new Vector2(cell.position.x, cell.position.y));
        var ins = Instantiate(
            _currentChapterData.chapterCellPrefab, pos, Quaternion.identity);
        ins.CellData = cell;
        _instanceContainer.Add(cell.guid, ins);

        return ins;

    }
    private int FindCellIndeByCell(Guid key)
    {

        for (int i = 0; i < _cells.Length; i++)
        {

            if (_cells[i].guid == key)
                return i;

        }

        return -1;

    }
    private void SetUpCloneCellObject(Guid targetCell, CellInstance cell)
    {

        var idx = FindCellIndeByCell(targetCell);
        var cellData = _cells[idx];

        if (cellData.unitKey == Guid.Empty) return;

        var cins = CellObjectManager.Instance.CloneCellObjectInstance(cellData.unitKey);

        if (cins.GetData().cellObjectType == CellObjectType.MoveAble)
        {

            cins.transform.SetParent(cell.transform);
            cins.transform.localPosition = Vector3.zero;
            SetCellUnitKey(idx, cins.key);

        }

    }
    private void ConnectedPortal(PortalDataSO so)
    {

        if (so == null) return;

        foreach (var item in so.portalLinkList)
        {

            var idx1 = item.linkPortalIndex_1;
            var idx2 = item.linkPortalIndex_2;
            var obj1 = CellObjectManager.Instance.GetCellObjectInstance(new int2(idx1.x, (_data.width - 1) - idx1.y)) as PortalObject;
            var obj2 = CellObjectManager.Instance.GetCellObjectInstance(new int2(idx2.x, (_data.width - 1) - idx2.y)) as PortalObject;

            obj1.Connect(obj2);
            obj2.Connect(obj1);

        }

    }
    private void SetUpCellObjects(StageDataSO data)
    {

        var array = data.GetData();

        for (int x = 0; x < _data.width; x++)
        {

            for (int y = 0; y < _data.height; y++)
            {


                Debug.Log(array.Length);
                var obj = _currentChapterData.GetStageDataByName(array[x, y]).Item2 as CellObjectSO;
                var ins = _instanceContainer[_grid[x, (_data.width - 1) - y].guid];
                CreateAndAddCellObject(new int2(x, (_data.width - 1) - y), ins, obj);

                //if (array[x, y] != StageCellType.Normal)
                //{

                //    

                //}

            }

        }

    }
    public Cell? GetCell(int2 pos)
    {

        if (!_grid.CheckOutBounce(pos))
        {

            return _grid[pos];

        }

        return null;

    }
    public int FindCellIdxByUnit(Guid unitKey)
    {

        foreach (var cell in _cells)
        {

            if (cell.unitKey == unitKey)
            {

                return FindCellIndeByCell(cell.guid);

            }

        }

        return -1;

    }
    public void SetUpGrid()
    {

        var data = _currentChapterData.GetStageByIndex(0); //나중에 교환
        _data.width = _data.height = data.column;

        InitContainer();

        _mask.transform.localScale = new Vector3(_data.width, _data.height);

        SetUpCells();
        SetUpCellObjects(data);
        ConnectedPortal(data.portalData);

    }
    public NativeArray2D<Cell> GetGrid() => _grid;
    public int2 FindGridIdxByUnit(Guid unitKey)
    {

        for (int x = 0; x < _data.width; x++)
        {

            for (int y = 0; y < _data.height; y++)
            {

                if (_grid[x, y].unitKey == unitKey)
                {

                    return new int2(x, y);

                }

            }

        }

        return new int2(-1, -1);

    }
    public Cell? FindCellByPosition(Vector2 position)
    {

        return FindCloestCell(position);

    }
    public void PickGrid(Cell cell)
    {

        if (_movement != null || _isJobStarted || !TurnManager.Instance.GetTurnData<bool>(TurnDataType.IsPreview)) return;

        _movement = new(cell, _movementPivot, _data, _cellSize, HandleMovementEnd, HandleMoveCancel, _gridInput);

    }
    public Cell[] CopyAndCollocateCellLine(Cell cell, LineType type, int moveCount)
    {

        var origin = GetCellLine(cell, type);
        Cell[] copy = new Cell[origin.Length];

        for (int i = 0; i < copy.Length; i++)
        {

            var o = origin[i];

            copy[i] = (Cell)o.Clone();
            copy[i].position = type == LineType.Row
                ? o.position + new int2(1, 0) * moveCount
                : o.position + new int2(0, 1) * moveCount;

            _cells.Add(copy[i]);

        }

        return copy;

    }
    public void InitContainer()
    {

        StopAllCoroutines();

        _handle.Complete();

        if (_grid.array.IsCreated)
        {

            _grid.Dispose();

        }

        if (_cells.IsCreated)
        {

            _cells.Dispose();

        }

        foreach (var item in _instanceContainer.Values)
        {

            Destroy(item.gameObject);

        }

        _grid = new NativeArray2D<Cell>(_data.width, _data.height, Allocator.Persistent);
        _cells = new NativeList<Cell>(_data.width * _data.height, Allocator.Persistent);
        _instanceContainer.Clear();
        _movement?.Dispose();
        _isJobStarted = false;
        _movement = null;

    }
    public CellInstance[] CopyAndCollocateCellInstances(Cell cell, LineType type, int moveCount)
    {

        var cells = CopyAndCollocateCellLine(cell, type, moveCount);
        CellInstance[] inss = new CellInstance[cells.Length];

        for (int i = 0; i < cells.Length; i++)
        {

            inss[i] = CreateCellInstance(cells[i]);

            SetUpCloneCellObject(cells[i].guid, inss[i]);

        }

        return inss;

    }
    public void SetGridUnitKey(int2 idx, Guid guid)
    {

        var data = _grid[idx];
        data.unitKey = guid;
        _grid[idx] = data;

    }
    public void CreateAndAddCellObject(int2 pos, CellInstance cellIntance, CellObjectSO so)
    {

        var ins = CellObjectManager.Instance.CreateCellObject(pos, so);

        if (ins.GetData().cellObjectType == CellObjectType.MoveAble)
        {

            SetUnitKey(pos, ins.key);
            ins.transform.SetParent(cellIntance.transform);
            ins.transform.localPosition = Vector3.zero;

        }
        else
        {

            ins.transform.position = cellIntance.transform.position;

        }

    }
    public void SetCellUnitKey(int idx, Guid guid)
    {

        var data = _cells[idx];
        data.unitKey = guid;
        _cells[idx] = data;

    }
    public void SetUnitKey(int2 idx, Guid guid)
    {

        var cell = _grid[idx];
        var cidx = FindCellIndeByCell(cell.guid);
        SetGridUnitKey(idx, guid);
        SetCellUnitKey(cidx, guid);

    }
    public Cell[] GetCellLine(Cell cell, LineType type)
    {

        Cell[] cells = new Cell[type == LineType.Row ? _data.width : _data.height];

        for (int i = 0; i < cells.Length; i++)
        {

            cells[i] = type == LineType.Row ? _grid[i, cell.position.y] : _grid[cell.position.x, i];

        }

        return cells;

    }
    public CellInstance[] GetCellInstnaces(Cell cell, LineType type)
    {

        var cells = GetCellLine(cell, type);
        var cellins = new CellInstance[cells.Length];

        for (int i = 0; i < cellins.Length; i++)
        {

            cellins[i] = _instanceContainer[cells[i].guid];

        }

        return cellins;

    }
    public Vector2 ConvertGridPoint(Vector2 gridPoint)
    {

        var x = (gridPoint.x * _cellSize) - _data.width / 2;
        var y = (gridPoint.y * _cellSize) - _data.height / 2;

        return new Vector2(x, y);

    }
    public void ApplyCellParent(int2 targetPos, CellObjectInstance item)
    {

        var ins = _instanceContainer[_grid[targetPos].guid];

        item.transform.SetParent(ins.transform);

    }
    public List<Cell> GetEmptyCells()
    {

        List<Cell> cells = new();

        foreach (var item in _cells)
        {

            if (item.unitKey == Guid.Empty && CellObjectManager.Instance.GetCellObjectInstance(item.position) == null)
            {

                cells.Add(item);

            }

        }

        return cells;

    }
    public CellInstance GetCellInstance(Guid guid)
    {

        return _instanceContainer[guid];

    }
    private void OnDestroy()
    {

        _handle.Complete();

        _grid.Dispose();
        _cells.Dispose();

        if (_cellPositions.IsCreated)
        {

            _cellPositions.Dispose();

        }

    }

    [Serializable]
    public struct GridData
    {

        public int width;
        public int height;

    }

    public enum LineType
    {

        Row,
        Column

    }

    [Serializable]
    public class ObjectInstanceContainer
    {

        public StageCellType type;
        public Object instance;

    }

    [BurstCompatible]
    private struct CellRearrangementJob : IJob
    {

        [ReadOnly] public NativeHashMap<Guid, float3> cellPositons;
        [ReadOnly] public NativeArray<Cell> cells;
        [ReadOnly] public GridData data;
        [ReadOnly] public LineType lineType;
        [ReadOnly] public float cellSize;
        public NativeArray2D<Cell> grid;

        public void Execute()
        {

            for (int x = 0; x < data.width; x++)
            {

                for (int y = 0; y < data.height; y++)
                {

                    Rearrangement(new int2(x, y));

                }

            }

        }

        private void Rearrangement(int2 idx)
        {

            float minDist = float.MaxValue;
            Cell targetCell = cells[0];
            var pos = ConvertPosition(idx);

            foreach (var cell in cells)
            {

                var cellPos = cellPositons[cell.guid];

                if (!IsSameLine(cellPos, pos)) continue;

                var dist = math.distance(pos, cellPos);
                if (dist < minDist)
                {

                    minDist = dist;
                    targetCell = cell;

                }

            }

            targetCell.position = idx;
            grid[idx] = targetCell;

        }

        private bool IsSameLine(in float3 origin, in float3 target)
        {

            return lineType == LineType.Row ? origin.y == target.y : origin.x == target.x;

        }

        private float3 ConvertPosition(in int2 idx)
        {

            var x = (idx.x * cellSize) - data.width / 2;
            var y = (idx.y * cellSize) - data.height / 2;

            return new float3(x, y, 0);

        }

    }

}