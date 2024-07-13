using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class ConditionalFieldAttribute : PropertyAttribute
{
    public string Condition;
    public bool Option;

    public ConditionalFieldAttribute(string conditionName, bool option)
    {
        Condition = conditionName;
        Option = option;
    }
}