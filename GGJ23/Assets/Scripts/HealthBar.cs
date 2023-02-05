using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text healthText;
    
    private int maxHealth, currHealth;

    public void SetEverything(int maxHealthValue)
    {
        maxHealth = maxHealthValue;
        currHealth = maxHealthValue;
        slider.maxValue = maxHealth;
        slider.value = currHealth;
        SetText();
    }
    
    public void SetMaxHealth(int maxHealthValue)
    {
        maxHealth = maxHealthValue;
        slider.maxValue = maxHealthValue;
    }
    
    public void SetHealth(int currHealthValue)
    {
        currHealth = currHealthValue;
        slider.value = currHealthValue;
    }

    public void SetText()
    {
        healthText.text = currHealth + " / " + maxHealth;
    }
}
