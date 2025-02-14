﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_HordePointsGiver : EntityComponent
{
    public float pointsPerDamage;
    public float pointsPerDeath;

    PlayerRessources playerRessources;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        playerRessources = PlayerRessources.Instance;
    }

    public override void OnTakeDamage(DamageInfo damageInfo)
    {
        if(damageInfo.damageGiver is E_PlayerEntity)
        {
            
                playerRessources.AddPlayerPoints((damageInfo.damageGiver as E_PlayerEntity).playerID, damageInfo.damage * pointsPerDamage);
        }
        
    }

    public override void OnDie(GameEntity killer)
    {
        if (killer != null)
        {
            if(killer.tag == "Player")
            {
                playerRessources.AddPlayerPoints((killer as E_PlayerEntity).playerID, pointsPerDeath);
            }
        }
    }
}
