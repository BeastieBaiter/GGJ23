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
    public List<Monster> monsterList { public get; private set;}
    public List<Monster> enemiesList { public get; private set;}

    private void Start()
    {
        waveCounter = 0;
    }
}
