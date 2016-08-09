using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chargerData : unitData
{
    bool charged = false;
    void Start()
    {
        maxHealth = 4;
        currentHealth = 4;
        baseAttack = 1;
        baseMoveSpeed = 1;
        unitName = "Battery";
        unitDesc = "Charge it with points";
        activeName = "Charge Up";
    }

    public override void OnActivePressed()
    {
        if (mapControl.globalMap.currentActionPoints > 0&&!charged) {
            OnActiveUse();
            charged = true;
            mapControl.globalMap.currentActionPoints--;
        }
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (charged)
        {
            charged = false;
            mapControl.globalMap.currentActionPoints++;
        }
    }
}