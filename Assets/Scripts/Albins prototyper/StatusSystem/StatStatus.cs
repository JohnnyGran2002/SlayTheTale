using System;
using UnityEngine;

public class StatStatus : Status
{
    public string targetStat;
    public float multiplicative;
    public float addative;
    public StatStatus(bool inCastByPlayer): base(inCastByPlayer)
    {
        
    }
    
    public override void Tick(StatusData.TickType tick, Statmanager statmanager)
    {
        if (tick != tickType)
        {
            return;
        }
        if (duration >= 0)
        {
            duration--;
        }
        else
        {
            RemoveStatus(statmanager);
        }
    }
    
    public override void Stack(Status other)
    {
        if (stackable)
        {
            other.duration += duration;
        }
        else
        {
            
        }
        
        
    }
    
    public override void ApplyStatus(Statmanager statmanager)
    {
        if (!statmanager.TryModifyStat(targetStat, addative, multiplicative))
        {
            throw new Exception("wrong name");
        }
    }

    public override void RemoveStatus(Statmanager statmanager)
    {
        if (!statmanager.TryModifyStat(targetStat, addative * -1, multiplicative * -1))
        {
            throw new Exception("wrong name");
        }
    }
}
