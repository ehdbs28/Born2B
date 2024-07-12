using UnityEngine;

public class InputSO : ScriptableObject
{
    public InputMapType inputMapType;

    protected virtual void OnEnable()
    {
        Debug.Log($"Set InputSO : {inputMapType}");
    }
}