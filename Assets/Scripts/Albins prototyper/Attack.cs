using UnityEngine;
using UnityEngine.VFX;

[System.Serializable]
[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attack")]
public class Attack : ScriptableObject
{
    public AreaType areaType;
    public int damage;
    public float delay;
    public float lingerTime;
    public float length;
    public float width;
    public float radius;
    public float angle;

    public VisualEffectAsset vfx;
    //add vfx holder 
    
    
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
        effect.vfx = att.vfx;
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
