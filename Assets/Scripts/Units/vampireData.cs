using UnityEngine;
using System.Collections;

public class vampireData : unitData
{
    void Start()
    {
        maxHealth = 8;
        currentHealth = 8;
        baseAttack = 2;
        baseMoveSpeed = 1;
        unitName = "Vampire";
        unitDesc = "Sucks Blood";
    }

    public override void OnAttacking()
    {
        base.OnAttacking();
        OnHealing(currentAttack);
    }
}
