using UnityEngine;
using System.Collections;

public class droneData : unitData
{
    void Start()
    {
        maxHealth = 1;
        currentHealth = 1;
        baseAttack = 1;
        baseMoveSpeed = 2;
        unitName = "Drone";
        unitDesc = "Follows Orders";
        StartSprite();  
    }

    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_drone");
    }
}