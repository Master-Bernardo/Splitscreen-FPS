using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("------------Base Weapon------------")]
    public string weaponName;
    public int teamID;
    protected EC_HumanWeaponSystem weaponSystem;
    public GameEntity weaponWieldingEntity;
    public float damage; //could be expanded later to diffeent damages  
    public bool automaticTrigger;
    public bool usesAimingLine;
    [Tooltip("the animation which is right for this weapon - 1 is sword, 2 is rifle, 3 is pistol")]
    public int stanceAnimationTypeID;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip selectWeaponSound;
    public AudioClip deselectWeaponSound;



    public void SetUp(EC_HumanWeaponSystem weaponSystem, int teamID)
    {
        this.weaponSystem = weaponSystem;
        this.teamID = teamID;
    }

    public virtual void OnWeaponSelect(GameEntity selectingEntity)
    {
        weaponWieldingEntity = selectingEntity;
        if (selectWeaponSound)
        {
            audioSource.clip = selectWeaponSound;
            audioSource.Play();
        }
    }

    public virtual void OnWeaponDeselect()
    {
        if (deselectWeaponSound)
        {
            audioSource.clip = deselectWeaponSound;
            audioSource.Play();
        }
    }

    public virtual void HandleWeaponKeyDown(int weaponKey)
    {

    }

    public virtual void HandleWeaponKeyHold(int weaponKey)
    {

    }

    public virtual void HandleWeaponKeyUp(int weaponKey)
    {

    }

    /*#region for aiming
    public virtual void ShowAimingLine(int playerUILayer)
    {
        if (Settings.Instance.showAimingLine)
        {
            if (usesAimingLine)
            {
                aimingLine.ChangeLayer(playerUILayer);
                aimingLine.gameObject.SetActive(true);
            }
        }
    }

    public virtual void UpdateAimingLine()
    {

    }

    public virtual void HideAimingLine()
    {
        if(usesAimingLine) aimingLine.gameObject.SetActive(false);
    }
    #endregion*/

    /*public virtual void HandleLMBHold()
    {

    }*/

    /*public virtual void HandleRMBHold()
    {

    }*/

}
