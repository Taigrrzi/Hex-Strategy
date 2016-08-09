using UnityEngine;
using System.Collections;

public class mortarData : unitData
{
    int range = 2;
    int rangedDamage = 3;
    void Start()
    {
        maxHealth = 7;
        currentHealth = 7;
        baseAttack = 0;
        baseMoveSpeed = 0;
        unitName = "Mortar";
        unitDesc = "Fires big long range shells";
        activeName = "Bombard";
    }

    public override void OnHexTouchedSelected(GameObject hexTouched)
    {
        base.OnHexTouchedSelected(hexTouched);
        if (mode == 3)
        {
            if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 1)
            {
                OnUncloaking();
                OnActiveUse();
                foreach (GameObject bombedHex in mapControl.globalMap.SelectInRangeOccupied(hexTouched,1,false))
                {
                    if (bombedHex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().team!=team)
                    {
                        bombedHex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnTakingDamage(rangedDamage,false,gameObject);
                    }
                }

                LoseFocus();
                mapControl.globalMap.ClearHighlights();
                mapControl.globalMap.currentActionPoints-=2;
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
            validHexes = mapControl.globalMap.SelectInRange(occupyingHex,range,false);
            mapControl.globalMap.HighlightHash(validHexes, Color.red);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }
}