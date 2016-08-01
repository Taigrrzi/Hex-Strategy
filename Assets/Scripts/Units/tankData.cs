using UnityEngine;
using System.Collections;

public class tankData : unitData
{
    void Start()
    {
        maxHealth = 20;
        currentHealth = 20;
        baseAttack = 1;
        baseMoveSpeed = 1;
        unitName = "Tank";
        unitDesc = "Has a lot of health, but not much else";
    }
}
