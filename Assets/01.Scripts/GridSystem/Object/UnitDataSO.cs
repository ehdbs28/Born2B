using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnitDataSO : CellObjectSO
{

    [field:SerializeField] public WeaponItemSO weaponItem { get; set; }
    [field:SerializeField] public List<int2> movementRole { get; set; }
    [field:SerializeField] public StatSO stat { get; set; }

}
