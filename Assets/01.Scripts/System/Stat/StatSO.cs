using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stat")]
public class StatSO : ScriptableObject
{
    [Serializable]
    public class StatSlot
    {
        public StatType StatType;
        public Stat Stat;
    }
    
	public List<StatSlot> stats = new List<StatSlot>();
    private Dictionary<StatType, Stat> statDictionary;
    public Stat this[StatType indexer] {
        get {
            if (statDictionary.ContainsKey(indexer) == false)
            {
                Debug.LogWarning("Stat of Given Type is Doesn't Existed");
                return null;
            }

            return statDictionary[indexer];
        }
    }

    public event Action OnStatChangedEvent = null;

    private void OnEnable()
    {
        statDictionary = new Dictionary<StatType, Stat>();
        stats.ForEach(i => {
            if(statDictionary.ContainsKey(i.StatType))
            {
                Debug.LogWarning("Stat of Current Type is Already Existed");
                return;
            }
            i.Stat.Init();
            statDictionary.Add(i.StatType, i.Stat);
        });
        OnStatChangedEvent?.Invoke();
    }

    public void AddModifier(StatModifierSlot modifierSlot) => 
        AddModifier(modifierSlot.StatType, modifierSlot.ModifierType, modifierSlot.Value);

    public void RemoveModifier(StatModifierSlot modifierSlot) => 
        RemoveModifier(modifierSlot.StatType, modifierSlot.ModifierType, modifierSlot.Value);

    public void AddModifier(StatType statType, StatModifierType modifierType, float value) 
    {
        this[statType]?.AddModifier(modifierType, value);
        OnStatChangedEvent?.Invoke();
    }

    public void RemoveModifier(StatType statType, StatModifierType modifierType, float value) 
    {
        this[statType]?.RemoveModifier(modifierType, value);
        OnStatChangedEvent?.Invoke();
    }
}
