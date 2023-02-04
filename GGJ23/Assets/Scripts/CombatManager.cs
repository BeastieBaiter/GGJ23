using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{

    public static CombatManager Instance { get; private set; }
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

    public TMP_Text VStext;
    public TMP_Text enemyStats;
    public TMP_Text monsterStats;
    public TMP_Text enemyDamage;
    public TMP_Text monsterDamage;
    public TMP_Text enemyCounter;
    public TMP_Text monsterCounter;
    //public TMP_Text timer;
    
    public float armyGap;
    public Transform enemyArmyPos;
    public Transform monsterArmyPos;

    public float timeBetweenAttacks = 10f;
    public float timeBetweenSpawns = 0.2f;
    private float _timeLeft;
    
    public float minMultiplier;
    public float maxMultiplier;

    private List<Monster> _monsterArmy;
    private List<Monster> _enemyArmy;

    private void Start()
    {
        StartBattle();
    }

    private void Update()
    {
        _timeLeft = Mathf.Clamp(_timeLeft - Time.deltaTime, 0, 10f);
        //timer.fillAmount = _timeLeft / 10f;
        //timer.text = _timeLeft.ToString();
    }

    public void StartBattle()
    {
        //_monsterArmy = GameManager.Instance.monsterArmy;
        //_enemyArmy = GameManager.Instance.enemyArmy;
        _monsterArmy = WaveManager.Instance.GetNextWave(20);
        _enemyArmy = WaveManager.Instance.GetNextWave(15);
        _timeLeft = timeBetweenAttacks;
        StartCoroutine(BattleProcess());
    }

    private IEnumerator BattleProcess()
    {
        int counter = 0;
        DisplayInGrid(true);
        yield return new WaitForSeconds(timeBetweenAttacks);
        _timeLeft = timeBetweenAttacks;
        while (counter < 10)
        {
            if (_monsterArmy.Count <= 0 || _enemyArmy.Count <= 0)
            {
                break;
            }
            Attack();
            DisplayInGrid(true);
            counter++;
            if (_monsterArmy.Count <= 0 || _enemyArmy.Count <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(timeBetweenAttacks);
            _timeLeft = timeBetweenAttacks;
        }
       
        if (GameManager.Instance.currTreeHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            GameManager.Instance.BattleOver(_monsterArmy);
        } 
    }

    private void DisplayInGrid(bool firstDraw)
    {
        
        for (int i = enemyArmyPos.childCount - 1; i >= 0; i--)
        {
            Destroy(enemyArmyPos.GetChild(i).gameObject);
        }
        
        for (int i = monsterArmyPos.childCount - 1; i >= 0; i--)
        {
            Destroy(monsterArmyPos.GetChild(i).gameObject);
        }
        
        int monsterArmyStrength = GetArmyStrength(_monsterArmy);
        int enemyArmyStrength = GetArmyStrength(_enemyArmy);
        int monsterArmyHealth = GetArmyHealth(_monsterArmy);
        int enemyArmyHealth = GetArmyHealth(_enemyArmy);

        monsterStats.text = "HP: " + monsterArmyHealth + "   STR: " + monsterArmyStrength;

        enemyStats.text = "HP: " + enemyArmyHealth + "   STR: " + enemyArmyStrength;

        enemyCounter.text = _enemyArmy.Count.ToString();
        monsterCounter.text = _monsterArmy.Count.ToString();

        StartCoroutine(DrawSprites(firstDraw, _enemyArmy, enemyArmyPos));
        StartCoroutine(DrawSprites(firstDraw, _monsterArmy, monsterArmyPos));

    }
    
    private IEnumerator DrawSprites(bool firstDraw, List<Monster> army, Transform anchor)
    {
        List<Monster> reverseArmy = army;
        reverseArmy.Reverse();
        for (int i = 0; i < 10 * 10 && (i < reverseArmy.Count); i++)
        {
            Instantiate(reverseArmy[i].spritePrefab,
                anchor.position + new Vector3(armyGap * (i % 10), armyGap * (i / 10), 0),
                Quaternion.identity, enemyArmyPos);
            if (firstDraw)
            {
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }

    private void Attack()
    {
        int monsterArmyStrength = GetArmyStrength(_monsterArmy);
        int enemyArmyStrength = GetArmyStrength(_enemyArmy);
        
        int monsterArmyDamage = Mathf.RoundToInt(monsterArmyStrength * Random.Range(minMultiplier, maxMultiplier));
        int enemyArmyDamage = Mathf.RoundToInt(enemyArmyStrength * Random.Range(minMultiplier, maxMultiplier));

        enemyDamage.text = enemyArmyDamage.ToString();
        monsterDamage.text = monsterArmyDamage.ToString();
        
        int excessDamage = ApplyDamage(_monsterArmy, enemyArmyDamage);
        ApplyDamage(_enemyArmy, monsterArmyDamage);

        GameManager.Instance.HurtTree(excessDamage);
    }

    private int ApplyDamage(List<Monster> army, int damage)
    {
        int damageLeft = damage;
        List<Monster> newArmy = new List<Monster>();
        List<Monster> toDestroy = new List<Monster>();
        foreach (Monster m in army)
        {
            int healthLeft = m.currentHealth - damageLeft;
            damageLeft = healthLeft <= 0
                ? Mathf.Clamp(damageLeft - m.maxHealth, 0, damage)
                : 0;
            if (healthLeft > 0)
            {
                newArmy.Add(m);
            }
            else
            {
                toDestroy.Add(m);
            }
        }

        /*if (toDestroy.Count > 0)
        {
            foreach (Monster m in toDestroy)
            {
                if (m.gameObject.activeSelf)
                {                
                    toDestroy.Remove(m);
                    Destroy(m.gameObject);
                }

            }
        }*/

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
    
    private int GetArmyStrength(List<Monster> army)
    {
        int counter = 0;
        foreach (Monster m in army)
        {
            counter += m.damage;
        }
        return counter;
    }
    
    private int GetArmyHealth(List<Monster> army)
    {
        int counter = 0;
        foreach (Monster m in army)
        {
            counter += m.currentHealth;
        }
        return counter;
    }
}