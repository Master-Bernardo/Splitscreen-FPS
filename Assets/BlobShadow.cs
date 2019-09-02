using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobShadow : MonoBehaviour
{
    public Transform projectionPoint;
    public Transform shadow;
    public LayerMask layer;

    public Vector3 raycastDirection;
    Vector3 correctionVector = new Vector3(0, 0.01f, 0);

    void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(projectionPoint.position, raycastDirection, out hit, Mathf.Infinity, layer))
        {
            shadow.position = hit.point + correctionVector;
            shadow.up = Vector3.up;
        }

    }
}
