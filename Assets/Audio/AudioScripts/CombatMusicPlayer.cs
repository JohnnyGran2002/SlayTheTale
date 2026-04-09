using System;
using UnityEngine;
using Sonity;
using Sonity.Internal;

public class CombatMusicPlayer : MonoBehaviour
{
    public SoundEvent combatMusic;
    
    public SoundParameterIntensity soundIntensityParameter = new SoundParameterIntensity(1f, UpdateMode.Continuous);

    public float intensityValue = 0f;
    
    void Start()
    {
        soundIntensityParameter.Intensity = 0;
        combatMusic.Play(transform, soundIntensityParameter);
    }
}
