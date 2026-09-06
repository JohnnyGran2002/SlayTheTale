using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthSlider1, healthSlider2;
    
    // Update is called once per frame
    void Update()
    {
        healthText.text = PlayerStatic.instance.currentHealth.ToString() + "/" + PlayerStatic.instance.maxHealth.ToString();
        healthSlider1.value = PlayerStatic.instance.currentHealth / 100f;
        healthSlider2.value = PlayerStatic.instance.currentHealth / 100f;
    }
}
