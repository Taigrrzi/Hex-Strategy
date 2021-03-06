﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class visageData : unitData
{

    void Start()
    {
        maxHealth = 4;
        currentHealth = 4;
        baseAttack = 0;
        baseMoveSpeed = 1;
        unitName = "Visage";
        unitDesc = "Has 2 shadows";
        StartSprite();
    }

    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_visage");
    }
    public override void OnDeath()
    {
        occupyingHex.GetComponent<hexData>().Empty();
        GameObject newUnit = (GameObject)Instantiate(Resources.Load("Unit"));
        newUnit.AddComponent<spiritData>();
        newUnit.GetComponent<unitData>().team = team;
        mapControl.globalMap.CreateOnHex(newUnit, occupyingHex);
        if (team==0)
        {
            mapControl.globalMap.Team0Units.Add(newUnit);
            newUnit.name = "Unit: Spirit";
            newUnit.GetComponent<SpriteRenderer>().color = Color.green;
        } else
        {
            mapControl.globalMap.Team1Units.Add(newUnit);
            newUnit.name = "Enemy: Spirit";
            newUnit.GetComponent<SpriteRenderer>().color = Color.red;
        }
        Destroy(gameObject);
    }

}


