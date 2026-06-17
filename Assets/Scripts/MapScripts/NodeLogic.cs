using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeLogic : MonoBehaviour
{
    public bool assigned;
    public List<GameObject> nextNode = new List<GameObject>();
    private void OnDrawGizmos()
    {
        if (nextNode == null) return;

        foreach (var node in nextNode)
        {
            if (node == null) continue;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, node.transform.position);
            Gizmos.DrawSphere(transform.position, 0.15f);
        }
    }
}
