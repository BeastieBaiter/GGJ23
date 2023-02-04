using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{

    public float armyGap;
    public Transform enemyArmyPos;
    public Transform monsterArmyPos;

    public GameObject testPrefab;
    
    public float minMultiplier;
    public float maxMultiplier;

    private List<Monster> _monsterArmy;
    private List<Monster> _enemyArmy;

    private void Start()
    {
        //DisplayInGridTest();
        StartBattle();
    }

    public void StartBattle()
    {
        //_monsterArmy = GameManager.Instance.monsterArmy;
        //_enemyArmy = GameManager.Instance.enemyArmy;
        _monsterArmy = WaveManager.Instance.GetNextWave(2);
        _enemyArmy = WaveManager.Instance.GetNextWave(1);

        DisplayInGrid(true);
        while (_monsterArmy.Count > 0 || _enemyArmy.Count > 0)
        {
            Attack();
            DisplayInGrid(false);
            Debug.Log("The monster army has " + _monsterArmy.Count + " monsters");
            Debug.Log("The enemy army has " + _enemyArmy.Count + " monsters");

        }

        if (_monsterArmy.Count == 0 && GameManager.Instance.currTreeHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }

        if (_enemyArmy.Count == 0)
        {
            GameManager.Instance.BattleOver(_monsterArmy);
        }
    }

    private void DisplayInGrid(bool firstDraw)
    {
        
        //A BOOL é para dar beautify se tiver tempo, e basicamente por os sprites a aparecerem 1 a 1 (mas rapidamente) 
        //com som e tudo
        
        //O CODIGO SÓ DÁ RENDER DE 100 SPRITES
        //SE SOBRAR MAIS DO EXÉRCITO, NÃO SÃO DADOS RENDER, ATÉ AO PRÓXIMO ATAQUE
        
        for (int i = enemyArmyPos.childCount - 1; i >= 0; i--)
        {
            Destroy(enemyArmyPos.GetChild(i).gameObject);
        }
        
        for (int i = monsterArmyPos.childCount - 1; i >= 0; i--)
        {
            Destroy(monsterArmyPos.GetChild(i).gameObject);
        }
        
        List<Monster> reverseArmy = _enemyArmy;
        reverseArmy.Reverse();
        for (int i = 0; i < 10 * 10 && (i < reverseArmy.Count); i++)
        {
            Instantiate(reverseArmy[i].spritePrefab,
                enemyArmyPos.position + new Vector3(armyGap * (i % 10), armyGap * (i / 10), 0),
                Quaternion.identity, enemyArmyPos);
        }
        
        reverseArmy = _monsterArmy;
        reverseArmy.Reverse();
        for (int i = 0; i < 10 * 10 && i < reverseArmy.Count; i++)
        {
            Instantiate(reverseArmy[i].spritePrefab,
                monsterArmyPos.position + new Vector3(armyGap * (i % 10), armyGap * (i / 10), 0),
                Quaternion.identity, monsterArmyPos);
        }
    }

    private void Attack()
    {
        int monsterArmyStrength = 0;
        int enemyArmyStrength = 0;

        foreach (Monster m in _monsterArmy)
        {
            monsterArmyStrength += m.damage;
        }

        foreach (Monster m in _enemyArmy)
        {
            enemyArmyStrength += m.damage;
        }

        int monsterArmyDamage = Mathf.RoundToInt(monsterArmyStrength * Random.Range(minMultiplier, maxMultiplier));
        int enemyArmyDamage = Mathf.RoundToInt(enemyArmyStrength * Random.Range(minMultiplier, maxMultiplier));

        Debug.Log("The monster army is dealing " + monsterArmyDamage + " damage");
        Debug.Log("The enemy army is dealing " + enemyArmyDamage + " damage");

        int excessDamage = ApplyDamage(_monsterArmy, enemyArmyDamage);
        Debug.Log("The monster army suffered " + excessDamage + " excess damage");
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
                : Mathf.Clamp(damageLeft - (m.maxHealth - m.currentHealth), 0, damage);
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
    
    
    private void DisplayInGridTest()
    {
     
        for (int i = 0; i < 10 * 10; i++)
        {
            Instantiate(testPrefab,
                enemyArmyPos.position + new Vector3(armyGap * (i % 10), armyGap * (i / 10), 0),
                Quaternion.identity, enemyArmyPos);
        }
             
        for (int i = 0; i < 10 * 10; i++)
        {
            Instantiate(testPrefab,
                monsterArmyPos.position + new Vector3(armyGap * (i % 10), armyGap * (i / 10), 0),
                Quaternion.identity, monsterArmyPos);
        }
    }
}