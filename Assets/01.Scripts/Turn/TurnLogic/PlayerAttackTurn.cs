using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Turn/PlayerAttackTurn")]

public class PlayerAttackTurn : TurnObjectSO
{
    public override IEnumerator TurnLogic()
    {
        yield return null;
        InputManager.ChangeInputMap(InputMapType.Attack);
    }

    public override IEnumerator TurnEndLogic()
    {
        yield return null;
        InputManager.ChangeInputMap(InputMapType.Grid);
    }
}
