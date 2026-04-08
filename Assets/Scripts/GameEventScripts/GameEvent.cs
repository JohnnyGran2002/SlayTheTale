using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    //list of object listening(GameEventListener)
    public List<GameEventListener> listeners = new();

    //raise evnets trough different methods signatures
    public void Raise(Component sender, object data)
    {
        //loop over all listeners and call the OnEventRaised method in GameEventListener
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised(sender, data);
        }
    }

    //manage listeners
    public void RegisterListeners(GameEventListener listener)
    {
        //if listener is not known, then add it to avoid duplicates
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        //check if listener is known before removing listener
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
