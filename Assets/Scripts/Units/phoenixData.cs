using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class phoenixData : unitData
{

    void Start()
    {
        maxHealth = 7;
        currentHealth = 7;
        baseAttack = 3;
        baseMoveSpeed = 1;
        unitName = "Phoenix";
        unitDesc = "Born in flame";
    }

    public override void OnDeath()
    {
        occupyingHex.GetComponent<hexData>().Empty();
        GameObject newUnit = (GameObject)Instantiate(Resources.Load("Unit"));
        newUnit.AddComponent<eggData>();
        newUnit.GetComponent<unitData>().team = team;
        mapControl.globalMap.CreateOnHex(newUnit, occupyingHex);
        if (team == 0)
        {
            mapControl.globalMap.Team0Units.Add(newUnit);
            newUnit.name = "Unit: Egg";
            newUnit.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            mapControl.globalMap.Team1Units.Add(newUnit);
            newUnit.name = "Enemy: Egg";
            newUnit.GetComponent<SpriteRenderer>().color = Color.red;
        }
        Destroy(gameObject);
    }

}


