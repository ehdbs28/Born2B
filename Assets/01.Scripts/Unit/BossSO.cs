using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Cell/Boss")]

public class BossSO : UnitDataSO
{

    [System.Serializable]
    public class BossWeapon
    {

        public string objectName;
        public WeaponItemSO item;

    }

    [field:SerializeField] public List<BossWeapon> weapons { get; private set; }

}
