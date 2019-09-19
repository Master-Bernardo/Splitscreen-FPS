using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_HordePointsGiver : EntityComponent
{
    public float pointsPerDamage;
    public float pointsPerDeath;

    HordeModeManager hordeModeManager;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        hordeModeManager = HordeModeManager.Instance;
    }

    public override void OnTakeDamage(DamageInfo damageInfo)
    {
        hordeModeManager.AddPlayerPoints(damageInfo.damageGiver, damageInfo.damage * pointsPerDamage);
    }

    public override void OnDie(GameEntity killer)
    {
        hordeModeManager.AddPlayerPoints(killer, pointsPerDeath);
    }
}
