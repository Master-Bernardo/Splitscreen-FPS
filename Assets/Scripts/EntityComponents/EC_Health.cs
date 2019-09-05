using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EC_Health : EntityComponent
{
    public float currentHealth;
    public float maxHealth;
    public Image healthBarFill; // for later
    public bool changeColorOnDamage;
    bool colorChanged;
    public Material damageMaterial;
    Material[] normalMaterials;
    public MeshRenderer[] renderersToTint;
    public float changeColorTime = 0.2f;
    float nextGoBackToNormalColorTime;

    public DeathEffect deathEffect;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        //Unit unit = entity.GetComponent<Unit>();
        //if(unit!=null)maxHealth = unit.unitData.healthPoints;
        currentHealth = maxHealth;


        normalMaterials = new Material[renderersToTint.Length];

        for (int i = 0; i < renderersToTint.Length; i++)
        {
            normalMaterials[i] = renderersToTint[i].material;
        }
    }

    public override void OnTakeDamage(float damage)
    {
        base.OnTakeDamage(damage);

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (deathEffect != null) deathEffect.OnDie();
            myEntity.Die();
        }

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }

        if (changeColorOnDamage)
        {
            nextGoBackToNormalColorTime = Time.time + changeColorTime;

            colorChanged = true;
            for (int i = 0; i < renderersToTint.Length; i++)
            {
                renderersToTint[i].material = damageMaterial;
            }
        }
    }

    public override void OnTakeDamage(float damage, Vector3 force)
    {
        base.OnTakeDamage(damage);

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (deathEffect != null) deathEffect.OnDie(force);
            myEntity.Die();

        }

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }

        if (changeColorOnDamage)
        {
            nextGoBackToNormalColorTime = Time.time + changeColorTime;

            colorChanged = true;
            for (int i = 0; i < renderersToTint.Length; i++)
            {
                renderersToTint[i].material = damageMaterial;
            }
        }
    }

    public override void UpdateComponent()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnTakeDamage(500);
        }

        base.UpdateComponent();

        if (colorChanged)
        {
            if(Time.time> nextGoBackToNormalColorTime)
            {
                colorChanged = false;

                for (int i = 0; i < renderersToTint.Length; i++)
                {
                    renderersToTint[i].material = normalMaterials[i];

                    // You can re-use this block between calls rather than constructing a new one each time.
                    //var block = new MaterialPropertyBlock();

                    // You can look up the property by ID instead of the string to be more efficient.
                    //block.SetColor("_BaseColor", normalColor[i]);

                    // You can cache a reference to the renderer to avoid searching for it.
                    //renderersToTint[i].SetPropertyBlock(block);
                }
            }
        }
    }

    public void AddHealth(int value)
    {
        currentHealth += value;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }
}
