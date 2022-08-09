using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthExporter : Health
{
    [SerializeField] private Health importer;
    [SerializeField] private float damageMultiply;

    public override void AddHealth(float health)
    {
        if (importer != null) importer.AddHealth(health);
    }

    public override void SetMaxHealth(float maxHealth)
    {
        if (importer != null) importer.SetMaxHealth(maxHealth);
    }

    public override void GetDamage(float damage,Transform source = null)
    {
        if(importer != null) importer.GetDamage(damage * damageMultiply,source);
    }

}
