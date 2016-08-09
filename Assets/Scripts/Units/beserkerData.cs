using UnityEngine;
using System.Collections;

public class beserkerData : unitData
{
    int aoeDamage;
    void Start()
    {
        maxHealth = 8;
        currentHealth = 8;
        baseAttack = 0;
        aoeDamage = 3;
        baseMoveSpeed = 1;
        unitName = "Troll";
        unitDesc = "AoE";
    }

    public override void OnHexTouchedSelected(GameObject hexTouched)
    {
        base.OnHexTouchedSelected(hexTouched);
        if (mode == 3)
        {
            if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 0)
            {
                OnActiveUse();
                foreach (GameObject ajacentUnitHex in validHexes)
                {
                    ajacentUnitHex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnTakingDamage(aoeDamage,true,gameObject);
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
            validHexes = mapControl.globalMap.SelectInRange(occupyingHex,1,true);
            mapControl.globalMap.HighlightHash(validHexes, Color.red);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }
}