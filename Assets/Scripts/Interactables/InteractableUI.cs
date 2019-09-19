using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InteractableUI : MonoBehaviour
{
    public Interactable interactable;
    //how fast does our fill rect empthy itself
    public float defillSpeed;
    public float fillSpeed;
    //value between 0 and 1
    float interactionFill;

    [Tooltip("does the fill bar reset if we stop filluing or does it defill with a certain speed")]
    public bool resetOnRelease;

    public Canvas uiHolder;
    public Image fillImage;

    public Color sucessfulColor;
    public Color failedColor;
    //Color currentTransitionColor;
    public Color normalColor;

    public Image[] imagesToTint;

    public bool hideOnSucessfulInteract;
    [Tooltip("if we want the interactabele button to scale down and up again after sucessful interaction")]
    public bool hideForASecondOnSucessfulInteract;
    public float animationPlayTime;
    float changeBackToNormalColorTime;

    bool interactionSucessful;
    public float scaleSpeed;

    //public UnityEvent OnStartInteract;
    //public UnityEvent OnHoldInteract;
    //public UnityEvent OnStopInteract;
   // public UnityEvent OnSucessfullyInteract;

    public GameEntity interactingPlayer; //the personInteracting with it 

    enum InteractionState
    {
        NoInteraction,
        Interacting,
        PlayingInteractionAnimation,
    }

    InteractionState state = InteractionState.NoInteraction;


    //the interactibles zoom up and down depending on visiblity
    enum VisibilityState
    {
        Default,
        ScalingUp,
        ScalingDown,
        ScalingDownAndUp
    }

    VisibilityState visibilityState;

    public virtual void StartInteract(GameObject goInteracting)
    {
        GameEntity interactingPlayer = goInteracting.GetComponent<GameEntity>();

        if (interactable.StartInteract(interactingPlayer))
        {
            this.interactingPlayer = interactingPlayer;

            if (state == InteractionState.NoInteraction)
            {
                state = InteractionState.Interacting;

            }


        }
        else
        {
            //shake that its not possible
        }


        //OnStartInteract.Invoke();
    }

    public void HoldInteract()
    {
        if (state == InteractionState.Interacting)
        {
            //interactedThisFrame = true;
            interactionFill += fillSpeed*Time.deltaTime;

            if (interactionFill >= 1)
            {
                interactionFill = 1;

                state = InteractionState.PlayingInteractionAnimation;
                changeBackToNormalColorTime = Time.time + animationPlayTime;

                for (int i = 0; i < imagesToTint.Length; i++)
                {
                    imagesToTint[i].color = sucessfulColor;
                }
                interactionSucessful = true;

                if (hideOnSucessfulInteract) Hide();
                else if (hideForASecondOnSucessfulInteract) HideAndShow();

            }

            fillImage.fillAmount = interactionFill;

        }

        interactable.HoldInteract(interactionFill);
        //OnHoldInteract.Invoke();
    }

    public void SucessfullyEndInteract()
    {

    }

    public void StopInteract()
    {
        if(state == InteractionState.Interacting)
        {
            state = InteractionState.PlayingInteractionAnimation;
            changeBackToNormalColorTime = Time.time + animationPlayTime;

            for (int i = 0; i < imagesToTint.Length; i++)
            {
                imagesToTint[i].color = failedColor;
            }

            interactionSucessful = false;
        }

        interactable.StopInteract();
        //OnStopInteract.Invoke();
    }

    void Update()
    {
        switch (state)
        {
            case InteractionState.NoInteraction:

                interactionFill -= defillSpeed * Time.deltaTime;
                if (interactionFill < 0) interactionFill = 0;
                fillImage.fillAmount = interactionFill;

                break;

            case InteractionState.PlayingInteractionAnimation:
                //Debug.Log("yee");
                if(Time.time> changeBackToNormalColorTime)
                {
                    state = InteractionState.NoInteraction;

                    //Debug.Log("Time.time: " + Time.time);
                    //Debug.Log("changeBackToNormalColorTime: " + changeBackToNormalColorTime);

                    for (int i = 0; i < imagesToTint.Length; i++)
                    {
                        imagesToTint[i].color = normalColor;
                    }

                    if (interactionSucessful)
                    {
                        //OnSucessfullyInteract.Invoke();
                        interactable.SucessfullyInteract();


                        Reset();
                    }
                    else
                    {
                        interactable.StopInteract();
                        //OnStopInteract.Invoke();
                    }
                }


                break;
        }

        if(visibilityState == VisibilityState.ScalingUp)
        {
            float deltaScaleSpeed = scaleSpeed*Time.deltaTime;
            uiHolder.transform.localScale += new Vector3(deltaScaleSpeed, deltaScaleSpeed, deltaScaleSpeed);
            if (uiHolder.transform.localScale.x > 0.99)
            {
                uiHolder.transform.localScale = new Vector3(1, 1, 1);
                visibilityState = VisibilityState.Default;
            }

        }
        else if(visibilityState == VisibilityState.ScalingDown)
        {
            float deltaScaleSpeed = scaleSpeed * Time.deltaTime;
            uiHolder.transform.localScale -= new Vector3(deltaScaleSpeed, deltaScaleSpeed, deltaScaleSpeed);
            if (uiHolder.transform.localScale.x < 0.01)
            {
                uiHolder.transform.localScale = new Vector3(0, 0, 0);
                //Debug.Log("backToDefault");
                visibilityState = VisibilityState.Default;
                //Debug.Log("visiblityState: " + visibilityState);
                uiHolder.enabled = false;
 
            }
        }
        else if (visibilityState == VisibilityState.ScalingDownAndUp)
        {
            float deltaScaleSpeed = scaleSpeed * Time.deltaTime;
            uiHolder.transform.localScale -= new Vector3(deltaScaleSpeed, deltaScaleSpeed, deltaScaleSpeed);
            if (uiHolder.transform.localScale.x < 0.01)
            {
                uiHolder.transform.localScale = new Vector3(0, 0, 0);
                //Debug.Log("backToDefault");
                visibilityState = VisibilityState.Default;
                //Debug.Log("visiblityState: " + visibilityState);
                uiHolder.enabled = false;

                Show();
            }
        }

    }

    void Reset()
    {
        interactionFill = 0;
        fillImage.fillAmount = interactionFill;
    }



    public void Show()
    {
        visibilityState = VisibilityState.ScalingUp;
        uiHolder.enabled = true;
        //uiHolder.enabled = true;
    }

    public void Hide()
    {
        visibilityState = VisibilityState.ScalingDown;
        //uiHolder.enabled = false;

    }

    //cool effect upon sucessfully interacting
    public void HideAndShow()
    {
        visibilityState = VisibilityState.ScalingDownAndUp;

    }
}
