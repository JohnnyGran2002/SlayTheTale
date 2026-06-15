using UnityEngine;
using UnityEngine.Serialization;

public class MapGenerator : MonoBehaviour
{
    public GameObject node;

    public int rows,columns;

    [Tooltip("The space between the nodes")]public float spaceHorizontal, spaceVertical;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rows == 0 || columns == 0)
        {
            Debug.LogWarning("Rows or colums can't be 0");
            return;
        }
        
        for (var i = 0; i < rows; i++)
        {
            //Instantiate(node, new Vector3(0 + (spaceHorizontal * i), 0, 0), Quaternion.identity);
            
            for (var j = 0; j < columns; j++)
            {
                Instantiate(node, new Vector3(0 + (spaceHorizontal * i), 0, 0 + (spaceVertical * j)), Quaternion.identity);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
