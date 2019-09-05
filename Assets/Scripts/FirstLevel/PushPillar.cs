using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPillar : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 force;
    public Vector3 position;

    public void Push()
    {
        rb.isKinematic = false;
        rb.AddForceAtPosition(force, transform.InverseTransformPoint(position), ForceMode.Impulse);
    }
}
