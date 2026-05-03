using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Modifiable<>))]
public class Stats_CustomEditor: Editor
{
    //private bool showPosition = true;

    void OnEnable()
    {
    
    }
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Stats stats = (Stats)target;
        if (GUILayout.Button("add float"))
        {
            stats.floatList.Add(new Modifiable<float>());
        }

        if (GUILayout.Button("add int"))
        {
            stats.intList.Add(new Modifiable<int>());
        }
        
    }
    
}
