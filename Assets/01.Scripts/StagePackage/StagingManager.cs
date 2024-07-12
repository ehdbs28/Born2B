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

            Debug.LogWarning("게임이 끝");
            return;

        }

        CellObjectManager.Instance.InitContainer();
        StageManager.Instance.SetUpGrid();
        _currentStage++;

    }

}
