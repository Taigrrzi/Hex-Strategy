using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class conduitData : unitData
{
    void Start()
    {
        maxHealth = 6;
        currentHealth = 6;
        baseAttack = 2;
        baseMoveSpeed = 1;
        unitName = "Conduit";
        unitDesc = "Transfers damage to healthiest ally";
        StartSprite();
    }

    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_conduit");
    }
    public override void OnTakingDamage(int damage, bool uncloak,GameObject dealer)
    {
        if (mapControl.globalMap.explosionInProgress)
        {
            mapControl.globalMap.damageQueue.Add(new damage(gameObject, damage, uncloak,dealer));
        }
        else
        {
            if (uncloak)
            {
                OnUncloaking();
            }

            List<GameObject> teamArray;
            if (team == 0)
            {
                teamArray = mapControl.globalMap.Team0Units;
            }
            else
            {
                teamArray = mapControl.globalMap.Team1Units;
            }

            int health = 0;
            List<GameObject> healthyAllies = new List<GameObject>();
            foreach (GameObject ally in teamArray)
            {
                if (ally.GetComponent<unitData>().currentHealth > health)
                {
                    healthyAllies = new List<GameObject>();
                    healthyAllies.Add(ally);
                    health = ally.GetComponent<unitData>().currentHealth;
                }
                else if (ally.GetComponent<unitData>().currentHealth > health)
                {
                    healthyAllies.Add(ally);
                }
            }
            GameObject randomHealthiest = healthyAllies[Mathf.FloorToInt(Random.Range(0, healthyAllies.Count))];
            if (gameObject == randomHealthiest)
            {
                base.OnTakingDamage(damage, true,dealer);
            }
            else
            {
                randomHealthiest.GetComponent<unitData>().OnTakingDamage(damage, false,dealer);
            }
        }
    }
}