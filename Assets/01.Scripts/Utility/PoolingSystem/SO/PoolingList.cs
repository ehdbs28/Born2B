using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolableItem
{
    public PoolableMono prefab;
    public int cnt;
}

[CreateAssetMenu(menuName = "SO/Data/PoolingList")]
public class PoolingList : ScriptableObject
{
    public List<PoolableItem> poolableItems = new List<PoolableItem>();
}