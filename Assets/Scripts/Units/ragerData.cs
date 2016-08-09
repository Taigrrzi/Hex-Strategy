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
        unitName = "Beserker";
        unitDesc = "Don't make him angry";
    }

    public override void OnTakingDamage(int damage, bool uncloak,GameObject dealer)
    {
        baseAttack++;
        base.OnTakingDamage(damage, uncloak,dealer);
    }
}