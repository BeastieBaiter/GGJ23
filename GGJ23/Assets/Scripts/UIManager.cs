using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) 
            Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private HealthBar healthBar;

    private GameManager _gameManager;
    private AudioManager _audioManager;

    public void Start()
    {
        _gameManager = GameManager.Instance;
        _audioManager = AudioManager.Instance;
        healthBar.SetEverything(_gameManager.maxTreeHealth);
    }

    public void UpdateHealthBar()
    {
        healthBar.SetHealth(_gameManager.currTreeHealth);
        healthBar.SetText();
    }

    public void UpdateTimerText(string text)
    {
        timerText.text = text;
    }
}
