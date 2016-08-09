using UnityEngine;
using System.Collections;

public class demonData : unitData
{
    void Start()
    {
        maxHealth = 7;
        currentHealth = 7;
        baseAttack = 3;
        baseMoveSpeed = 1;
        unitName = "Demon";
        unitDesc = "Sucks Souls";
    }

    public override void OnKilling()
    {
        OnHealing(maxHealth-currentHealth);
    }
}