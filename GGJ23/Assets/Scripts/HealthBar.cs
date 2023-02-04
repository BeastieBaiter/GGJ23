using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    
    private int maxHealth, currHealth;
    private TMP_Text healthText;

    public void SetEverything(int maxHealthValue)
    {
        SetMaxHealth(maxHealthValue);
        SetHealth(maxHealthValue);
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
