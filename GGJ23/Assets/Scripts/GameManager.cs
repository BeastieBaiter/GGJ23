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
    int treeHealth { public get; private set;}
    
    public List<Monster> monsterArmy { public get; private set;}
    public List<Monster> enemyArmy { public get; private set;}

    private void Start()
    {
        waveCounter = 0;
    }

    public void HurtTree(int damage)
    {
        
    }

    public void BattleOver()
    {
        
    }
    
    public void GameOver()
    {
        
    }
}
