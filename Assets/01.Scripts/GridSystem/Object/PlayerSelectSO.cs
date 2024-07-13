using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Cell/Player")]
public class PlayerSelectSO : CellObjectSO
{
    public ItemDatabaseSO weaponItemDatabase;
    [field:SerializeField] public List<UnitDataSO> playerDatas;

}
