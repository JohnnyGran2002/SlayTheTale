using UnityEngine;

public abstract class Status
{
    public string name;
    public int duration;
    public StatusData.TickType tickType;
    public bool castByPlayer;
    public bool stackable;
    public int stacks;

    public Status(bool inCastByPlayer)
    {
        castByPlayer = inCastByPlayer;
    }
    public virtual void Tick(StatusData.TickType tick, Statmanager statmanager)
    {
        
    }

    public virtual void Stack(Status other)
    {
        
    }
    
    public virtual void ApplyStatus(Statmanager statmanager)
    {
        
    }

    public virtual void RemoveStatus(Statmanager statmanager)
    {
        
    }
}
