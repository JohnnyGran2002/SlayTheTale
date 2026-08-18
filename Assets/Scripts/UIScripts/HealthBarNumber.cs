using TMPro;
using UnityEngine;

public class HealthBarNumber : MonoBehaviour
{
    private int _maxHealth;
    private int _curreHealth;
    private TMP_Text _healthText;
    [SerializeField] private Health _health;
    private void Start()
    {
        
    }
    public void UpdateHealthNumber(Component sender, object data)
    {
        _curreHealth = _health.CurrentHealth;
        _maxHealth = _health.MaxHealth;
        _healthText.text = _curreHealth.ToString() + "/" + _maxHealth.ToString();
    }
}
