using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AimVisualisationType
{
    Line, //Very simple line visualisation with raycasts
    Cone, //typical gun visualisation, has start end and spread
    Circle, //for grenades or bows, which also get thrown - we can click for a quick bowshot or hold to aim
    MeshPlacement //for scrtructures like barricades etc
}

public enum AimVisualisationMode
{
    FirstPerson,
    TopDown
}

public class AimVisualiser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    //public LayerMask layerMask;
    public float lineStartWidth;

    public LayerMask lineLayerMask;
    AimVisualisationMode currentAimVisualisationMode;
    //works together with weaponSystem to visualise where the player is aiming with guns/grenades or where he wants to build a barricade



    public void ShowLine()
    {
        if (currentAimVisualisationMode == AimVisualisationMode.TopDown)
        {
            lineRenderer.enabled = true;
            Debug.Log("show line");
        }
    }

    public void HideLine()
    {
        lineRenderer.enabled = false;
        Debug.Log("hide line");

    }

    public void DrawCone(Vector3 start, Vector3 direction, float maxDistance, float spread)
    {
        //spread is bloom, distance depends on wepon, shotgunhas shorter distance than sniper

        float lineLength;

        lineRenderer.SetPosition(0, start);
        lineRenderer.startWidth = lineStartWidth; 

        /*RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(start, direction, out hit, maxDistance, layerMask))
        {
            lineRenderer.SetPosition(1, hit.point);
            lineLength = (hit.point- start).magnitude;
        }
        else
        {*/
            lineRenderer.SetPosition(1, start + direction * maxDistance);
            lineLength = maxDistance;
        //}

        lineRenderer.endWidth = (Mathf.Tan(spread * Mathf.Deg2Rad) *lineLength)*2;

    }

    public void DrawLine(Vector3 start, Vector3 direction, float maxDistance)
    {
        if (currentAimVisualisationMode == AimVisualisationMode.TopDown)
        {

            lineRenderer.SetPosition(1, start);
            lineRenderer.startWidth = lineStartWidth;

            lineRenderer.SetPosition(0, start- direction.normalized*0.5f);
            lineRenderer.startWidth = lineStartWidth;

            Vector3 end;

            RaycastHit hit;

            if (Physics.Raycast(start, direction, out hit, maxDistance, lineLayerMask))
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
                end = hit.point;
            }
            else
            {
                end = start + direction.normalized * maxDistance;
            }

            lineRenderer.SetPosition(2, end);
            lineRenderer.startWidth = lineStartWidth;
        }
    }


    public void ChangeVisualisationMode(AimVisualisationMode newMode)
    {
        currentAimVisualisationMode = newMode;

        if (currentAimVisualisationMode == AimVisualisationMode.FirstPerson)
        {
            HideLine();
        }
    }
  
}
