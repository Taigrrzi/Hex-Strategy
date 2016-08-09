using UnityEngine;
using System.Collections;

public class murdererData : unitData
{
    void Start()
    {
        maxHealth = 14;
        currentHealth = 14;
        baseAttack = 1;
        baseMoveSpeed = 1;
        unitName = "Psycho";
        unitDesc = "Getting crazier";
    }

    public override void OnKilling()
    {
        baseAttack++;
    }
}