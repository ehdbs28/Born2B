using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Cell/Player")]
public class PlayerCellObjectSO : CellObjectSO
{

    [SerializeField] private GameObject[] _instances;

    public override GameObject cellObjectInstancePrefab {
        get
        {
            return _instances[UnitSelectManager.Instance.selectedIdx];
        }
        protected set 
        {
        
        }
    }

}
