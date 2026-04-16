using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class IntensitySlideScript : MonoBehaviour
{
    public MusicManager musicManager;

    [SerializeField] private Slider _slider;
    
    public void OnValueChange()
    {
        Debug.Log(_slider.value);
        musicManager.soundIntensityParameter.Intensity = _slider.value;
    }
}
