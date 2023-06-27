using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.123f;
    public Vector3 offset = new Vector3 (0.2f, 0.0f, -10f);
    public float danpingTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void MoveCamera(bool smooth)
    {
        Vector3 destination = new Vector3(target.position.x - offset.x, offset.y, offset.z);

        if (smooth)
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, danpingTime);
 
        }
        else
        {
            this.transform.position = destination;
        }
    }


    private void LateUpdate()
    {
        MoveCamera(true);
    }
}
