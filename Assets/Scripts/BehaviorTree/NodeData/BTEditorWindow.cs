using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;

//custom editor window for behavior tree graph view tool
public class BTEditorWindow : EditorWindow
{
    private BTGraphView graphView;
    //add menu item to open the behavior tree editor window
    [MenuItem("Window/Behavior Tree Editor")]
    public static void OpenBTEditorWindow()
    {
        BTEditorWindow window = GetWindow<BTEditorWindow>();
        window.titleContent = new GUIContent("Behavior Tree Editor");
    }

    //called when window in opened or reloaded
    private void OnEnable()
    {
        graphView = new BTGraphView();
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    //cleanup when window is closed or disabled
    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }
}
