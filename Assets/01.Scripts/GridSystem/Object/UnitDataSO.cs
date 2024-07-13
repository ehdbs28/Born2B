using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Cell/Unit")]
public class UnitDataSO : CellObjectSO
{
    public string unitName;
    public string unitDesc;
    
    public IHealth health { get; set; }
    public StatusController statusController { get; set; }

    [field:SerializeField] public WeaponItemSO weaponItem { get; private set; }
    [field:SerializeField] public List<int2> movementRole { get; private set; }

    [SerializeField] StatSO stat = null;
    private StatSO statInstance = null;
    public StatSO Stat => statInstance;

    public void Init()
    {
        if(statInstance == null)
            statInstance = Instantiate(stat);
    }
}
