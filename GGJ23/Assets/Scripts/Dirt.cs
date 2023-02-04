using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dirt : MonoBehaviour
{
    public int waterLevel;
    public Sprite[] spritesWaterLevel = new Sprite[4];
    SpriteRenderer spriteRenderer;
    
    void Start()
    {
        waterLevel = 0;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.tag == "Dirt")
            {
                //remove Dirt if clicked
                //log unity
                Debug.Log(hit.collider.gameObject.name +"clicked");
                hit.collider.gameObject.SetActive(false);
            }
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
