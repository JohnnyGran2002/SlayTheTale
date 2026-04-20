using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

//custom editor window for behavior tree graph view tool
public class BTEditorWindow : EditorWindow
{
    private BTGraphView graphView;

    private BehaviorTreeData currentTree;
    private BehaviorTreeData previousTree;
    private ObjectField treeField;

    //add menu item to open the behavior tree editor window
    [MenuItem("Tools/Behavior Tree Editor")]
    public static void OpenBTEditorWindow()
    {
        BTEditorWindow window = GetWindow<BTEditorWindow>();
        window.titleContent = new GUIContent("Behavior Tree Editor");
    }

    //called when window in opened or reloaded
    private void OnEnable()
    {
        // Clear everything
        rootVisualElement.Clear();

        // Create a vertical layout container
        var container = new VisualElement();
        container.style.flexDirection = FlexDirection.Column;
        container.style.flexGrow = 1;

        // ===== TOP BAR =====
        var topBar = new VisualElement();
        topBar.style.flexDirection = FlexDirection.Row;
        topBar.style.height = 30;
        topBar.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);

        treeField = new ObjectField("Tree")
        {
            objectType = typeof(BehaviorTreeData),
            style = { flexGrow = 1 }
        };

        treeField.RegisterValueChangedCallback(evt =>
        {
            currentTree = evt.newValue as BehaviorTreeData;
            LoadTree();
        });

        topBar.Add(treeField);

        // ===== GRAPH =====
        graphView = new BTGraphView();
        graphView.style.flexGrow = 1; // VERY IMPORTANT

        graphView.OnNodeCreated += CreateNode;

        // Add to container
        container.Add(topBar);
        container.Add(graphView);

        // Add to window
        rootVisualElement.Add(container);
    }

    //cleanup when window is closed or disabled
    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    //called when a nodes is created from right-click
    private void CreateNode(NodeData nodeData)
    {
        Debug.Log("CreateNode in EditorWindow called");

        if (currentTree == null)
        {
            Debug.LogWarning("No BehaviorTreeData was selected!");
            {
                return;
            }
        }

        //add a tree asset
        currentTree.Nodes.Add(nodeData);

        //save sub-asset, so unity keeps/saves it
        AssetDatabase.AddObjectToAsset(nodeData, currentTree);
        AssetDatabase.SaveAssets();

        //create visual node
        graphView.CreateNode(nodeData);
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        //field to assign BehaviorTreeData
        currentTree = (BehaviorTreeData)EditorGUILayout.ObjectField( "Tree", currentTree, typeof(BehaviorTreeData), false);

        GUILayout.EndHorizontal();

        if (currentTree != previousTree)
        {
            previousTree = currentTree;
            LoadTree();
        }
    }

    private void LoadTree()
    {
        if (currentTree == null)
        {
            return;
        }

        graphView.DeleteElements(graphView.graphElements);

        if (currentTree == null)
        {
            return;
        }

        foreach (NodeData nodeData in currentTree.Nodes)
        {
            graphView.CreateNode(nodeData);
        }
    }
}
