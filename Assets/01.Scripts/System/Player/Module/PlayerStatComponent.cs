using UnityEngine;

public class PlayerStatComponent : PlayerComponent
{
	[SerializeField] StatSO statData = null;
    public StatSO Stat => statData;

    public override void Init(PlayerInstance player)
    {
        base.Init(player);
        statData = Instantiate((player.GetData() as UnitDataSO).stat);
    }

    public Stat GetStat(StatType statType) => statData[statType];
}
