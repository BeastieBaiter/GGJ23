using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSetter : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.Play("Ambient");
        AudioManager.Instance.Play("Music");
        
    }
}
