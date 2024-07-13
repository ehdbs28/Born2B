using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditional.Condition);

        if (conditionProperty != null && conditionProperty.boolValue == conditional.Option)
        {
            Rect newPosition = new Rect(position);
            newPosition.x += 10;
            newPosition.width -= 10;
            EditorGUI.PropertyField(newPosition, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditional.Condition);

        if (conditionProperty != null && conditionProperty.boolValue == conditional.Option)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}