using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WeaponRange))]
public class WeaponRangeDrawer : PropertyDrawer
{
    private float buttonTotalSize = 320f;
    private int size = 0;
    private int indexErr = 0;
    private int theta = 0;

    private const int FIELD_COUNT = 4;
    private const float FIELD_WIDTH = 50;
    private const float FIELD_WIDTH_OFFSET = 5;
    private const float SIDE_OFFSET = 15;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float singleLineHeight = EditorGUIUtility.singleLineHeight;

        Rect foldoutRect = new Rect(position.x, position.y, SIDE_OFFSET, singleLineHeight);
        Rect nameRect = new Rect(position.x + SIDE_OFFSET, position.y, position.width - ((FIELD_WIDTH + FIELD_WIDTH_OFFSET) * FIELD_COUNT + SIDE_OFFSET), singleLineHeight);
        Rect sizeFieldRect = new Rect(position.x + position.width - FIELD_WIDTH, position.y, FIELD_WIDTH, singleLineHeight);
        Rect thetaFieldRect = new Rect(sizeFieldRect.x - (FIELD_WIDTH + FIELD_WIDTH_OFFSET), position.y, FIELD_WIDTH, singleLineHeight);
        Rect utilButtonRect = new Rect(thetaFieldRect.x - (FIELD_WIDTH + FIELD_WIDTH_OFFSET), position.y, FIELD_WIDTH, singleLineHeight);

        SerializedProperty sizeProperty = property.FindPropertyRelative("size");
        SerializedProperty thetaProperty = property.FindPropertyRelative("theta");
        SerializedProperty indexProperty = property.FindPropertyRelative("index");
        SerializedProperty rangesListProperty = property.FindPropertyRelative("rangesList");
        SerializedProperty rangesProperty = rangesListProperty.GetArrayElementAtIndex(indexProperty.intValue).FindPropertyRelative("Ranges");

        if (theta != thetaProperty.intValue)
            theta = thetaProperty.intValue;

        if (size != sizeProperty.intValue)
        {
            if (sizeProperty.intValue % 2 == 0)
                sizeProperty.intValue = size;
            else
            {
                size = sizeProperty.intValue;
                indexErr = size / 2;
            }
        }

        Event currentEvent = Event.current;
        bool clicked = currentEvent.type == UnityEngine.EventType.MouseDown;
        clicked &= nameRect.Contains(currentEvent.mousePosition);
        if (clicked)
        {
            property.isExpanded = !property.isExpanded;
            currentEvent.Use();
        }

        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, GUIContent.none);
        if(property.isExpanded)
        {
            // Preset
            Rect leftButtonRect = new Rect(position.x + SIDE_OFFSET, position.y + buttonTotalSize + singleLineHeight * 2, FIELD_WIDTH, singleLineHeight);
            if(GUI.Button(leftButtonRect, "<"))
            {
                indexProperty.intValue = ((int)WeaponDirection.END + indexProperty.intValue - 1) % (int)WeaponDirection.END;
                return;
            }
            
            Rect indexFieldRect = new Rect(leftButtonRect.xMax + FIELD_WIDTH_OFFSET, position.y + buttonTotalSize + singleLineHeight * 2, position.width - SIDE_OFFSET * 2 - (FIELD_WIDTH_OFFSET + FIELD_WIDTH) * 2, singleLineHeight);
            GUI.Button(indexFieldRect, ((WeaponDirection)indexProperty.intValue).ToString());
            
            Rect rightButtonRect = new Rect(indexFieldRect.xMax + FIELD_WIDTH_OFFSET, position.y + buttonTotalSize + singleLineHeight * 2, FIELD_WIDTH, singleLineHeight);
            if(GUI.Button(rightButtonRect, ">"))
            {
                indexProperty.intValue = ((int)WeaponDirection.END + indexProperty.intValue + 1) % (int)WeaponDirection.END;
                return;
            }

            // Reset
            Rect resetRect = new Rect(position.x + SIDE_OFFSET, position.y + buttonTotalSize + singleLineHeight * 4, position.width - SIDE_OFFSET * 2, singleLineHeight);
            if (GUI.Button(resetRect, "RESET"))
            {
                rangesProperty.ClearArray();
                return;
            }

            resetRect.y += singleLineHeight * 1.25f;
            if (GUI.Button(resetRect, "RESET ALL"))
            {
                for(int i = 0; i < rangesListProperty.arraySize; ++i)
                    rangesListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("Ranges").ClearArray();
                return;
            }

            // Load Data
            List<float> rangeValues = new List<float>(new float[size * size]);
            for (int i = 0; i < rangesProperty.arraySize; ++i)
            {
                SerializedProperty rangeSlotProperty = rangesProperty.GetArrayElementAtIndex(i);
                Vector2Int point = rangeSlotProperty.FindPropertyRelative("Position").vector2IntValue;
                float theta = rangeSlotProperty.FindPropertyRelative("Theta").floatValue;
                
                int yValue = point.y + indexErr;
                yValue = size - 1 - yValue;
                yValue *= size;
                
                int xValue = point.x + indexErr;

                int index = yValue + xValue;

                if (index < 0 || index >= rangeValues.Count) //10 => 11 O / 10 X
                    continue;
                rangeValues[index] = theta;
            }

            // Copy & Paste
            if (GUI.Button(utilButtonRect, "PASTE"))
            {
                List<float> pasted = JsonConvert.DeserializeObject<List<float>>(GUIUtility.systemCopyBuffer);
                if(pasted != null && pasted.Count == rangeValues.Count)
                    rangeValues = pasted;
            }

            utilButtonRect.x -= FIELD_WIDTH + FIELD_WIDTH_OFFSET;
            if (GUI.Button(utilButtonRect, "COPY"))
            {
                string data = JsonConvert.SerializeObject(rangeValues);
                GUIUtility.systemCopyBuffer = data;
            }

            // Grid
            float buttonSize = buttonTotalSize / size;
            float startX = (position.width - buttonTotalSize) / 2;
            float startY = position.y + singleLineHeight * 1.5f;

            Rect buttonRect = new Rect(startX, startY, buttonSize, buttonSize);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int index = y * size + x;
                    if (GUI.Button(buttonRect, rangeValues[index].ToString()))
                        rangeValues[index] = theta;

                    buttonRect.x += buttonSize;
                }
                buttonRect.x = startX;
                buttonRect.y += buttonSize;
            }

            rangesProperty.ClearArray();
            for (int i = 0; i < rangeValues.Count; ++i)
            {
                float theta = rangeValues[i];
                if (theta == 0)
                    continue;

                int newIndex = rangesProperty.arraySize;
                rangesProperty.InsertArrayElementAtIndex(newIndex);
                SerializedProperty rangeSlotProperty = rangesProperty.GetArrayElementAtIndex(newIndex);
                int y = i / size;
                y = size - 1 - y;
                y -= indexErr;
                rangeSlotProperty.FindPropertyRelative("Position").vector2IntValue = new Vector2Int((i % size) - indexErr, y);
                rangeSlotProperty.FindPropertyRelative("Theta").floatValue = theta;
            }
        }

        EditorGUI.LabelField(nameRect, property.displayName); 
        EditorGUI.PropertyField(sizeFieldRect, sizeProperty, GUIContent.none);
        EditorGUI.PropertyField(thetaFieldRect, thetaProperty, GUIContent.none);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;
        if(property.isExpanded)
        {
            height += buttonTotalSize;
            height += EditorGUIUtility.singleLineHeight * 6f;
        }

        return height;
    }
}
