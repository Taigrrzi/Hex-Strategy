using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class turretData : unitData
{

    int rangedDamage;
    int range;
    void Start()
    {
        maxHealth = 10;
        currentHealth = 10;
        baseAttack = 0;
        baseMoveSpeed = 0;
        rangedDamage = 3;
        range = 2;
        unitName = "Turret";
        unitDesc = "Can't Move, but can shoot";
        activeName = "Fire";
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
                hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnTakingDamage(rangedDamage, true,gameObject);
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
            validHexes = GetEnemyHexesInRange(range);
            mapControl.globalMap.HighlightHash(validHexes, Color.red);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }

}
