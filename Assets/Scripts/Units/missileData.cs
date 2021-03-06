﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class missileData : unitData
{

    int explosionDamage;
    void Start()
    {
        maxHealth = 4;
        currentHealth = 4;
        baseAttack = 0;
        baseMoveSpeed = 2;
        explosionDamage = 8;
        unitName = "Missile";
        unitDesc = "Can move fast as blow up";
        activeName = "Explode";
        StartSprite();
    }


    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_missile");
    }

    public override void OnActivePressed()
    {
        if (mapControl.globalMap.currentActionPoints > 0)
        {
            mapControl.globalMap.currentActionPoints--;
            OnDeath();
        }
    }

    public override void OnDeath()
    {
        HashSet<GameObject> ajacentUnits = GetEnemyHexesInRange(1);
        StartExplosion();
            foreach (GameObject ajacentUnit in ajacentUnits)
            {
                ajacentUnit.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnTakingDamage(explosionDamage,false,gameObject);
            }
        EndExplosion();
        occupyingHex.GetComponent<hexData>().Empty();
        base.OnDeath();
    }

}

