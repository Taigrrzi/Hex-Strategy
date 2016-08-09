using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class shamanData : unitData
{

    int range;
    int rangedHeal;
    int rangedDamage;
    void Start()
    {
        maxHealth = 7;
        currentHealth = 7;
        baseAttack = 0;
        baseMoveSpeed = 1;
        range = 2;
        rangedDamage = 2;
        rangedHeal = 3;
        unitName = "Shaman";
        unitDesc = "Heals and damages";
        activeName = "Use Voodoo";
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
                if (unit.team == team)
                {
                    unit.OnHealing(rangedHeal);
                } else
                {
                    unit.OnTakingDamage(rangedDamage,true,gameObject);
                }
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
            HashSet<GameObject> enemyHexes = GetEnemyHexesInRange(range);
            mapControl.globalMap.HighlightHash(validHexes, Color.green);
            mapControl.globalMap.HighlightHash(enemyHexes, Color.red);
            validHexes.UnionWith(enemyHexes);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        foreach (GameObject allyHex in GetAllyHexesInRange(1))
        {
            unitData unit = allyHex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>();
            unit.OnHealing(1);
        }
    }

}
