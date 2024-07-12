using Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectManager : MonoSingleton<UnitSelectManager>
{

    private bool _isSelectable;
    public int selectedIdx { get; private set; }

    public void StartSelect()
    {

        _isSelectable = true;

    }

    public void Select(int idx)
    {

        if (!_isSelectable) return;

        selectedIdx = idx;
        FlowManager.Instance.NextCycle();

    }

    //Debug
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {

            Select(0);
        
        }

    }

}
