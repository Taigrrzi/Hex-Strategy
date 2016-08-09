using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class manipulatorData : unitData
{

    int range;
    GameObject servant;
    void Start()
    {
        maxHealth = 6;
        currentHealth = 6;
        baseAttack = 0;
        baseMoveSpeed = 1;
        range = 2;
        unitName = "Manipulator";
        unitDesc = "Silver Tongued";
        activeName = "Order";
    }

    public override void OnHexTouchedSelected(GameObject hexTouched)
    {
        base.OnHexTouchedSelected(hexTouched);
        if (mode == 3)
        {
            if (servant==null)
            {
                servant = hexTouched.GetComponent<hexData>().occupyingObject;
                validHexes = mapControl.globalMap.SelectInRangeUnoccupied(hexTouched, 1, true);
                mapControl.globalMap.ClearHighlights();
                mapControl.globalMap.HighlightHash(validHexes, Color.green);
            } else
            {
                if (mapControl.globalMap.currentActionPoints > 0)
                {
                    OnActiveUse();
                    servant.GetComponent<unitData>().MoveToHex(hexTouched);
                    LoseFocus();
                    mapControl.globalMap.currentActionPoints--;
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
            validHexes = GetAllyHexesInRange(range);
            validHexes.Remove(occupyingHex);
            HashSet<GameObject> enemyHexes = GetEnemyHexesInRange(range);
            validHexes.UnionWith(enemyHexes);
            mapControl.globalMap.HighlightHash(validHexes, Color.blue);
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