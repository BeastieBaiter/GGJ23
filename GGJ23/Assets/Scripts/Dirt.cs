using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dirt : MonoBehaviour
{
    public int waterLevel;
    public Sprite[] spritesWaterLevel = new Sprite[4];
    public bool canBeBroken=false;
    CircleCollider2D circleCollider2D;
    
    void Start()
    {
        //this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = spritesWaterLevel[0];
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        waterLevel = 0;
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Broken"))
        {
            this.gameObject.GetComponent<Dirt>().canBeBroken = true;
        }
        
    }

    public void UpdateWaterLevel()
    {
        // percentage of possibility of water level increase
        int percentage = Random.Range(0, 100);
        if (percentage <= 20)
        {
            int waterLevelIncrease = Random.Range(waterLevel, 3);
            waterLevel = waterLevelIncrease;
            //log unity
            Debug.Log("Water level increased to " + waterLevel);
            //spriteRenderer.sprite = spritesWaterLevel[waterLevel];
        }
    }
}
