using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class boobyTrapData : unitData
{

    int explosionDamage;
    void Start()
    {
        maxHealth = 5;
        currentHealth = 5;
        baseAttack = 0;
        baseMoveSpeed = 0;
        explosionDamage = 9;
        unitName = "Booby Trap";
        unitDesc = "Handle with Care";
        activeName = "Detonate";
    }

    public override void OnActivePressed()
    {
        if (mapControl.globalMap.currentActionPoints > 0)
        {
            HashSet<GameObject> ajacentUnits = mapControl.globalMap.SelectInRangeOccupied(occupyingHex,2,true);
            StartExplosion();
            foreach (GameObject ajacentUnit in ajacentUnits)
            {
                    ajacentUnit.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnTakingDamage(explosionDamage, false);
            }
            EndExplosion();
            mapControl.globalMap.currentActionPoints--;
            OnDeath();
        }
    }
}

