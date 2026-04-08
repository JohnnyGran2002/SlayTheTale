using System.Collections.Generic;
using UnityEngine;

public class ShapeCreator : MonoBehaviour
{
    public string name;

    public List<Vector2> corners;

    public Vector2 centerPivot;

    public bool draw;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        List<Vector2> _corners = new List<Vector2>();
        if (corners != null)
        {
            foreach (var corner in corners)
            {
                if (!_corners.Contains(corner))
                    _corners.Add(corner);
            }
        }
        
        if (draw)
        {
            for (int i = 0; i < _corners.Count; i++)
            {
                if (i +1 >= _corners.Count)
                    Gizmos.DrawLine(_corners[i], _corners[0]);
                
                else
                    Gizmos.DrawLine(_corners[i], _corners[i + 1]);
            }
        }
    }
}
