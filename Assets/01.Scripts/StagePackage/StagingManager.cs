using Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingManager : MonoSingleton<StagingManager>
{

    [SerializeField] private List<StageDataSO> _stages;
    private int _currentStage;

    public void LoadStage()
    {

        if(_currentStage == _stages.Count)
        {

            Debug.LogWarning("������ ��");
            return;

        }

        CellObjectManager.Instance.InitContainer();
        StageManager.Instance.Grid.SetUpGrid(0);
        _currentStage++;

    }

}
