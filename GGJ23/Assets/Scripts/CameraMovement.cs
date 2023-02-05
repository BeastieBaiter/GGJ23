using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float topBoundary, bottomBoundary;

    void Update()
    {
        if (GameManager.Instance.battleStarted)
        {
            var ct = transform;
            ct.position = new Vector3(0, 0, ct.position.z);
            return;
        }
        int sign = 0;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("DOWN");
            sign = -1;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("UP");
            sign = 1;
        }

        float input = sign * (cameraSpeed / 100);
        var cameraTransform = transform;
        cameraTransform.position = new Vector3(
            0,
            Mathf.Clamp(cameraTransform.position.y + input , bottomBoundary, topBoundary),
            cameraTransform.position.z);
    }
}
