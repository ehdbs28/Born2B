using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Turn/AttackEmenyTurn")]
public class EnemyAttackTurn : TurnObjectSO
{
    public override IEnumerator TurnLogic()
    {

        CellObjectManager.Instance.AttackCellObject<UnitInstance>(true);
        yield return null;

    }

    public override IEnumerator TurnEndLogic()
    {

        yield return null;

    }

}
