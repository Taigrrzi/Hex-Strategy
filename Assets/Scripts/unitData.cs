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
    public int baseArmor;
    public int buffArmor = 0;
    public GameObject shieldObj;
    public Sprite uncloakedSprite;
    public int buffMoveSpeed=0;
    public HashSet<GameObject> validHexes;
    public int team;
    public int mode;// 0 = unselected 1= moving 2=attacking 3=using active
    public string unitName = "Blank Unit";
    public string unitDesc = "Blank Description";
    public string activeName = "No Active";
    public bool shielded=false;
    public bool cloaked=true;
    public HashSet<GameObject> teamStartHexes;

    public void Awake()
    {
        uncloakedSprite = Resources.Load<Sprite>("best_unit");
    }

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
                    hexTouched.GetComponent<hexData>().occupyingObject.GetComponent<unitData>().OnTakingDamage(currentAttack,true);
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
        currentAttack = baseAttack+buffAttack+occupyingHex.GetComponent<hexData>().buffAttack;
        OnUncloaking();
    }

    public virtual void OnHealing(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth>maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public virtual void OnMovePressed() {
        if (mode==1)
        {
            mode = 0;
            mapControl.globalMap.ClearHighlights();
        } else
        {
            mode = 1;
            if (mapControl.globalMap.gamePhase == 0) 
            {
                validHexes = new HashSet<GameObject>();
                foreach (GameObject currentHex in teamStartHexes)
                {
                    if (!currentHex.GetComponent<hexData>().occupied)
                    {
                        validHexes.Add(currentHex);
                    }
                }
            } else if (mapControl.globalMap.teamTurn == team)
            {
                if (baseMoveSpeed + buffMoveSpeed > 0)
                {
                    validHexes = mapControl.globalMap.SelectInRangeUnoccupied(occupyingHex.gameObject, baseMoveSpeed + buffMoveSpeed,true);
                } else
                {
                    mode = 0;
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
            if (baseAttack+buffAttack+occupyingHex.GetComponent<hexData>().buffAttack > 0)
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
            mapControl.globalMap.ClearHighlights();
        }
    }

    public HashSet<GameObject> GetEnemyHexesInRange (int hexRange) {
        HashSet<GameObject> tempHexes = new HashSet<GameObject>();
            foreach (GameObject currentHex in mapControl.globalMap.SelectInRangeOccupied(occupyingHex, hexRange, true))
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
            foreach (GameObject currentHex in mapControl.globalMap.SelectInRangeOccupied(occupyingHex, hexRange,false))
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

    public virtual void OnTurnEnd()
    {
    }

    public virtual void OnTurnStart()
    {
        UpdateSprite();
    }

    public virtual void OnMoveStart()
    {

    }

    public virtual void OnMoveEnd()
    {

    }

    public virtual void OnShieldGained()
    {
        shielded = true;
        shieldObj = (GameObject)Instantiate(Resources.Load<GameObject>("shieldPrefab"), transform);
        shieldObj.transform.localPosition = Vector3.zero;
    }

    public virtual void OnShieldLost()
    {
        shielded = false;
        Destroy(shieldObj);
    }

    public virtual void UpdateSprite()
    {
        if (mapControl.globalMap.teamTurn != team)
        {
            if (cloaked)
            {
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("best_unit_cloak");
            }
        }
        else {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("best_unit");
        }
    }

    public void OnUncloaking()
    {
        cloaked = false;
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("best_unit");
    }

    public virtual void OnTakingDamage(int damage,bool uncloak)
    {
        if (uncloak)
        {
            OnUncloaking();
        }
        if (!shielded)
        {
            currentHealth -= (int)Mathf.Clamp(((damage-baseArmor)-buffArmor-occupyingHex.GetComponent<hexData>().buffArmor),0,Mathf.Infinity);
        } else
        {
            OnShieldLost();
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

    public virtual void OnGameStart()
    {
        UpdateSprite();
    }

    public void MoveToHex(GameObject hex)
    {
        OnMoveStart();
        occupyingHex.GetComponent<hexData>().Empty();
        occupyingHex = hex;
        hex.GetComponent<hexData>().Fill(gameObject);
        transform.position = hex.transform.position;
        mapControl.globalMap.UnitMoved(gameObject);
        if (shielded)
        {
            shieldObj.transform.localPosition = Vector3.zero;
        }
        OnMoveEnd();
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
