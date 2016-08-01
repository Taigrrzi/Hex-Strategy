using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class missileData : unitData
{

    int explosionDamage;
    void Start()
    {
        maxHealth = 4;
        currentHealth = 4;
        baseAttack = 0;
        baseMoveSpeed = 2;
        explosionDamage = 8;
        unitName = "Missile";
        unitDesc = "Can move fast as blow up";
        activeName = "Explode";
    }

    public override void OnActivePressed()
    {
        if (mapControl.globalMap.currentActionPoints > 0)
        {
            OnDeath();
            mapControl.globalMap.currentActionPoints--;
        }
    }

    public override void OnDeath()
    {
        HashSet<GameObject> ajacentUnits = GetEnemyHexesInRange(1);
            foreach (GameObject ajacentUnit in ajacentUnits)
            {
                ajacentUnit.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnTakingDamage(explosionDamage);
            }
        occupyingHex.GetComponent<hexData>().Empty();
        if (team == 0)
        {
            mapControl.globalMap.Team0Units.Remove(gameObject);
        }
        else
        {
            mapControl.globalMap.Team1Units.Remove(gameObject);
        }
        Destroy(gameObject);
    }

}

