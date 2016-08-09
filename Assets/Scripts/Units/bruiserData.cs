using UnityEngine;
using System.Collections;

public class bruiserData : unitData
{
    void Start()
    {
        maxHealth = 12;
        currentHealth = 12;
        baseAttack = 2;
        baseMoveSpeed = 1;
        unitName = "Brawler";
        unitDesc = "Stat Adjustment";
    }
}