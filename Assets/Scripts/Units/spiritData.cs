using UnityEngine;
using System.Collections;

public class spiritData : unitData
{
    void Start()
    {
        maxHealth = 5;
        currentHealth = 5;
        baseAttack = 4;
        baseMoveSpeed = 1;
        unitName = "Spirit";
        unitDesc = "Spoooky";
        cloaked = false;
        StartSprite();
    }

    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_spirit");
    }
}