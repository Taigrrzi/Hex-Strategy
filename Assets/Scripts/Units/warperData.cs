using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class warperData : unitData
{
    void Start()
    {
        maxHealth = 20;
        currentHealth = 20;
        baseAttack = 1;
        baseMoveSpeed = 1;
        unitName = "Warper";
        unitDesc = "Mastery of time and space";
        activeName = "Teleport To Ally";
    }

    public override void OnHexTouchedSelected(GameObject hexTouched)
    {
        base.OnHexTouchedSelected(hexTouched);
        if (mode == 3)
        {
            if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 0)
            {
                OnActiveUse();
                MoveToHex(hexTouched);
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
            validHexes = new HashSet<GameObject>();
            List<GameObject> teamHexes = new List<GameObject>();
            if (team==0)
            {
                teamHexes = mapControl.globalMap.Team0Units;
            } else
            {
                teamHexes = mapControl.globalMap.Team1Units;
            }
            foreach(GameObject ally in teamHexes)
            {
                if (ally != gameObject)
                {
                    validHexes.UnionWith(mapControl.globalMap.SelectInRangeUnoccupied(ally.GetComponent<unitData>().occupyingHex, 1, true));
                }
            }
            mapControl.globalMap.HighlightHash(validHexes, Color.green);
            if (validHexes.Count == 0)
            {
                mode = 0;
            }
        }
    }

}
