using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper
{
    
    public static void Clear(this Transform transform)
    {

        for(int i = 0; i < transform.childCount; ++i)
        {

            Object.Destroy(transform.GetChild(i).gameObject);

        }

    }

}
