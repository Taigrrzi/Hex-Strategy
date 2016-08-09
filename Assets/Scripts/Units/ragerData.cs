using UnityEngine;
using System.Collections;

public class ragerData : unitData
{
    void Start()
    {
        maxHealth = 12;
        currentHealth = 12;
        baseAttack = 0;
        baseMoveSpeed = 1;
        unitName = "Soldier";
        unitDesc = "Don't make him angry";
    }

    public override void OnTakingDamage(int damage, bool uncloak)
    {
        baseAttack++;
        base.OnTakingDamage(damage, uncloak);
    }
}