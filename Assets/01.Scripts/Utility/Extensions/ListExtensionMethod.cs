using System.Collections.Generic;
using UnityEngine;

public static class ListExtensionMethod
{
    public static void Swap<T>(this List<T> list, int from, int to)
    {
        (list[from], list[to]) = (list[to], list[from]);
    }
    
    public static T PickRandom<T>(this List<T> source)
    {
        int randomIndex = Random.Range(0, source.Count);
        if (randomIndex >= source.Count)
            return default(T);

        return source[randomIndex];
    }
}