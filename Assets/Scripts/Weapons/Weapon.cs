using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Base Weapon")]
    public string weaponName;
    public int teamID;
    protected EC_WeaponSystem weaponSystem;
    public GameEntity weaponWieldingEntity;
    public float damage; //could be expanded later to diffeent damages  
    public bool automaticTrigger;
    public bool usesAimingLine;
    //public AimingLine aimingLine;

    public void SetUp(EC_WeaponSystem weaponSystem)
    {
        this.weaponSystem = weaponSystem;
    }

    public virtual void OnWeaponSelect(GameEntity selectingEntity)
    {
        weaponWieldingEntity = selectingEntity;
    }

    public virtual void OnWeaponDeselect()
    {

    }

    public virtual void HandleWeaponKey(int weaponKey)
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
