using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebugFlowHandler : MonoBehaviour
{

    [SerializeField] private CellObjectSO _nextPortalIns;

    private void Awake()
    {

        EventManager.Instance.RegisterEvent(EventType.OnGameStart, GameStartHandler);
        EventManager.Instance.RegisterEvent(EventType.OnSelectPlayerUnit, SelectUnitHandler);
        EventManager.Instance.RegisterEvent(EventType.OnStageJoin, StageJoinHandler);
        EventManager.Instance.RegisterEvent(EventType.OnBattleStart, BattleStartHandler);
        EventManager.Instance.RegisterEvent(EventType.OnBattleFinish, BattleFinishHandler);

    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            FlowManager.Instance.InitCycle();

        }

    }

    private void GameStartHandler(object[] args)
    {

        Debug.Log("게임 시작");
        FlowManager.Instance.NextCycle();

    }

    private void SelectUnitHandler(object[] args)
    {

        Debug.Log("유닛 선택");
        UnitSelectManager.Instance.StartSelect();

    }

    private void StageJoinHandler(object[] args)
    {

        Debug.Log("스테이지 입장");
        StartCoroutine(Co());

    }

    private IEnumerator Co()
    {

        yield return null;

        TurnManager.Instance.SetTurnData(TurnDataType.IsMovementCell, false);
        TurnManager.Instance.InitTurn();
        TurnManager.Instance.StartTurn();
        StagingManager.Instance.LoadStage();
        CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>().GetComponent<PlayerWeaponComponent>().Equip();
        FlowManager.Instance.NextCycle();

    }

    private void BattleStartHandler(object[] args)
    {

        Debug.Log("배틀 시작");

    }

    private void BattleFinishHandler(object[] args)
    {

        Debug.Log("배틀 끝");
        var cells = StageManager.Instance.Grid.GetEmptyCells();
        var target = cells[Random.Range(0, cells.Count)];
        var ins = StageManager.Instance.Grid.GetCellInstance(target.guid);

        StageManager.Instance.Grid.CreateAndAddCellObject(target.position, ins, _nextPortalIns);

    }

}