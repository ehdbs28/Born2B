using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Turn/PlayerAttackTurn")]

public class PlayerAttackTurn : TurnObjectSO
{
    public override IEnumerator TurnLogic()
    {

        if(FlowManager.Instance.CurrentCycle == EventType.OnBattleFinish)
        {

            TurnManager.Instance.EndCurrentTurn();
            yield break;

        }

        yield return null;
        InputManager.ChangeInputMap(InputMapType.Attack);
    }

    public override IEnumerator TurnEndLogic()
    {
        yield return null;
        InputManager.ChangeInputMap(InputMapType.Grid);
    }
}
