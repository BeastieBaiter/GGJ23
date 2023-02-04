using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 _cameraPosition;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float topBoundry, bottomBoundry;
    
    void Start()
    {
        _cameraPosition = transform.position;
    }

    void Update()
    {
        _cameraPosition.y += Input.GetAxisRaw("Vertical") * (cameraSpeed / 100);
        if (_cameraPosition.y > topBoundry || _cameraPosition.y < bottomBoundry) return;
        transform.position = _cameraPosition;
    }
}
