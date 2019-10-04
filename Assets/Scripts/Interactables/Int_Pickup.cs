using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Health,
    Ammo
}

public class Int_Pickup : MonoBehaviour
{
    InteractableUI interactable;
    public bool infinite;

    
    [Header("Pickup")]
    public PickupType pickupType;

    [HideInInspector]
    public AmmoType ammoType;

    public int amount;

    public void OnPickup(Interactable interactable)
    {
        Debug.Log("Pick up");
        if(pickupType == PickupType.Health)
        {
            EC_Health health = interactable.interactingPlayer.GetComponent<EC_Health>();

            health.AddHealth(amount);
        }
        else if(pickupType == PickupType.Ammo)
        {
            EC_WeaponSystem weaponSystem = interactable.interactingPlayer.GetComponent<EC_WeaponSystem>();
            weaponSystem.AddAmmo(ammoType, amount);
        }

        if (!infinite)
        {
            Destroy(gameObject, 0.03f);
        }
    }
       

}
