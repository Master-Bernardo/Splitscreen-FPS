using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Int_Pickup : MonoBehaviour
{
    Interactable interactable;

    public enum PickupType
    {
        Health,
        Ammo
    }
    [Header("Pickup")]
    public PickupType pickupType;

    [Tooltip("only if we choosen ammo")]
    public AmmoType ammoType;

    public int amount;

    public void OnPickup(Interactable interactable)
    {
        if(pickupType == PickupType.Health)
        {
            EC_Health health = interactable.goInteracting.GetComponent<EC_Health>();

            health.AddHealth(amount);
        }
        else if(pickupType == PickupType.Ammo)
        {
            WeaponSystem weaponSystem = interactable.goInteracting.GetComponent<WeaponSystem>();
            weaponSystem.AddAmmo(ammoType, amount);
        }

        Destroy(gameObject,0.03f);
    }
       

}
