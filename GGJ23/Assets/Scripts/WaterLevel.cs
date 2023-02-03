using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    
    public int waterLevel;
    public Sprite[] spritesWaterLevel = new Sprite[4];
    public SpriteRenderer spriteRenderer;
    
    void Start()
    {
        waterLevel = 0;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void UpdateWaterLevel()
    {
        waterLevel++;
        spriteRenderer.sprite = spritesWaterLevel[waterLevel];
    }
}
