using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class IntensitySlideScript : MonoBehaviour
{

    [SerializeField] private Slider _slider;
    
    public void OnValueChange()
    {
        Debug.Log(_slider.value);
        MusicManager.musicManager.soundIntensityParameter.Intensity = _slider.value;
    }
}
