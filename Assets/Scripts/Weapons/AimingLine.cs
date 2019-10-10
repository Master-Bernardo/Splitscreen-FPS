using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//visual representation of where the weapon is currently aiming
//later on there will be several representations - for niow only line
public class AimingLine : MonoBehaviour
{
    /*
    public LineRenderer lineRenderer;
    public int maxDistance;
    public LayerMask layerMask;

    public void ChangeLayer(int playerUILayer)
    {
        //lineRenderer.sortingLayerID = playerUILayer;
        gameObject.layer = playerUILayer;

    }
    public void DrawLine()
    {
        lineRenderer.SetPosition(0, transform.position);

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance, layerMask))
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + transform.forward * maxDistance);
        }
    }*/
}
