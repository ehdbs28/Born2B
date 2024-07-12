using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Turn/MovementTurn")]
public class MovementCellTurn : TurnObjectSO
{
    public override IEnumerator TurnLogic()
    {

        TurnManager.Instance.AddTurnData(TurnDataType.IsMovementCell, true);
        yield return null;

    }

}
