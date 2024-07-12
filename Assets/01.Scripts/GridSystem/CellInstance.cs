using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInstance : MonoBehaviour
{
    
    public Cell CellData { get; set; }
    public void Init()
    {



    }
    private void InitCellData()
    {

        CellData = StageManager.Instance.Grid.GetCell(CellData.position).Value;

    }

    private void OnMouseDown()
    {

        if (TurnManager.Instance.GetTurnData<bool>(TurnDataType.IsPreview))
        {
            
            InitCellData();

            if (CellObjectManager.Instance.GetCellObjectInstance(CellData.unitKey) is PlayerInstance)
            {

                StageManager.Instance.Grid.PickGrid(CellData);

            }

        }

    }

    public void Grow()
    {

        Debug.DrawLine(transform.position - new Vector3(-0.45f, 0.45f), transform.position - new Vector3(0.45f, 0.45f), Color.green, 1.25f);
        Debug.DrawLine(transform.position - new Vector3(0.45f, 0.45f), transform.position - new Vector3(0.45f, -0.45f), Color.green, 1.25f);
        Debug.DrawLine(transform.position - new Vector3(0.45f, -0.45f), transform.position - new Vector3(-0.45f, -0.45f), Color.green, 1.25f);
        Debug.DrawLine(transform.position - new Vector3(-0.45f, -0.45f), transform.position - new Vector3(-0.45f, 0.45f), Color.green, 1.25f);

    }

}
