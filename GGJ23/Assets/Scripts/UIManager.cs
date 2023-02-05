using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        }
    }

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private HealthBar healthBar;
    
    [Header("End Screen:")]
    [SerializeField] private GameObject endScreenPanel;
    [SerializeField] private TMP_Text endText;
    
    private GameManager _gameManager;
    private AudioManager _audioManager;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenuScene") return;
        _gameManager = GameManager.Instance;
        _audioManager = AudioManager.Instance;
        healthBar.SetEverything(_gameManager.maxTreeHealth);
    }

    public void UpdateHealthBar()
    {
        healthBar.SetHealth(_gameManager.currTreeHealth);
        healthBar.SetText();
    }

    public void ResetHealthBar()
    {
        healthBar.SetEverything(_gameManager.maxTreeHealth);
    }

    public void UpdateTimerText(string text)
    {
        timerText.text = text;
    }

    public void SceneLoader(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenEndScreenPanel(string text)
    {
        endScreenPanel.SetActive(true);
        endText.text = text;
    }
}
