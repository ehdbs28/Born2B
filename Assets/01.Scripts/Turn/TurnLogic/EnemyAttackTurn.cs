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

        Debug.Log("턴 로직이 끝나다");
        yield return null;

    }

}
