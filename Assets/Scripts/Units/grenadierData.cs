using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class grenadierData : unitData
{

    int rangedDamage;
    int range;
    void Start()
    {
        maxHealth = 8;
        currentHealth = 8;
        baseAttack = 1;
        baseMoveSpeed = 1;
        rangedDamage = 2;
        range = 1;
        unitName = "Hillbilly";
        unitDesc = "Shoots foreigners";
        activeName = "Shoot Shotgun";
        StartSprite();
    }

    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_grenadier");
    }
    public override void OnHexTouchedSelected(GameObject hexTouched)
    {
        base.OnHexTouchedSelected(hexTouched);
        if (mode == 3)
        {
            if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 0)
            {
                OnActiveUse();
                hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnTakingDamage(rangedDamage,true,gameObject);
                HashSet<GameObject> surroundingOccupied = mapControl.globalMap.SelectInRangeOccupied(hexTouched, 1,true);
                StartExplosion();
                foreach (GameObject surroundingThing in surroundingOccupied)
                {
                    if (surroundingThing.tag=="Unit"&&surroundingThing.GetComponent<unitData>().team!=team)
                    {
                        surroundingThing.GetComponent<unitData>().OnTakingDamage(rangedDamage,false,gameObject);
                    }
                }
                EndExplosion();
                LoseFocus();
                mapControl.globalMap.currentActionPoints--;
            }
        }
    }

    public override void OnActivePressed()
    {
        if (mode == 3)
        {
            mode = 0;
            mapControl.globalMap.ClearHighlights();
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

