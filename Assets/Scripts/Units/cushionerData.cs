using UnityEngine;
using System.Collections;

public class cushioner : unitData
{
    void Start()
    {
        maxHealth = 5;
        currentHealth = 5;
        baseAttack = 2;
        baseMoveSpeed = 1;
        unitName = "Cushioner";
        unitDesc = "Reduces damage taken by nearby allys";
        mapControl.globalMap.moveListeners.Add(gameObject);
    }

    public override void OnGlobalMove(GameObject movedUnit)
    {
        //if (mapControl.globalMap.AreAjacentHexes(occupyingHex,movedUnit))
    }
}
