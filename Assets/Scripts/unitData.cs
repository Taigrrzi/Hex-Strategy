using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class unitData : MonoBehaviour {

    public GameObject occupyingHex;
    public int maxHealth;
    public int currentHealth;
    public int baseAttack;
    public int baseMoveSpeed;
    public int currentAttack;
    public int buffAttack=0;
    public int buffMoveSpeed=0;
    public HashSet<GameObject> validHexes;
    public int team;
    public int mode;// 0 = unselected 1= moving 2=attacking 3=using active
    public string unitName = "Blank Unit";
    public string unitDesc = "Blank Description";
    public string activeName = "No Active";
    public bool shielded=false;
    public bool cloaked;

    public virtual void OnHexTouchedSelected(GameObject hexTouched)
    {
        switch (mode)
        {
            case 0:
                if (hexTouched.GetComponent<hexData>().occupied&&hexTouched.GetComponent<hexData>().occupyingObject.tag=="Unit")
                {
                    //if (hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().team==team)
                    //{
                        if (hexTouched == occupyingHex)
                        {
                            mapControl.globalMap.selectedUnit = null;
                        }
                        else
                        {
                            mapControl.globalMap.selectedUnit = hexTouched.GetComponent<hexData>().occupyingObject;
                        }
                    //}
                }
                break;
            case 1:
                if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 0)
                {
                    MoveToHex(hexTouched);
                    LoseFocus();
                    mapControl.globalMap.currentActionPoints--;
                }
                break;
            case 2:
                if (validHexes.Contains(hexTouched) && mapControl.globalMap.currentActionPoints > 0)
                {
                    OnAttacking();
                    hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnTakingDamage(currentAttack);
                    LoseFocus();
                    mapControl.globalMap.currentActionPoints--;
                }
                break;
            default:
                break;
        }
    }

    public virtual void OnActiveUse()
    {

    }

    public virtual void OnAttacking()
    {
        currentAttack = baseAttack+buffAttack;
    }

    public virtual void OnMovePressed() {
        if (mode==1)
        {
            mode = 0;
            mapControl.globalMap.ClearHighlights();
        } else
        {
            mode = 1;
            if (mapControl.globalMap.teamTurn == team)
            {
                    validHexes = mapControl.globalMap.SelectInRangeUnoccupied(occupyingHex.gameObject, baseMoveSpeed+buffMoveSpeed);
            } else if (mapControl.globalMap.teamTurn == -1) 
            {
                if (team == 0)
                {
                    validHexes = mapControl.globalMap.Team0StartSquares;
                } else
                {
                    validHexes = mapControl.globalMap.Team1StartSquares;
                }
            }
            mapControl.globalMap.HighlightHash(validHexes, Color.green);
        }
    }

    public virtual void OnAttackPressed() {
        if (mode == 2)
        {
            mode = 0;
            mapControl.globalMap.ClearHighlights();
        }
        else
        {
            if (baseAttack > 0)
            {
                mode = 2;
                validHexes = GetEnemyHexesInRange(1);
                mapControl.globalMap.HighlightHash(validHexes, Color.red);
                if (validHexes.Count == 0)
                {
                    mode = 0;
                }
            }
        }
    }

    public virtual void OnActivePressed() {
        if (mode == 3)
        {
            mode = 0;
            mapControl.globalMap.ClearHighlights();
        }
        else
        {
            mode = 3;
        }
    }

    public HashSet<GameObject> GetEnemyHexesInRange (int hexRange) {
        HashSet<GameObject> tempHexes = new HashSet<GameObject>();
            foreach (GameObject currentHex in mapControl.globalMap.SelectInRangeOccupied(occupyingHex, hexRange))
            {
                if (currentHex.GetComponent<hexData>().occupyingObject.tag == "Unit")
                {
                    if (currentHex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().team != team)
                    {
                        tempHexes.Add(currentHex);
                    }
                }
            }
        return tempHexes;
    }

    public HashSet<GameObject> GetAllyHexesInRange(int hexRange)
    {
        HashSet<GameObject> tempHexes = new HashSet<GameObject>();
            foreach (GameObject currentHex in mapControl.globalMap.SelectInRangeOccupied(occupyingHex, hexRange))
            {
                if (currentHex.GetComponent<hexData>().occupyingObject.tag == "Unit")
                {
                    if (currentHex.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().team == team)
                    {
                        tempHexes.Add(currentHex);
                    }
                }
            }
        return tempHexes;
    }

    public virtual void OnTurnEnd() {}

    public virtual void OnTakingDamage(int damage)
    {
        if (!shielded)
        {
            currentHealth -= damage;
        } else
        {
            shielded = false;
        }
        if (currentHealth<=0)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath() {
        occupyingHex.GetComponent<hexData>().Empty();
        if (team == 0) {
            mapControl.globalMap.Team0Units.Remove(gameObject);
                } else
        {
            mapControl.globalMap.Team1Units.Remove(gameObject);
        }
        Destroy(gameObject);
    }

    public void MoveToHex(GameObject hex)
    {
        occupyingHex.GetComponent<hexData>().Empty();
        occupyingHex = hex;
        hex.GetComponent<hexData>().Fill(gameObject);
        transform.position = hex.transform.position;
        mapControl.globalMap.UnitMoved(gameObject);
    }

    public virtual void OnGlobalMove(GameObject movedUnit)
    {

    }

    public void LoseFocus()
    {
        mapControl.globalMap.selectedUnit = null;
        mapControl.globalMap.ClearHighlights();
        mode = 0;
    }

}
