using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;


//Main graph container that handles things like zoom, drag, selection, and grid background for the behavior tree editor and other things
public class BTGraphView : GraphView
{
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

        //GraphViewChange += OnGraphViewChange;
    }

    /*private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        //when edges are created
        if (graphViewChange.edgesToCreate != null)
        {
            foreach (var edge in graphViewChange)
        }
    }*/

    //creates and adds a visual node to the graph
    public void CreateNode(NodeData nodeData)
    {
        NodeView nodeView = new NodeView(nodeData);

        //set positon based on stored data
        nodeView.SetPosition(new Rect(nodeData.Position, new Vector2(200, 150)));

        AddElement(nodeView);
    }
}
