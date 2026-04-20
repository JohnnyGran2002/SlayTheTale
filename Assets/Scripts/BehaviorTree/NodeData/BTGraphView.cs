using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Mono.Cecil.Cil;
using System;


//Main graph container that handles things like zoom, drag, selection, and grid background for the behavior tree editor and other things
public class BTGraphView : GraphView
{
    //callback to editor window
    public Action<NodeData> OnNodeCreated;

    public BTGraphView()
    {
        //enables zooming
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        //enables dragging the graph view around
        this.AddManipulator(new ContentDragger());

        //enables selecting and dragging nodes around
        this.AddManipulator(new SelectionDragger());

        //enables box selection
        this.AddManipulator(new RectangleSelector());

        //adds a grid background to the graph view for better visualization
        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        graphViewChanged += OnGraphViewChanged;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);

        Vector2 mousePosition = evt.mousePosition;

        //add options to menu
        evt.menu.AppendAction("Create/Action/Attack", (a) => CreateNodeAt(mousePosition, typeof(AttackNodeData)));
        evt.menu.AppendAction("Create/Condition/Is Player In Range", (a) => CreateNodeAt(mousePosition, typeof(IsPlayerInRangeData)));
    }

    private void CreateNodeAt(Vector2 position, Type type)
    {
        Debug.Log("CreateNodeAt called");

        //create a scriptableObject instance
        NodeData nodeData = ScriptableObject.CreateInstance(type) as NodeData;

        //save position
        nodeData.Position = position;

        //notify editor window
        OnNodeCreated?.Invoke(nodeData);
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange change)
    {
        //when edges are created
        if (change.edgesToCreate != null)
        {
            foreach (Edge edge in change.edgesToCreate)
            {
                NodeView parent = edge.output.node as NodeView;
                NodeView child = edge.input.node as NodeView;

                if (parent != null && child != null)
                {
                    //add child reference in data
                    parent.NodeData.children.Add(child.NodeData);
                }
            }
        }

        //when edges are removed
        if(change.elementsToRemove != null)
        {
            foreach (var elementToRemove in change.elementsToRemove)
            {
                if (elementToRemove is Edge edge)
                {
                    NodeView parent = edge.output.node as NodeView;
                    NodeView child = edge.input.node as NodeView;

                    if (parent != null && child != null)
                    {
                        parent.NodeData.children.Remove(child.NodeData);
                    }
                }
            }
        }

        return change;
    }

    //creates and adds a visual node to the graph
    public void CreateNode(NodeData nodeData)
    {

        Debug.Log("GraphView.CreateNode called");

        NodeView nodeView = new NodeView(nodeData);

        //set positon based on stored data
        nodeView.SetPosition(new Rect(nodeData.Position, new Vector2(200, 150)));

        AddElement(nodeView);
    }
}
