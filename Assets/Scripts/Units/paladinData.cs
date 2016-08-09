using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class paladinData : unitData
{

    void Start()
    {
        maxHealth = 7;
        currentHealth = 7;
        baseAttack = 3;
        baseMoveSpeed = 1;
        unitName = "Paladin";
        unitDesc = "Protected by God";
    }

    public override void OnGameStart()
    {
        OnShieldGained();
        base.OnGameStart();
    }
}
