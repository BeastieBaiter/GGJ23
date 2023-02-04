using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int initialArmyStrength;
    public int linearProgressionMultiplier;

    public List<Monster> monsters;

    public List<Monster> GetNextWave(int waveNumber)
    {
        List<Monster> enemyArmy = new List<Monster>();
        int waveStrength = waveNumber * linearProgressionMultiplier * initialArmyStrength;
        while (GetArmyStrength(enemyArmy) < waveStrength)
        {
            enemyArmy.Add(monsters[Random.Range(0,3)]);
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
