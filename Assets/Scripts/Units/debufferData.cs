using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class debufferData : unitData
{

    int range;
    void Start()
    {
        maxHealth = 6;
        currentHealth = 6;
        baseAttack = 2;
        baseMoveSpeed = 1;
        range = 1;
        unitName = "Librarian";
        unitDesc = "Keeps the peace";
        activeName = "Silence";
    }

    public override void OnHexTouchedSelected(GameObject hexTouched)
    {
        base.OnHexTouchedSelected(hexTouched);
        if (mode == 3)
        {
            if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 0)
            {
                OnActiveUse();
                hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().buffAttack=0;
                hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().maxHealth -= hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().buffHealth;
                hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().buffHealth = 0;
                hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().buffArmor = 0;
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
            mapControl.globalMap.HighlightHash(validHexes, Color.blue);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }

}