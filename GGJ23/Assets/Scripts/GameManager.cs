using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
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

    private int waveCounter;
    [HideInInspector] public int currTreeHealth;
    public int maxTreeHealth;
    
    public List<Monster> monsterArmy { get; private set;}
    public List<Monster> enemyArmy { get; private set;}
    public Grid grid { get; private set;}

    private void Start()
    {
        waveCounter = 0;
        currTreeHealth = maxTreeHealth;
    }

    public void Update()
    {
        
    }
    
    

    public void HurtTree(int damage)
    {
        
    }

    public void BattleStart()
    {
        
    }
    
    public void BattleOver(List<Monster> newMonsterArmy)
    {
        
    }
    
    public void GameOver()
    {
        
    }
}
