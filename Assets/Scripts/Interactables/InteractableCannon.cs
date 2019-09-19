using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCannon : MonoBehaviour
{
    public InteractableUI reloadInteractible;
    public InteractableUI shootInteractable;

    public GameObject projectilePrefab;
    public float projectileLaunchSpeed;
    public Transform shootPoint;

    bool loaded = false;

    public void OnReloadCompleted()
    {
        reloadInteractible.gameObject.SetActive(false);
        shootInteractable.gameObject.SetActive(true);

        loaded = true;
    }

    public void OnShootButtonPressed()
    {
        reloadInteractible.gameObject.SetActive(true);
        shootInteractable.gameObject.SetActive(false);
        Shoot();
        loaded = false;
    }


     void Shoot()
     {
         Projectile projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation).GetComponent<Projectile>();
         projectile.SetVelocity(projectileLaunchSpeed);
     }


}
