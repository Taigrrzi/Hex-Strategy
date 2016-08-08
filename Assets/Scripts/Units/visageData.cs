using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class visageData : unitData
{

    int explosionDamage;
    void Start()
    {
        maxHealth = 4;
        currentHealth = 4;
        baseAttack = 0;
        baseMoveSpeed = 2;
        explosionDamage = 8;
        unitName = "Visage";
        unitDesc = "Has 2 shadows";
    }

    public override void OnDeath()
    {
        occupyingHex.GetComponent<hexData>().Empty();
        GameObject newUnit = (GameObject)Instantiate(Resources.Load("Unit"));
        newUnit.AddComponent<spiritData>();
        newUnit.GetComponent<unitData>().team = team;
        mapControl.globalMap.CreateOnHex(newUnit, occupyingHex);
        if (team==0)
        {
            mapControl.globalMap.Team0Units.Add(newUnit);
            newUnit.name = "Unit: Spirit";
            newUnit.GetComponent<SpriteRenderer>().color = Color.green;
        } else
        {
            mapControl.globalMap.Team1Units.Add(newUnit);
            newUnit.name = "Enemy: Spirit";
            newUnit.GetComponent<SpriteRenderer>().color = Color.red;
        }
        Destroy(gameObject);
    }

}


