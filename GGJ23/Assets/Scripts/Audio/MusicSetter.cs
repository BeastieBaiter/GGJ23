using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSetter : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.Play("Ambient");
        AudioManager.instance.Play("Music");
        
    }
}
