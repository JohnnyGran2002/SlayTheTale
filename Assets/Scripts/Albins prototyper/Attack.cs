using UnityEngine;
[System.Serializable]

public class Attack : MonoBehaviour
{
    public AreaType areaType;
    public int damage;
    public float delay;
    
    
    public enum AreaType{
        Square,
        Circle,
        Cone
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
