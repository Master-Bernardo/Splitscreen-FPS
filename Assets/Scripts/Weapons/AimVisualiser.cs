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

/*public enum AimVisualisationMode
{
    FirstPerson,
    TopDown
}*/

public class AimVisualiser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    //public LayerMask layerMask;
    public float lineStartWidth;

    public LayerMask lineLayerMask;
    //AimVisualisationMode currentAimVisualisationMode;

    Material lineMaterial;
    //works together with weaponSystem to visualise where the player is aiming with guns/grenades or where he wants to build a barricade

    enum AnimationState //for appearing and dissapearing animation
    {
        Showing,
        FullShown,
        Hiding,
        Hidden
    }

    AnimationState animationState  = AnimationState.Hidden;
    public float appearAnimationSpeed;



    public void ShowLine()
    {
        //if (currentAimVisualisationMode == AimVisualisationMode.TopDown)
        //{
            lineMaterial = lineRenderer.material;
            BeginnShowingAnimation();
            //lineRenderer.enabled = true;
        //}
    }

    public void HideLine()
    {
        //if (currentAimVisualisationMode == AimVisualisationMode.TopDown)
        //{
            if (animationState == AnimationState.FullShown)
            {
                BeginnHidingAnimation();
            }
        //}
        //    lineRenderer.enabled = false;
        //Debug.Log("hide line");

    }

    public void HideLineInstantly()
    {
        lineRenderer.enabled = false;
    }

    public void ShowLineInstantly()
    {
        lineRenderer.enabled = true;
    }

    void BeginnShowingAnimation()
    {
        animationState = AnimationState.Showing;
        lineMaterial.color = new Color(1, 1, 1, 0);
        //lineRenderer.enabled = true;

    }

    void EndShowingAnimation()
    {
        animationState = AnimationState.FullShown;
        lineMaterial.color = new Color(1, 1, 1, 1);
    }

    void BeginnHidingAnimation()
    {
        animationState = AnimationState.Hiding;
        lineMaterial.color = new Color(1, 1, 1, 1);
    }

    void EndHidingAnimation()
    {
        animationState = AnimationState.Hidden;
        lineMaterial.color = new Color(1, 1, 1, 0);
        //lineRenderer.enabled = false;
    }


    private void Update()
    {
        if(animationState == AnimationState.Showing)
        {
            float alpha = lineMaterial.color.a + appearAnimationSpeed * Time.deltaTime;
            if (alpha >= 1)
            {
                EndShowingAnimation();
            }
            else
            {
                lineMaterial.color = new Color(1, 1, 1, alpha);
            }
            
        }
        else if(animationState == AnimationState.Hiding)
        {
            float alpha = lineMaterial.color.a - appearAnimationSpeed * Time.deltaTime;
            if (alpha <= 0)
            {
                EndHidingAnimation();
            }
            else
            {

            }
            lineMaterial.color = new Color(1, 1, 1, alpha);
        }
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
       // if (currentAimVisualisationMode == AimVisualisationMode.TopDown)
        //{

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
        //}
    }



    /*public void ChangeVisualisationMode(AimVisualisationMode newMode)
    {
        currentAimVisualisationMode = newMode;

        if (currentAimVisualisationMode == AimVisualisationMode.FirstPerson)
        {
            HideLine();
        }
    }*/
  
}
