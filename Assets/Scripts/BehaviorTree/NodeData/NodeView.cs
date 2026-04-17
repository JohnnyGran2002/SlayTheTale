using UnityEngine;
using UnityEditor.Experimental.GraphView;

//visual representation of a node in the editor
public class NodeView : Node
{
    //reference to the data(scriptableObject)
    public NodeData NodeData;

    public NodeView(NodeData nodeData)
    {
        NodeData = nodeData;

        //display name of the node
        title = nodeData.name;

        var input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));

        input.portName = "In";
        inputContainer.Add(input);

        var output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        output.portName = "Out";
        outputContainer.Add(output);

        //refresh UI
        RefreshExpandedState();
        RefreshPorts();
    }
}
