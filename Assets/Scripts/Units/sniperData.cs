﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sniperData : unitData {

    int rangedDamage;
    int range;
    void Start()
    {
        maxHealth = 4;
        currentHealth = 4;
        baseAttack = 1;
        baseMoveSpeed = 1;
        rangedDamage = 4;
        range = 2;
        unitName = "Sniper";
        unitDesc = "Squishy and poor at short range, but excels at long range combat";
        activeName = "Snipe";
        uncloakedSprite = Resources.Load<Sprite>("devUnit_sniper");
        StartSprite();
    }

    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_sniper");
    }

    public override void OnHexTouchedSelected (GameObject hexTouched) {
        base.OnHexTouchedSelected(hexTouched);
        if (mode==3)
        {
            if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 0)
            {
                OnUncloaking();
                OnActiveUse();
                hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnTakingDamage(rangedDamage,true,gameObject);
                LoseFocus();
                mapControl.globalMap.currentActionPoints--;
            }
        }
    }

    public override void OnActivePressed()
    {
        mapControl.globalMap.ClearHighlights();
        if (mode == 3)
        {
            mode = 0;
        }
        else
        {
            mode = 3;
            validHexes =  GetEnemyHexesInRange(range);
            mapControl.globalMap.HighlightHash(validHexes, Color.red);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }

}
