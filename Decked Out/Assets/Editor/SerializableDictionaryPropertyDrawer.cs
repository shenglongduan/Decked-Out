using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
public class SerializableDictionaryPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var keys = property.FindPropertyRelative("keys");
        var values = property.FindPropertyRelative("values");

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.height = EditorGUIUtility.singleLineHeight;

        if (keys.arraySize != values.arraySize)
        {
            values.arraySize = keys.arraySize;
        }

        int newSize = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height), "Size", keys.arraySize);

        if (newSize != keys.arraySize)
        {
            keys.arraySize = newSize;
            values.arraySize = newSize;
        }

        EditorGUI.indentLevel++;

        for (int i = 0; i < keys.arraySize; i++)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y + (i + 1) * EditorGUIUtility.singleLineHeight, position.width / 2, position.height),
                keys.GetArrayElementAtIndex(i), GUIContent.none);
            EditorGUI.PropertyField(new Rect(position.x + position.width / 2, position.y + (i + 1) * EditorGUIUtility.singleLineHeight, position.width / 2, position.height),
                values.GetArrayElementAtIndex(i), GUIContent.none);
        }

        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (property.FindPropertyRelative("keys").arraySize + 1) * EditorGUIUtility.singleLineHeight;
    }
}
