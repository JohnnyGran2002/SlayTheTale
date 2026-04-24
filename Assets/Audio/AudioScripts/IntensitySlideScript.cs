using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class IntensitySlideScript : MonoBehaviour
{

    [SerializeField] private Slider _slider;
    
    public void OnValueChange()
    {
        MusicManager.musicManager.soundIntensityParameter.Intensity = _slider.value;
    }
}
