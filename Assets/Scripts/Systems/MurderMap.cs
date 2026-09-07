using UnityEngine;

public class MurderMap : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (MapGenerator.instance.isActiveAndEnabled)
        {
            Destroy(MapGenerator.instance.gameObject);
        }
    }
}
