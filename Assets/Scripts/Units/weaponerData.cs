using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class weaponerData : unitData
{

    public int range;
    void Start()
    {
        range = 1;
        maxHealth = 6;
        currentHealth = 6;
        baseAttack = 2;
        baseMoveSpeed = 1;
        unitName = "Swordsmith";
        unitDesc = "Decent stats, and can give stuff attack";
        activeName = "Give Sword";
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
                unit.buffAttack += 1;
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
            validHexes = GetAllyHexesInRange(range);
            mapControl.globalMap.HighlightHash(validHexes, Color.green);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }
}