using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class TerrainProperty
{
    public TerrainType Type;
    public int Cost;
    public bool Passable;
    public bool BlocksView;
    public Color DebugColor;
}

[CustomPropertyDrawer(typeof(TerrainProperty))]
public class TerrainCostDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 16f+18f*4f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);

        position.height = 16f;
        EditorGUI.indentLevel += 1;
        contentPosition = EditorGUI.IndentedRect(position);
        contentPosition.y += 18f;

        EditorGUI.indentLevel = 0;
        EditorGUIUtility.labelWidth = 60f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Type"), new GUIContent("Type"));
        contentPosition.y += 18f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Cost"), new GUIContent("Cost"));
        contentPosition.width *= 0.5f;
        contentPosition.y += 18f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Passable"), new GUIContent("Passable"));
        contentPosition.x += contentPosition.width;
        EditorGUIUtility.labelWidth = 80f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("BlocksView"), new GUIContent("Blocks View"));
        contentPosition.x -= contentPosition.width;
        contentPosition.width *= 2f;
        contentPosition.y += 18f;
        EditorGUIUtility.labelWidth = 100f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Debug"), new GUIContent("Debug Color"));
        EditorGUI.EndProperty();
    }
}