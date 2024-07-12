using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Turn/CheckAllEnemyDie")]
public class CheckAllEnemyDie : TurnObjectSO
{
    public override IEnumerator TurnLogic()
    {

        var objs = CellObjectManager.Instance.GetCellObjectInstances<UnitInstance>();
        yield return null;

        if(objs.Count == 0)
        {

            if(FlowManager.Instance.CurrentCycle == EventType.OnBattleStart)
            {

                FlowManager.Instance.NextCycle();

            }

        }

        TurnManager.Instance.EndCurrentTurn();

    }

}
