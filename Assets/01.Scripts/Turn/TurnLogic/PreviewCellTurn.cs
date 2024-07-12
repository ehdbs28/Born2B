using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Turn/PreviewTurn")]
public class PreviewCellTurn : TurnObjectSO
{

    public override IEnumerator TurnLogic()
    {

        TurnManager.Instance.AddTurnData(TurnDataType.IsPreview, true);
        yield return null;

    }

}