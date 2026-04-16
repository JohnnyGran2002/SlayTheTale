using System;
using UnityEngine;
using Sonity;
using Sonity.Internal;
using UnityEngine.Serialization;

public class MusicManager : MonoBehaviour
{
    public static MusicManager musicManager;
    
    //The current soundevent that is playing
    public SoundEvent currentMusic;
    
    //Global parameter that will change based on which turn it is
    public SoundParameterIntensity soundIntensityParameter = new SoundParameterIntensity(1f, UpdateMode.Continuous);
    
    private void Awake()
    {
        if (musicManager != null && musicManager != this)
        {
            Destroy(this);
        }
        else
        {
            musicManager = this;
            DontDestroyOnLoad(this);
        }
    }
    
    void Start()
    {
        //Sets the intensity to 1, should be changed if the player should have the first turn
        soundIntensityParameter.Intensity = 1;
    }

    public void InvokeAudioSettings(EventSettings.EventInfo msg)
    {
        switch (msg._action)
        {
            //Plays selected music
            case EventSettings.Action.play:
                currentMusic = msg.SoundEvent;
                Debug.Log("Playing " + currentMusic);
                //soundIntensityParameter.Intensity = msg.parameterValue;
                currentMusic.MusicPlay(true, msg.allowFadeOut, soundIntensityParameter);
                //currentMusic.Play(transform, soundIntensityParameter);
                break;
            
            //Stops selected music
            case EventSettings.Action.stop:
                currentMusic = msg.SoundEvent;
                currentMusic.MusicStop(msg.allowFadeOut);
                break;
        }
    }

    //Stops all music (very necessary comment)
    public void StopAllMusic()
    {
        currentMusic.MusicStop();
    }
}
