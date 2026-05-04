using System;
using Sonity;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public enum Action
    {
        Start,
    }
    
    [System.Serializable]
    public struct Setting
    {
        public Action Action;
        [Space(7)] 
        public SoundEvent SoundEvent;
    }

    public Setting Settings;
    

    private void Start()
    {
        switch (Settings.Action)
        {
            case Action.Start:
                Settings.SoundEvent.Play(transform);
                break;
        }
    }
}
