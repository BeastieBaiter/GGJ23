using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    
    public static WaveManager Instance { get; private set; }
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
    
    public int initialArmyStrength;
    public float linearProgressionMultiplier;

    public List<Monster> monsters;

    public List<Monster> GetNextWave(int waveNumber)
    {
        List<Monster> enemyArmy = new List<Monster>();
        int waveStrength = Mathf.RoundToInt(waveNumber * linearProgressionMultiplier * initialArmyStrength);
        while (GetArmyStrength(enemyArmy) < waveStrength)
        {
            enemyArmy.Add(monsters[Random.Range(0,3)]);
        }

        foreach (Monster m in enemyArmy)
        {
            m.currentHealth = m.maxHealth;
        }
        return enemyArmy;
    }

    private int GetArmyStrength(List<Monster> army)
    {
        int counter = 0;
        foreach (Monster m in army)
        {
            counter += m.damage;
        }
        return counter;
    }
}
