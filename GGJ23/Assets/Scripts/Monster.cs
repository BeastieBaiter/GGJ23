using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [HideInInspector] public int currentHealth;
    public int maxHealth;
    public int damage;
    public int tier;

    public GameObject spritePrefab;

    public void Start()
    {
        currentHealth = maxHealth;
    }
}