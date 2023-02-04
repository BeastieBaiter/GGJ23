using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public bool canBeBroken=false;
    CircleCollider2D circleCollider2D;
    
    void Start()
    {
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Broken"))
        {
            this.gameObject.GetComponent<Dirt>().canBeBroken = true;
        }
        
    }
}
