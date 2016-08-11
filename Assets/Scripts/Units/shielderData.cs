using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class shielderData : unitData
{

    public int range;
    public int armorAmount;
    void Start()
    {
        range = 1;
        maxHealth = 8;
        currentHealth = 8;
        baseAttack = 2;
        baseMoveSpeed = 1;
        armorAmount = 3;
        unitName = "Shielder";
        unitDesc = "Can give stuff divine shield";
        activeName = "Shield";
        StartSprite();
    }

    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_shielder");
    }
    public override void OnHexTouchedSelected(GameObject hexTouched)
    {
        base.OnHexTouchedSelected(hexTouched);
        if (mode == 3)
        {
            if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 0)
            {
                OnActiveUse();
                unitData unit = hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>();
                unit.OnShieldGained();
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
            validHexes = GetAllyHexesInRange(range);
            mapControl.globalMap.HighlightHash(validHexes, Color.green);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }
}

