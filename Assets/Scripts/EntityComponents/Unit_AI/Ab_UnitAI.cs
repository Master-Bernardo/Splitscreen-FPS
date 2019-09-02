using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* todo not yet imlemented*/
public class Ab_UnitAI : EntityComponent
{
    //switches between different behaviours 

    //public Behaviour[] behaviours;
    [SerializeField]
    protected Behaviour currentBehaviour;
 

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        /*for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].SetUp(this);
        }*/
    }

    public override void UpdateComponent()
    {
        //1check if we need to change the current Bahaviour
        CheckCurrentBehaviour();
        //2. update bahaviour
        if(currentBehaviour != null)currentBehaviour.UpdateBehaviour();
    }

    public virtual void CheckCurrentBehaviour()
    {

    }

    protected void SetCurrentBehaviour(Behaviour newBehaviour)
    {
        if (currentBehaviour != newBehaviour)
        {
            if(currentBehaviour!=null)currentBehaviour.OnBehaviourExit();
            currentBehaviour = newBehaviour;
            if(currentBehaviour!=null)currentBehaviour.OnBehaviourEnter();
        }
    }

    public override void OnDie()
    {
        
    }
}


