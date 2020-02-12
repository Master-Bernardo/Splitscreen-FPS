using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioListener : MonoBehaviour
{
    bool topDownMode;

    public Transform target;
    public Vector3 offset;
    public Vector3 topdownForward;


    void Update()
    {
        if (topDownMode)
        {
            transform.position = target.position + offset;
        }
       
    }

    public void SwitchToTopDownMode()
    {
        topDownMode = true;
        transform.SetParent(null);
        transform.forward = topdownForward;
    }

    public void SwitchToFpMode()
    {
        topDownMode = false;
        transform.SetParent(target);
        transform.localPosition = offset;
        transform.forward = target.forward;
    }
}
