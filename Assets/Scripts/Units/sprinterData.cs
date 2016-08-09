using UnityEngine;
using System.Collections;

public class sprinterData : unitData
{
    void Start()
    {
        maxHealth = 7;
        currentHealth = 7;
        baseAttack = 2;
        baseMoveSpeed = 3;
        unitName = "Sprinter";
        unitDesc = "Has the runs";
    }
}