using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BuyInteractable : Interactable
{
    [Header("Buy")]
    public float cost;
    public TextMeshProUGUI[] costText;
    PlayerRessources playerRessources;

    private void Awake()
    {
        playerRessources = PlayerRessources.Instance;

        for (int i = 0; i < costText.Length; i++)
        {
            costText[i].gameObject.SetActive(true);
            costText[i].text = cost.ToString();
        }
        
    }

    public override bool StartInteract(GameEntity interactingPlayer)
    {
        //Debug.Log("the overriden start");
        this.interactingPlayer = interactingPlayer;
        // Debug.Log("Horde manager2: " + hordeModeManager);
        if (playerRessources.DoesPlayerHaveEnoughPoints((interactingPlayer as E_PlayerEntity).playerID, cost))
        {
           // Debug.Log("enough");

            return true;
        }
        else
        {
            //Debug.Log("not enough");

            return false;
        }
    }

    public override void SucessfullyInteract()
    {
        playerRessources.RemovePlayerPoints((interactingPlayer as E_PlayerEntity).playerID, cost);
        OnSucessfullyInteract.Invoke();
    }
}
