using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Int_BuyWeapon : Interactable
{
    [Header("Buy weapon")]
    public float cost;
    HordeModeManager hordeModeManager;

    private void Awake()
    {
        hordeModeManager = HordeModeManager.Instance;
    }

    public override void StartInteract(GameObject goInteracting)
    {
        GameEntity entity = goInteracting.GetComponent<GameEntity>();
        if(hordeModeManager.DoesPlayerHaveEnoughPoints(entity, cost))
        {
            base.StartInteract(goInteracting);
        }
        else
        {
            //shake the interactable and make this sound that you cant use it
        }
    }

    //public override void 
}
