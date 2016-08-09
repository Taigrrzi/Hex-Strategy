using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class eggData : unitData
{
    bool hadATurn=true;
    void Start()
    {
        maxHealth = 1;
        currentHealth = 1;
        baseAttack = 0;
        baseMoveSpeed = 0;
        unitName = "Phoenix Egg";
        unitDesc = "Growing Fast";
        OnUncloaking();
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (hadATurn)
        {
            if (mapControl.globalMap.teamTurn == team)
            {
                occupyingHex.GetComponent<hexData>().Empty();
                GameObject newUnit = (GameObject)Instantiate(Resources.Load("Unit"));
                newUnit.AddComponent<phoenixData>();
                newUnit.GetComponent<unitData>().team = team;
                mapControl.globalMap.CreateOnHex(newUnit, occupyingHex);
                if (team == 0)
                {
                    mapControl.globalMap.Team0Units.Add(newUnit);
                    newUnit.name = "Unit: Phoenix";
                    newUnit.GetComponent<SpriteRenderer>().color = Color.green;
                }
                else
                {
                    mapControl.globalMap.Team1Units.Add(newUnit);
                    newUnit.name = "Enemy: Phoenix";
                    newUnit.GetComponent<SpriteRenderer>().color = Color.red;
                }
                Destroy(gameObject);
            }
        } else
        {
            hadATurn = true;
        }
    }
}