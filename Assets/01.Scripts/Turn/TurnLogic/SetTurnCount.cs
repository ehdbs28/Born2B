using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Turn/SetTurnCount")]
public class SetTurnCount : TurnObjectSO
{
    public override IEnumerator TurnLogic()
    {

        yield return null;
        var cnt = CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>().GetComponent<PlayerStatComponent>().GetStat(StatType.TurnCount);
        TurnManager.Instance.AddTurnData(TurnDataType.TurnCount, (int)cnt.CurrentValue);

    }

}