using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//scans the area around player and shows the nearest interactable
public class InteractableShower : MonoBehaviour
{
    public float scanRadius;
    public LayerMask scanLayer;
    public float scanInterval;
    float nextScanTime;

    InteractableUI interactableLastFrame;
    // Start is called before the first frame update
    void Start()
    {
        nextScanTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextScanTime)
        {
            Collider[] visibleColliders = Physics.OverlapSphere(transform.position, scanRadius, scanLayer);

            //get the nearest
            float nearestDistance = Mathf.Infinity;
            Collider nearestInteractible = null;

            for (int i = 0; i < visibleColliders.Length; i++)
            {
                Collider currentCollider = visibleColliders[i];
                //Debug.Log("currentCollir: " + currentCollider);
                float currentDistance = (currentCollider.transform.position - transform.position).sqrMagnitude;
                if (currentDistance < nearestDistance)
                {
                    nearestDistance = currentDistance;
                    nearestInteractible = currentCollider;
                }

            }
            InteractableUI interactableThisFrame = null;

            if (nearestInteractible != null)
            {
                interactableThisFrame = nearestInteractible.GetComponent<InteractableUI>();
            }
            else
            {
                //reset the one which is too far
                if(interactableLastFrame != null)
                {
                    interactableLastFrame.StopInteract();

                }
            }

            if (interactableLastFrame != null)
            {
                if(interactableLastFrame!= interactableThisFrame)
                {
                    interactableLastFrame.Hide();
                }
            }

            if (interactableThisFrame != null)
            {
                if (interactableLastFrame != interactableThisFrame)
                {
                    interactableThisFrame.Show();
                }
            }

            interactableLastFrame = interactableThisFrame;
        }

    }

    public void StartInteract()
    {
        if (interactableLastFrame != null)
        {
            interactableLastFrame.StartInteract(gameObject);
        }
    }

    public void HoldInteract()
    {
        if (interactableLastFrame != null)
        {
            interactableLastFrame.HoldInteract();
        }
    }

    public void StopInteract()
    {
        if (interactableLastFrame != null)
        {
            interactableLastFrame.StopInteract();

        }
    }

   
}
