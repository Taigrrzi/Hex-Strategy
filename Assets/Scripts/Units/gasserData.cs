using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gasserData : unitData
{


    int gasDamage;
    void Start()
    {
        maxHealth = 10;
        currentHealth = 10 ;
        baseAttack = 1;
        baseMoveSpeed = 1;
        gasDamage = 1;
        unitName = "Gasser";
        unitDesc = "Deals damage to nearby enemies at the end of the turn";
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        foreach (GameObject enemyhex in GetEnemyHexesInRange(1))
        {
            unitData unit = enemyhex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>();
            unit.OnTakingDamage(gasDamage,false,gameObject);
        }
    }

}

