using UnityEngine;
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
    }

    public override void OnHexTouchedSelected(GameObject hexTouched)
    {
        base.OnHexTouchedSelected(hexTouched);
        if (mode == 3)
        {
            if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 0)
            {
                OnActiveUse();
                hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnTakingDamage(rangedDamage);
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
