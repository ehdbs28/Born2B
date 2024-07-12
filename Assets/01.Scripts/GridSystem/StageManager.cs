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

    [SerializeField] private GridSettingData _gridSettingData;

    public StageGrid Grid { get; private set; }

    private void Awake()
    {

        Grid = new(_gridSettingData);

    }

    private void Update()
    {

        Grid.Update();

    }
    private void LateUpdate()
    {

        Grid.LateUpdate();

    }



  
    private void OnDestroy()
    {

        Grid.Dispose();

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