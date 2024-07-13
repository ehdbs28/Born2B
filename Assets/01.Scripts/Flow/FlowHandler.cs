using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlowHandler : MonoBehaviour
{

    [SerializeField] private CellObjectSO _nextPortalIns;

    public void Init()
    {

        EventManager.Instance.RegisterEvent(EventType.OnGameStart, GameStartHandler);
        EventManager.Instance.RegisterEvent(EventType.OnSelectPlayerUnit, SelectUnitHandler);
        EventManager.Instance.RegisterEvent(EventType.OnStageJoin, StageJoinHandler);
        EventManager.Instance.RegisterEvent(EventType.OnBattleStart, BattleStartHandler);
        EventManager.Instance.RegisterEvent(EventType.OnBattleFinish, BattleFinishHandler);

    }

    private void GameStartHandler(object[] args)
    {

        StageManager.Instance.ResetManager();
        CellObjectManager.Instance.TrueInit();
        FlowManager.Instance.NextCycle();

    }

    private void SelectUnitHandler(object[] args)
    {

        UnitSelectManager.Instance.StartSelect();

    }

    private void StageJoinHandler(object[] args)
    {

        StartCoroutine(Co());

    }

    private IEnumerator Co()
    {

        yield return null;

        TurnManager.Instance.SetTurnData(TurnDataType.IsMovementCell, false);
        TurnManager.Instance.InitTurn();
        TurnManager.Instance.StartTurn();
        StageManager.Instance.NextStage();
        //CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>().GetComponent<PlayerWeaponComponent>().Equip();
        EventManager.Instance.PublishEvent(EventType.OnStageLoaded);
        FlowManager.Instance.NextCycle();

    }

    private void BattleStartHandler(object[] args)
    {


    }

    private void BattleFinishHandler(object[] args)
    {

        var cells = StageManager.Instance.Grid.GetEmptyCells();
        var target = cells[Random.Range(0, cells.Count)];
        var ins = StageManager.Instance.Grid.GetCellInstance(target.guid);

        StageManager.Instance.Grid.CreateAndAddCellObject(target.position, ins, _nextPortalIns);

    }

}