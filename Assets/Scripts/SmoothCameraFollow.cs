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

    public float smoothTime = 4f;
    Vector3 offset;
    Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        offset = baseOffset * distanceToTarget;
        //offset = transform.position - target.position ;
        //positionLastFame = transform.position;
    }

    public void SetUpForSplitscreen()
    {
        offset *= splitscreenDistanceModifier;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity,  smoothTime);
        transform.position = smoothedPosition;
    }

    public void TeleportToDesiredPosition()
    {
        Vector3 desiredPosition = target.position + offset;
        
        transform.position = desiredPosition;
    }
}
