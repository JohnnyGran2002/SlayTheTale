using System;
using UnityEngine;


public abstract class StatusData : ScriptableObject
{
    [SerializeField] protected string name;
    [SerializeField] protected int duration;
    [SerializeField] protected TickType tickType;
    [SerializeField] protected bool stackable;
    [SerializeField] protected int stacks;
    
    public enum TickType
    {
        turnStart,
        turnActive,
        turnEnd
    }
    
    public virtual Status Clone(bool wasCastByPlayer)
    {
        return null;
    }
    
    
}
