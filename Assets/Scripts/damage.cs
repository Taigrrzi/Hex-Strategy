using UnityEngine;
using System.Collections;

public class damage {

    public damage(GameObject obj, int dam,bool clo)
    {
        damagedObject = obj;
        damageAmount = dam;
        uncloak = clo;
    }

    public int damageAmount;
    public GameObject damagedObject;
    public bool uncloak;
}
