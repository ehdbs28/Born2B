using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Turn/MovementEmenyTurn")]
public class MovementEnemyTurn : TurnObjectSO
{
    public override IEnumerator TurnLogic()
    {

        CellObjectManager.Instance.MovementCellObject();
        yield return null;

    }

}