using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{

    public float armySize;
    public Transform enemyArmyPos;
    public Transform monsterArmyPos;

    public GameObject testPrefab;
    
    public float minMultiplier;
    public float maxMultiplier;

    private List<Monster> _monsterArmy;
    private List<Monster> _enemyArmy;

    private void Start()
    {
        DisplayInGridTest();
    }

    public void StartBattle()
    {
        _monsterArmy = GameManager.Instance.monsterArmy;
        _enemyArmy = GameManager.Instance.enemyArmy;
        int enemyArmySide = Mathf.CeilToInt(Mathf.Sqrt(_enemyArmy.Count));
        int monsterArmySide = Mathf.CeilToInt(Mathf.Sqrt(_monsterArmy.Count));

        DisplayInGrid(enemyArmySide, monsterArmySide);
        while (_monsterArmy.Count > 0 || _enemyArmy.Count > 0)
        {
            Attack();
        }

        if (_monsterArmy.Count == 0 && GameManager.Instance.treeHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }

        if (_enemyArmy.Count > 0)
        {
            GameManager.Instance.BattleOver(_monsterArmy);
        }
    }

    private void DisplayInGrid(int enemyArmySide, int monsterArmySide)
    {
        List<Monster> reverseArmy = _enemyArmy;
        reverseArmy.Reverse();
        float xSpace = armySize - (enemyArmySide * reverseArmy[0].transform.localScale.x);
        float ySpace = armySize - (enemyArmySide * reverseArmy[0].transform.localScale.y);
        for (int i = 0; i < enemyArmySide * enemyArmySide; i++)
        {
            Instantiate(reverseArmy[i].spritePrefab,
                enemyArmyPos.position + new Vector3(xSpace * (i % enemyArmySide), ySpace * (i / enemyArmySide), 0),
                Quaternion.identity);
        }
        
        reverseArmy = _monsterArmy;
        reverseArmy.Reverse();
        xSpace = armySize - (monsterArmySide * reverseArmy[0].transform.localScale.x);
        ySpace = armySize - (monsterArmySide * reverseArmy[0].transform.localScale.y);
        for (int i = 0; i < monsterArmySide * monsterArmySide; i++)
        {
            Instantiate(reverseArmy[i].spritePrefab,
                monsterArmyPos.position + new Vector3(xSpace * (i % monsterArmySide), ySpace * (i / monsterArmySide), 0),
                Quaternion.identity);
        }
    }
    
    private void DisplayInGridTest()
    {
        int enemyArmySide = 10;
        float xSpace = (armySize - (enemyArmySide * testPrefab.transform.localScale.x))/enemyArmySide;
        float ySpace = (armySize - (enemyArmySide * testPrefab.transform.localScale.y))/enemyArmySide;
        Debug.Log(xSpace);
        Debug.Log(ySpace);

        for (int i = 0; i < enemyArmySide * enemyArmySide; i++)
        {
            Instantiate(testPrefab,
                enemyArmyPos.position + new Vector3(xSpace * (i % enemyArmySide), ySpace * (i / enemyArmySide), 0),
                Quaternion.identity);
        }
        
        int monsterArmySide = 7;
        xSpace = (armySize - (monsterArmySide * testPrefab.transform.localScale.x))/monsterArmySide;
        ySpace = (armySize - (monsterArmySide * testPrefab.transform.localScale.y))/monsterArmySide;
        for (int i = 0; i < monsterArmySide * monsterArmySide; i++)
        {
            Instantiate(testPrefab,
                monsterArmyPos.position + new Vector3(xSpace * (i % monsterArmySide), ySpace * (i / monsterArmySide), 0),
                Quaternion.identity);
        }
    }

    private void Attack()
    {
        int monsterArmyStrenght = 0;
        int enemyArmyStrenght = 0;

        foreach (Monster m in _monsterArmy)
        {
            monsterArmyStrenght += m.damage;
        }

        foreach (Monster m in _enemyArmy)
        {
            enemyArmyStrenght += m.damage;
        }

        int monsterAttackDamage = Mathf.RoundToInt(monsterArmyStrenght * Random.Range(minMultiplier, maxMultiplier));
        int enemyArmyDamage = Mathf.RoundToInt(enemyArmyStrenght * Random.Range(minMultiplier, maxMultiplier));

        int excessDamage = ApplyDamage(_monsterArmy, enemyArmyDamage);
        ApplyDamage(_enemyArmy, monsterAttackDamage);

        GameManager.Instance.HurtTree(excessDamage);
    }

    private int ApplyDamage(List<Monster> army, int damage)
    {
        int damageLeft = damage;
        List<Monster> newArmy = new List<Monster>();
        foreach (Monster m in army)
        {
            int healthLeft = m.currentHealth - damageLeft;
            damageLeft = healthLeft <= 0
                ? Mathf.Clamp(damageLeft - m.maxHealth, 0, damage)
                : Mathf.Clamp(damageLeft - (m.maxHealth - m.currentHealth), 0, damage);
            if (healthLeft > 0)
            {
                newArmy.Add(m);
            }
        }
        
        if (army == _monsterArmy)
        {
            _monsterArmy = newArmy;
            _monsterArmy.Reverse();
        }

        if (army == _enemyArmy)
        {
            _enemyArmy = newArmy;
            _enemyArmy.Reverse();
        }

        return damageLeft > 0 ? damageLeft : 0;
    }
}