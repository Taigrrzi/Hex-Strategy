using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class leaderData : unitData
{

    public HashSet<GameObject> ajacentAllies;

    void Start()
    {
        maxHealth = 8;
        currentHealth = 8;
        baseAttack = 1;
        baseMoveSpeed = 1;
        unitName = "Leader";
        unitDesc = "Increases Nearby Ally's Attack";
        StartSprite();
    }

    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_leader");
    }
    public override void OnGameStart()
    {
        base.OnGameStart();
        foreach (GameObject adjacentHex in mapControl.globalMap.SelectInRange(occupyingHex, 1, true))
        {
            adjacentHex.GetComponent<hexData>().buffAttack++;
        }
    }

    public override void OnMoveStart()
    {
        foreach (GameObject adjacentHex in mapControl.globalMap.SelectInRange(occupyingHex, 1, true))
        {
            adjacentHex.GetComponent<hexData>().buffAttack--;
        }
    }

    public override void OnMoveEnd()
    {
        foreach (GameObject adjacentHex in mapControl.globalMap.SelectInRange(occupyingHex, 1, true))
        {
            adjacentHex.GetComponent<hexData>().buffAttack++;
        }
    }

    public override void OnDeath()
    {
        foreach (GameObject adjacentHex in mapControl.globalMap.SelectInRange(occupyingHex,1, true))
        {
            adjacentHex.GetComponent<hexData>().buffAttack--;
        }
        mapControl.globalMap.moveListeners.Remove(gameObject);
        base.OnDeath();
    }
}
