using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AimVisualisationType
{
    Line, //typical gun visualisation, has start end and spread
    Circle, //for grenades or bows, which also get thrown - we can click for a quick bowshot or hold to aim
    MeshPlacement //for scrtructures like barricades etc

}

public class AimVisualiser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public LayerMask layerMask;
    public float lineStartWidth;
    //works together with weaponSystem to visualise where the player is aiming with guns/grenades or where he wants to build a barricade

    #region line

    public void ShowLine()
    {
        lineRenderer.enabled = true;
        Debug.Log("show line");
    }

    public void HideLine()
    {
        lineRenderer.enabled = false;
        Debug.Log("hide line");

    }

    public void DrawLine(Vector3 start, float maxDistance, float spread)
    {
        //spread is bloom, distance depends on wepon, shotgunhas shorter distance than sniper

        float lineLength;

        lineRenderer.SetPosition(0, start);
        lineRenderer.startWidth = lineStartWidth; 

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(start, transform.TransformDirection(Vector3.forward), out hit, maxDistance, layerMask))
        {
            lineRenderer.SetPosition(1, hit.point);
            lineLength = (hit.point- start).magnitude;
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + transform.forward * maxDistance);
            lineLength = maxDistance;
        }

        lineRenderer.endWidth = (Mathf.Tan(spread)*lineLength)*2;

    }

    #endregion
}
