using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyInteractable : Interactable
{
    [Header("Buy")]
    public float cost;
    HordeModeManager hordeModeManager;

    private void Awake()
    {
        hordeModeManager = HordeModeManager.Instance;
    }

    public override bool StartInteract(GameEntity interactingPlayer)
    {
        Debug.Log("the overriden start");
        this.interactingPlayer = interactingPlayer;

        if (hordeModeManager.DoesPlayerHaveEnoughPoints(interactingPlayer, cost))
        {
            Debug.Log("enough");

            return true;
        }
        else
        {
            Debug.Log("not enough");

            return false;
        }
    }

    public override void SucessfullyInteract()
    {
        hordeModeManager.RemovePlayerPoints(interactingPlayer, cost);
        OnSucessfullyInteract.Invoke();
    }
}
