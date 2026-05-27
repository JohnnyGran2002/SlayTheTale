using System;
using UnityEngine;


[CreateAssetMenu(fileName = "StatStatusData", menuName = "Scriptable Objects/StatusData/StatData")]
public class StatStatusData: StatusData 
{
    [SerializeField]private string targetStat;
    [SerializeField]private float multiplicative;
    [SerializeField]private float Addative;
    
    
    public override Status Clone(bool wasCastByPlayer)
    {
        StatStatus output = new StatStatus(wasCastByPlayer);
        output.name = name;
        output.duration = duration;
        output.tickType = tickType;
        output.targetStat = targetStat;
        output.multiplicative = multiplicative;
        output.addative = Addative;
        output.stackable = stackable;
        output.stacks = stacks;
        
        return output;
    }
}
