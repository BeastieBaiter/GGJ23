using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public bool canBeBroken=false;
    CircleCollider2D circleCollider2D;
    
    public int index;
    
    void Start()
    {
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Broken"))
        {
            this.gameObject.GetComponent<Upgrade>().canBeBroken = true;
        }
        
    }
}
