using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI healthText;
    public PlayerMovement player;

    public bool isDead = false;

    void Start()
    {
        slider.maxValue = player.maxHealth;
        slider.value = player.currentHealth;

        UpdateText(player.currentHealth, player.maxHealth);
    }

    void Update()
    {
        if (isDead == true) return;
        slider.maxValue = player.maxHealth;
        slider.value = player.currentHealth;
        UpdateText(player.currentHealth, slider.maxValue);

        if (player.currentHealth <= 0)
        {
            isDead = true;
            UpdateText(0, player.maxHealth);
        }
    }

    private void UpdateText(float current, float max)
    {
        if (isDead == true) return;
        healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }
}


