using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class investigatorData : unitData
{

    int range;
    void Start()
    {
        maxHealth = 8;
        currentHealth = 8;
        baseAttack = 2;
        baseMoveSpeed = 1;
        range = 3;
        unitName = "Investigator";
        unitDesc = "Figures Stuff Out";
        activeName = "Uncloak";
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
                hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnUncloaking();
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
            HashSet<GameObject> tempHexes= new HashSet<GameObject>();
            foreach (GameObject validHex in validHexes)
            {
                if (validHex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().cloaked)
                {
                    tempHexes.Add(validHex);
                }
            }
            validHexes = tempHexes;
            mapControl.globalMap.HighlightHash(validHexes, Color.blue);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }

}
