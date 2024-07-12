using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInstance : MonoBehaviour
{
    
    private bool _isColorInit;
    public Cell CellData { get; set; }
    private void InitCellData()
    {

        CellData = StageManager.Instance.GetCell(CellData.position).Value;

    }

    private void Start()
    {

        if (_isColorInit) return;
        GetComponent<SpriteRenderer>().color = CellData.color;

    }

    public void TileInit(Sprite tile)
    {

        _isColorInit = true;
        GetComponent<SpriteRenderer>().sprite = tile;

    }

    private void OnMouseDown()
    {

        if (TurnManager.Instance.GetTurnData<bool>(TurnDataType.IsPreview))
        {
            
            InitCellData();

            if (CellObjectManager.Instance.GetCellObjectInstance(CellData.unitKey) is PlayerInstance)
            {

                StageManager.Instance.PickGrid(CellData);

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
