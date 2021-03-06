﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scoutData : unitData
{

    int rangedDamage;
    int range;
    void Start()
    {
        maxHealth = 5;
        currentHealth = 5;
        baseAttack = 1;
        baseMoveSpeed = 2;
        rangedDamage = 1;
        range = 2;
        unitName = "Scout";
        unitDesc = "Squishy, fast moving, designed to uncloak units then escape";
        activeName = "Poke";
        StartSprite();
    }

    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_scout");
    }

    public override void OnHexTouchedSelected(GameObject hexTouched)
    {
        base.OnHexTouchedSelected(hexTouched);
        if (mode == 3)
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


    public override void OnMoveEnd()
    {
        foreach (GameObject ajHex in mapControl.globalMap.SelectInRangeOccupied(occupyingHex,1,true))
        {
            if (ajHex.GetComponent<hexData>().occupied)
            {
                if (ajHex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().team!=team)
                {
                    if (ajHex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().cloaked) {
                        ajHex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnUncloaking();
                    }
                }
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
            validHexes = GetEnemyHexesInRange(range);
            mapControl.globalMap.HighlightHash(validHexes, Color.red);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }

}
