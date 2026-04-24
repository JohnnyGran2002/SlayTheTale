using System;
using UnityEngine;
using Sonity;
using Sonity.Internal;
using UnityEngine.Serialization;
using System.Collections.Generic;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager musicManager;

    public Dictionary<string, SoundEvent> currentMusic;

    private string _musicString;

    private AudioMixer _combatMixer;
    
    //Global parameter that will change based on which turn it is
    public SoundParameterIntensity soundIntensityParameter;
    
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
        currentMusic = new Dictionary<string, SoundEvent>();
        soundIntensityParameter = new SoundParameterIntensity(1f, UpdateMode.Continuous);
    }
    
    void Start()
    {
        
        
        //Sets the intensity to 1, should be changed if the player should have the first turn
        //soundIntensityParameter.Intensity = 0;
    }

    public void InvokeAudioSettings(EventSettings.EventInfo msg)
    {
        _musicString = msg.SoundEvent.ToString();
        Debug.Log(_musicString);
        if (!currentMusic.ContainsKey(_musicString))
            currentMusic[_musicString] = msg.SoundEvent;
        switch (msg._action)
        {
            //Plays selected music
            case EventSettings.Action.Play:
                currentMusic[_musicString].MusicPlay(msg.stopAllOtherMusic, msg.allowFadeOut);
                break;
            
            //Stops selected music
            case EventSettings.Action.Stop:
                currentMusic[_musicString].MusicStop(msg.allowFadeOut);
                currentMusic.Remove(_musicString);
                break;
            case EventSettings.Action.SetIntensity:
                soundIntensityParameter.Intensity = msg.intensityValue;
                break;
        }
    }

    public void EndCombat()
    {
        
    }
    
    
    //Stops all music (very necessary comment)
    public void StopAllMusic()
    {
        //currentMusic.MusicStop(true);
    }
}
