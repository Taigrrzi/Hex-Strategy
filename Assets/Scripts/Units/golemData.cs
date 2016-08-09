using UnityEngine;
using System.Collections;

public class golemData : unitData
{
    void Start()
    {
        maxHealth = 20;
        currentHealth = 20;
        baseAttack = 0;
        baseMoveSpeed = 2;
        unitName = "Golem";
        unitDesc = "A blank slate";
    }
}
