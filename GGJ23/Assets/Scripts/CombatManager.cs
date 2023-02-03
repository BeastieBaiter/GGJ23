using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;

    public float minMultiplier;
    public float maxMultiplier;

    private List<Monster> _monsterArmy;
    private List<Monster> _enemyArmy;

    public void StartBattle()
    {
        _monsterArmy = GameManager.Instance.monsterArmy;
        _enemyArmy = GameManager.Instance.enemyArmy;
        DisplayInGrid();
        while (_monsterArmy.Count > 0 || _enemyArmy.Count > 0)
        {
            Attack();
        }

        if (_monsterArmy.Count == 0 && GameManager.Instance.currTreeHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }

        if (_enemyArmy.Count > 0)
        {
            GameManager.Instance.BattleOver(_monsterArmy);
        }
    }

    private void DisplayInGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; i++)
            {
                //meter os inimigos em grid, em ordem inversa
            }
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