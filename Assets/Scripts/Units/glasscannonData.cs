using UnityEngine;
using System.Collections;

public class glasscannonData : unitData
{
    void Start()
    {
        maxHealth = 4;
        currentHealth = 4;
        baseAttack = 6;
        baseMoveSpeed = 1;
        unitName = "Glass Cannon";
        unitDesc = "A cannon made of glass";
    }
}
