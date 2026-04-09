using System;
using UnityEngine;
using Sonity;
using Sonity.Internal;

public class CombatMusicPlayer : MonoBehaviour
{
    public SoundEvent combatMusic;
    
    private SoundParameterIntensity soundIntensityParameter = new SoundParameterIntensity(1f, UpdateMode.Once);

    [SerializeField] private float _intensityValue = 0f;
    
    void Start()
    {
        soundIntensityParameter.Intensity = 0;
        combatMusic.Play(transform, soundIntensityParameter);
    }
}
