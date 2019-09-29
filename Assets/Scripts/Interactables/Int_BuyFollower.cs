using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Int_BuyFollower : MonoBehaviour
{
    InteractableUI interactable;

    public GameObject followerToSpawn;
    public Transform spawnPoint;

    public void OnPickup(Interactable interactable)
    {
        GameObject spawnedFollower = Instantiate(followerToSpawn, spawnPoint.position, spawnPoint.rotation);

        UAI_MeleeFollower melee = spawnedFollower.GetComponent<UAI_MeleeFollower>();

        if (melee != null)
        {
            melee.SetPositionToWanderAround(interactable.interactingPlayer.transform);
            //Debug.Log("1");
        }


           
    }
}
