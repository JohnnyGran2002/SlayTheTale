using Sonity;
using UnityEngine;

public class EventSettings : MonoBehaviour
{
    /// <summary>
    /// Whenever we want to change the music, we can add this component and add the relevant settings, or reference this script.
    /// </summary>
    
    //Options for what "UseSettings" should do
    public enum Action
    {
        None,
        Play,
        Stop,
        SetIntensity,
    }
    //Send the relevant event to musicmanager with some parameters
    [System.Serializable]
    public struct EventInfo
    {
        public SoundEvent SoundEvent;
        [Space (7)]
        public Action _action;
        [Space(7)] 
        public bool stopAllOtherMusic;
        [Space(7)]
        public bool allowFadeOut;
        [Space(7)]
        public float intensityValue;

    }
    public EventInfo events;

    //Invoke this function to play or stop the selected music
    public void UseSettings()
    {
        MusicManager.musicManager.InvokeAudioSettings(events);
    }
}
