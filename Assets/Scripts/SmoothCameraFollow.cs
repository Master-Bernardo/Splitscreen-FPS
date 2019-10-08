using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 baseOffset;
    public float distanceToTarget;
    [Tooltip("if we switch to splitscreen , we will multiply the distance by this value")]
    public float splitscreenDistanceModifier;

    public float smoothSpeed = 4f;
    Vector3 offset;

    private void Awake()
    {
        offset = baseOffset * distanceToTarget;
        //offset = transform.position - target.position ;
    }

    public void SetUpForSplitscreen()
    {
        offset *= splitscreenDistanceModifier;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        //transform.LookAt(target);
    }

    public void TeleportToDesiredPosition()
    {
        Vector3 desiredPosition = target.position + offset;
        
        transform.position = desiredPosition;
    }
}
