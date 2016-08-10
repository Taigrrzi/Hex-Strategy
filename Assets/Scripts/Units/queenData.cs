using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class queenData : unitData
{

    void Start()
    {
        maxHealth = 1;
        currentHealth = 8;
        baseAttack = 1;
        baseMoveSpeed = 1;
        unitName = "Hive Queen";
        unitDesc = "Eugh";
        activeName = "Give Birth";
        StartSprite();
    }

    public override void StartSprite()
    {
        uncloakedSprite = Resources.Load<Sprite>("devUnit_queen");
    }

    public override void OnHexTouchedSelected(GameObject hexTouched)
    {
        base.OnHexTouchedSelected(hexTouched);
        if (mode == 3)
        {
            if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 0)
            {
                OnActiveUse();

                GameObject newUnit = (GameObject)Instantiate(Resources.Load("Unit"));
                newUnit.AddComponent<droneData>();
                newUnit.GetComponent<unitData>().team = team;
                mapControl.globalMap.CreateOnHex(newUnit, hexTouched);
                if (team == 0)
                {
                    mapControl.globalMap.Team0Units.Add(newUnit);
                    newUnit.name = "Unit: Drone";
                    newUnit.GetComponent<SpriteRenderer>().color = Color.green;
                }
                else
                {
                    mapControl.globalMap.Team1Units.Add(newUnit);
                    newUnit.name = "Enemy: Drone";
                    newUnit.GetComponent<SpriteRenderer>().color = Color.red;
                }

                LoseFocus();
                mapControl.globalMap.currentActionPoints--;
            }
        }
    }

    public override void OnActivePressed()
    {
        mapControl.globalMap.ClearHighlights();
        if (mode == 3)
        {
            mode = 0;
        }
        else
        {
            mode = 3;
            validHexes = mapControl.globalMap.SelectInRangeUnoccupied(occupyingHex, 1, true);
            mapControl.globalMap.HighlightHash(validHexes, Color.yellow);
        }
    }
}

