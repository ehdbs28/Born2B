using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponRange
{
    [Serializable]
    private class WeaponRanges
    {
        public List<WeaponRangeSlot> Ranges = new List<WeaponRangeSlot>();
        public static implicit operator List<WeaponRangeSlot>(WeaponRanges left) => left.Ranges;
    }

    [SerializeField] WeaponRanges[] rangesList = new WeaponRanges[(int)WeaponDirection.END];
    public List<WeaponRangeSlot> GetRanges(int index) => rangesList[index];


#if UNITY_EDITOR
    [SerializeField] int index = 0;
    [SerializeField] int size = 5;
    [SerializeField] int theta = 5;
    #endif
}
