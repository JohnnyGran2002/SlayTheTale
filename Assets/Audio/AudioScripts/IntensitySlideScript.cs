using UnityEngine;
using UnityEngine.UI;

public class IntensitySlideScript : MonoBehaviour
{
    public CombatMusicPlayer combatMusicPlayer;

    [SerializeField] private Slider _slider;
    
    public void OnValueChange()
    {
        Debug.Log(_slider.value);
        combatMusicPlayer.soundIntensityParameter.Intensity = _slider.value;
    }
}
