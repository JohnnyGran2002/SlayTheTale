using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Modifiable<>))]
public class Modifiable_PropertyDrawer: PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the name field inside the element
        var nameProp = property.FindPropertyRelative("name");

        // Override label using the name value
        if (nameProp != null && !string.IsNullOrEmpty(nameProp.stringValue))
        {
            label = new GUIContent(nameProp.stringValue);
        }

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}






public class MyElementDrawer : PropertyDrawer
{
    
}