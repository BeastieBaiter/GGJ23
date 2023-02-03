using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
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

    [SerializeField] private float duration  = 100;
    private float _timeRemaining;
    public bool TimerRunning { get; private set; }
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private UIManager _uiManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _timeRemaining = duration;
        TimerRunning = true;
        _gameManager = GameManager.Instance;
        _audioManager = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerRunning)
        {
            if (_timeRemaining > 0)
            {
                
                _timeRemaining -= Time.deltaTime;
            }
            else
            {
                _timeRemaining = 0f;
                TimerRunning = false;
                //_audioManager.Play("Timer Over");
            }
        }
    }

    public string DisplayTime()
    {
        float minutes = Mathf.FloorToInt(_timeRemaining / 60);
        float seconds =  Mathf.FloorToInt(_timeRemaining % 60); 
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer()
    {
        _timeRemaining = duration;
        TimerRunning = true;
    }
}
