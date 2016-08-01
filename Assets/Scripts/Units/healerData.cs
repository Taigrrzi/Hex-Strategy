using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class healerData : unitData
{

    int range;
    void Start()
    {
        maxHealth = 7;
        currentHealth = 7;
        baseAttack = 0;
        baseMoveSpeed = 1;
        range = 2;
        unitName = "Healer";
        unitDesc = "Can heal stuff :), goes well with units with lots of health";
        activeName = "Heal";
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
                unit.currentHealth=unit.maxHealth;
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

    public override void OnTurnEnd()
    {
        foreach (GameObject allyHex in GetAllyHexesInRange(1))
        {
            unitData unit = allyHex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>();
            if (unit.currentHealth<unit.maxHealth)
            {
                unit.currentHealth++;
            }
        }
    }

}

