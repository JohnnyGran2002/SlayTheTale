using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    public Attack.AreaType areaType;
    public int damage;
    public float delay;
    public float lingerTime;
    public float length;
    public float width;
    public float angle;
}
