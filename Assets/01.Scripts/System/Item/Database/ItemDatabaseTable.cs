using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabaseTable
{
    public List<ItemDatabaseSlot> table = new List<ItemDatabaseSlot>();
    private float[] itemWeights;

    public void Init()
    {
        itemWeights = table.Select(item => item.weight).ToArray();
    }

    public ItemSO PickRandom()
    {
        int index = GetIndexByWeight();
        return table[index].itemData.PickRandom();
    }

    private int GetIndexByWeight()
    {
        float sum = 0f;

        for(int i = 0; i < itemWeights.Length; i++)
            sum += itemWeights[i];

        float randomValue = Random.Range(0f, sum);
        float tempSum = 0;

        for(int i = 0; i < itemWeights.Length; i++)
            if(randomValue >= tempSum && randomValue < tempSum + itemWeights[i])
                return i;
            else
                tempSum += itemWeights[i];

        return 0;
    }
}