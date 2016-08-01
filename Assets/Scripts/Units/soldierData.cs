using UnityEngine;
using System.Collections;

public class soldierData : unitData
{
    void Start()
    {
        maxHealth = 10;
        currentHealth = 10;
        baseAttack = 3;
        baseMoveSpeed = 1;
        unitName = "Soldier";
        unitDesc = "All rounder, solid, boring";
    }
}