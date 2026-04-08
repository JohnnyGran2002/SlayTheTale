using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> {}
public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public CustomGameEvent response;

    //when this object is enabled, tune in
    private void OnEnable()
    {
        gameEvent.RegisterListeners(this);
    }

    //when this object is disabled, tune out
    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    //called by GameEvent when a event is broadcasted
    public void OnEventRaised(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}
