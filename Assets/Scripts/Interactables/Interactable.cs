using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//communicates with the interactable UI
public class Interactable : MonoBehaviour
{
    public bool canBeUsedByMultiplePlayersAtOnce = true;

    public UnityEvent OnStartInteract;
    public UnityEvent OnHoldInteract;
    public UnityEvent OnStopInteract;
    public UnityEvent OnSucessfullyInteract;

    public GameEntity interactingPlayer;

    //lso checks if its possible to start interaction
    public virtual bool StartInteract(GameEntity interactingPlayer)
    {
        Debug.Log("the base start");

        this.interactingPlayer = interactingPlayer;
        return true;
    }

    //completion should be between 0 and 1
    public void HoldInteract(float completion)
    {
        OnHoldInteract.Invoke();
    }

    public void StopInteract()
    {
        OnStopInteract.Invoke();
    }

    public virtual void SucessfullyInteract()
    {
        OnSucessfullyInteract.Invoke();
    }

}
