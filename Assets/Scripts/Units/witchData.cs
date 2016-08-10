using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class witchData : unitData
{

    List<GameObject> markedTargets;
    int range;
    void Start()
    {
        markedTargets = new List<GameObject>();
        maxHealth = 7;
        currentHealth = 7;
        baseAttack = 2;
        baseMoveSpeed = 1;
        range = 2;
        unitName = "Gypsy";
        unitDesc = "Don't get cursed";
        activeName = "Curse";
    }

    public override void OnHexTouchedSelected(GameObject hexTouched)
    {
        base.OnHexTouchedSelected(hexTouched);
        if (mode == 3)
        {
            if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 0&& !markedTargets.Contains(hexTouched.GetComponent<hexData>().occupyingObject))
            {
                OnUncloaking();
                OnActiveUse();
                hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().buffArmor--;
                markedTargets.Add(hexTouched.GetComponent<hexData>().occupyingObject);
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
            mapControl.globalMap.HighlightHash(validHexes, Color.magenta);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }

    public override void OnDeath()
    {
        foreach (GameObject markedTarget in markedTargets)
        {
            markedTarget.GetComponent<unitData>().buffArmor++;
        }
        base.OnDeath();
    }

}
