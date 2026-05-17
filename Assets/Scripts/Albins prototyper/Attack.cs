using UnityEngine;
[System.Serializable]

public class Attack : MonoBehaviour
{
    public AreaType areaType;
    public int damage;
    public float delay;
    public float lingerTime;
    public float length;
    public float width;
    public float radius;
    public float angle;
    
    
    public static void InsertValues(Attack att, SpellEffect effect)
    {
        effect.damage = att.damage;
        effect.delay = att.delay;
        effect.LingerTime = att.lingerTime;
        effect.areaType = att.areaType;
        effect.length = att.length;
        effect.width = att.width;
        effect.radius = att.radius;
        effect.angle = att.angle;
    }

    public static void InsertValues(AttackData data, Attack att)
    {
        att.damage = data.damage;
        att.delay = data.delay;
        att.lingerTime = data.lingerTime;
        att.areaType = data.areaType;
        att.length = data.length;
        att.width = data.width;
        att.radius = data.radius;
        att.angle = data.angle;
    }
    
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
