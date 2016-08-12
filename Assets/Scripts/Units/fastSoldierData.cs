using UnityEngine;
using System.Collections;

public class fastSoldierData : unitData
{
    void Start()
    {
        maxHealth = 7;
        currentHealth = 7;
        baseAttack = 3;
        baseMoveSpeed = 2;
        unitName = "Fast Soldier";
        unitDesc = "Smaller but faster";
        StartSprite();
    }

    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_fastSoldier");
    }
}