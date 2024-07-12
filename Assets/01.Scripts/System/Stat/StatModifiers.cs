using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class StatModifiers
{
    private Dictionary<StatModifierType, List<float>> modifiers = new Dictionary<StatModifierType, List<float>>();
    public List<float> this[StatModifierType indexer] => modifiers[indexer];
	
    [Tooltip("가수")]
    [SerializeField] List<float> Addends = new List<float>();

    [Tooltip("합연산 승수")]
    [SerializeField] List<float> SumMultipliers = new List<float>();
    
    [Tooltip("곱연산 승수")]
    [SerializeField] List<float> MultiplicationMultipliers = new List<float>();

    public StatModifiers()
    {
        Type classType = typeof(StatModifiers);
        foreach(StatModifierType modifierType in Enum.GetValues(typeof(StatModifierType)))
        {
            FieldInfo field = classType.GetField($"{modifierType}s");
            if(field == null)
                return;
            modifiers.Add(modifierType, field.GetValue(this) as List<float>);
        }
    }

    public void Clear()
    {
        Addends.Clear();
        SumMultipliers.Clear();
        MultiplicationMultipliers.Clear();
    }

    public void CalculateValue(ref float inValue)
    {
        CaculateAddends(ref inValue);
        CalculateSumMultipliers(ref inValue);
        CalculateMultiplicationMultipliers(ref inValue);
    }

    private void CaculateAddends(ref float inValue)
    {
        for(int i = 0; i < Addends.Count; ++i)
            inValue += Addends[i];
    }

    private void CalculateSumMultipliers(ref float inValue)
    {
        float multiplier = 0f;
        for(int i = 0; i < SumMultipliers.Count; ++i)
            multiplier += SumMultipliers[i];
        inValue *= multiplier;
    }

    private void CalculateMultiplicationMultipliers(ref float inValue)
    {
        float multiplier = 1f;
        for(int i = 0; i < MultiplicationMultipliers.Count; ++i)
            multiplier *= MultiplicationMultipliers[i];
        inValue *= multiplier;
    }
}
