using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Int_WeaponPickup : MonoBehaviour
{
    public Transform weaponHolder;
    public Weapon pickUpWeapon;
  
    [Tooltip("if this is true, we can buy or pickup as many of this weapons as we like, they will be cloned over and over again, our current weapon will be dropped")]
    public bool infinite = false;
    //[Tooltip("only affects this pickup if infinite is true - hides the original Weapon model, we can intead use a fram or another model or an outline like in cod zombies")]
    //public bool visible = true; - not needed
    [Tooltip("when we drop our current weapon while getting a new, this container gets spawned around it so we can pick it up again later")]
    public GameObject dropWeaponPickupContainer;

    public void OnPickup(Interactable interactable)
    {
        //if this is only one weapon, then the pickup will now contain the players weapon which he swapped
        if (!infinite)
        {
            Weapon playersCurrentWeapon = interactable.interactingPlayer.GetComponent<EC_PlayerWeaponSystem>().SwapWeapon(pickUpWeapon);

            if (playersCurrentWeapon != null)
            {
                pickUpWeapon = playersCurrentWeapon;

                playersCurrentWeapon.transform.SetParent(weaponHolder);
                playersCurrentWeapon.transform.localPosition = new Vector3(0, 0, 0);
                playersCurrentWeapon.transform.forward = weaponHolder.transform.forward;
                //if (!visible) playersCurrentWeapon.gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject, 0.03f);
            }
        }
        else
        {
            //make a sepaerate container class for this - also instantate this container when we drop a weapon - call it DRopWeaponPikcupContainer
            GameObject clone = Instantiate(pickUpWeapon.gameObject);
            Weapon playersCurrentWeapon = interactable.interactingPlayer.GetComponent<EC_PlayerWeaponSystem>().SwapWeapon(clone.GetComponent<Weapon>());

            if (playersCurrentWeapon != null)
            {
                Int_WeaponPickup newPickup = Instantiate(dropWeaponPickupContainer).GetComponent<Int_WeaponPickup>();
                newPickup.transform.position = interactable.interactingPlayer.transform.position + new Vector3(0,1f,0);
                newPickup.pickUpWeapon = playersCurrentWeapon;
                playersCurrentWeapon.transform.SetParent(newPickup.weaponHolder);
                playersCurrentWeapon.transform.localPosition = new Vector3(0, 0, 0);
                playersCurrentWeapon.transform.forward = newPickup.weaponHolder.transform.forward;
            }
        }
        
    }
}
