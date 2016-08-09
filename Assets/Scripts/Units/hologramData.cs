using UnityEngine;
using System.Collections;

public class hologramData : unitData {

    void Start()
    {
        maxHealth = 1;
        currentHealth = 1;
        baseAttack = 0;
        baseMoveSpeed = 0;
        unitName = "Hologram";
        unitDesc = "Don't look closely";
    }
}
