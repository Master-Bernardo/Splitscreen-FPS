using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BuyInteractable : Interactable
{
    [Header("Buy")]
    public float cost;
    public TextMeshProUGUI[] costText;
    HordeModeManager hordeModeManager;

    private void Awake()
    {
        hordeModeManager = HordeModeManager.Instance;
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

        if (hordeModeManager.DoesPlayerHaveEnoughPoints(interactingPlayer, cost))
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
        hordeModeManager.RemovePlayerPoints(interactingPlayer, cost);
        OnSucessfullyInteract.Invoke();
    }
}
