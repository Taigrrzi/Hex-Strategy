using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cushionerData : unitData
{

    public HashSet<GameObject> ajacentAllies;

    void Start()
    {
        maxHealth = 8;
        currentHealth = 8;
        baseAttack = 1;
        baseMoveSpeed = 1;
        unitName = "Cushioner";
        unitDesc = "Increases Nearby Ally's Armor";
    }

    public override void OnGameStart()
    {
        base.OnGameStart();
        foreach (GameObject adjacentHex in mapControl.globalMap.SelectInRange(occupyingHex, 1,true))
        {
            adjacentHex.GetComponent<hexData>().buffArmor++;
        }
    }

    public override void OnMoveStart()
    {
        foreach (GameObject adjacentHex in mapControl.globalMap.SelectInRange(occupyingHex, 1,true))
        {
            adjacentHex.GetComponent<hexData>().buffArmor--;
        }
    }

    public override void OnMoveEnd()
    {
        foreach (GameObject adjacentHex in mapControl.globalMap.SelectInRange(occupyingHex, 1,true))
        {
            adjacentHex.GetComponent<hexData>().buffArmor++;
        }
    }

    public override void OnDeath()
    {
        foreach (GameObject adjacentHex in mapControl.globalMap.SelectInRange(occupyingHex, 1, true))
        {
            adjacentHex.GetComponent<hexData>().buffArmor--;
        }
        mapControl.globalMap.moveListeners.Remove(gameObject);
        base.OnDeath();
    }
}
