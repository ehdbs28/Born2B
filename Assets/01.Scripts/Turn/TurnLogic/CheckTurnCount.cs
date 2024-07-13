using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Turn/CheckTurnCount")]
public class CheckTurnCount : TurnObjectSO
{
    public override IEnumerator TurnLogic()
    {

        yield return null;
        TurnManager.Instance.InitTurn();

    }

}