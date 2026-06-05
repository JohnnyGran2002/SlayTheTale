using System;
using TMPro;
using UnityEngine;

public class DeathScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text deathText;
    [SerializeField] private TMP_Text winText;

    private void Awake()
    {
        deathText.enabled = false;
        winText.enabled = false;
    }

    public void SetText(bool isAlive)
    {
        if (isAlive) winText.enabled = true;
        else deathText.enabled = true;
    }
}
