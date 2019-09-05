using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Int_WeaponPickup : MonoBehaviour
{
    public Transform weaponHolder;
    public Weapon pickUpWeapon;

    public void OnPickup(Interactable interactable)
    {
        Weapon currentWeapon = interactable.goInteracting.GetComponent<WeaponSystem>().SwapWeapon(pickUpWeapon);

        if(currentWeapon != null)
        {
            pickUpWeapon = currentWeapon;

            currentWeapon.transform.SetParent(weaponHolder);
            currentWeapon.transform.localPosition = new Vector3(0, 0, 0);
            currentWeapon.transform.forward = weaponHolder.transform.forward;
        }
        else
        {
            Destroy(gameObject, 0.03f);
        }
    }
}
