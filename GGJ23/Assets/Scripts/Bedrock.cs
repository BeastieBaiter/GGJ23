using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bedrock : MonoBehaviour
{
    public bool canBeBroken=false;
    GameManager gameManager;
    CircleCollider2D circleCollider2D;
    
    void Start()
    {
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Broken") && gameManager.counter == 3)
        {
            this.gameObject.GetComponent<Bedrock>().canBeBroken = true;
        }
        
    }
}
