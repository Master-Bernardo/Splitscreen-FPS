using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EC_Health : EntityComponent
{
    public float currentHealth;
    public float maxHealth;
    public RectTransform healthBarFill; // for later
    public bool changeColorOnDamage;
    bool colorChanged;
    public Color damageColor;
    Color[] normalColor;
    public MeshRenderer[] renderersToTint;
    public float changeColorTIme;
    float nextGoBackToNormalColorTIme;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        //Unit unit = entity.GetComponent<Unit>();
        //if(unit!=null)maxHealth = unit.unitData.healthPoints;
        currentHealth = maxHealth;


        normalColor = new Color[renderersToTint.Length];

        for (int i = 0; i < renderersToTint.Length; i++)
        {
            normalColor[i] = renderersToTint[i].material.GetColor("_BaseColor");
        }
    }

    public override void OnTakeDamage(float damage)
    {
        base.OnTakeDamage(damage);

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            myEntity.Die();
        }

        if (changeColorOnDamage)
        {
            nextGoBackToNormalColorTIme = Time.time + changeColorTIme;

            colorChanged = true;
            for (int i = 0; i < renderersToTint.Length; i++)
            {
                // You can re-use this block between calls rather than constructing a new one each time.
                var block = new MaterialPropertyBlock();

                // You can look up the property by ID instead of the string to be more efficient.
                block.SetColor("_BaseColor", damageColor);

                // You can cache a reference to the renderer to avoid searching for it.
                renderersToTint[i].SetPropertyBlock(block);
            }
        }
    }

    public override void UpdateComponent()
    {
        base.UpdateComponent();

        if (colorChanged)
        {
            if(Time.time> nextGoBackToNormalColorTIme)
            {
                colorChanged = false;

                for (int i = 0; i < renderersToTint.Length; i++)
                {
                    // You can re-use this block between calls rather than constructing a new one each time.
                    var block = new MaterialPropertyBlock();

                    // You can look up the property by ID instead of the string to be more efficient.
                    block.SetColor("_BaseColor", normalColor[i]);

                    // You can cache a reference to the renderer to avoid searching for it.
                    renderersToTint[i].SetPropertyBlock(block);
                }
            }
        }
    }
}
