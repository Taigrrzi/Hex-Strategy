using UnityEngine;
using System.Collections;

public class damage {

    public damage(GameObject obj, int dam,bool clo,GameObject deal)
    {
        damagedObject = obj;
        damageAmount = dam;
        uncloak = clo;
        dealer = deal;
    }

    public GameObject dealer;
    public int damageAmount;
    public GameObject damagedObject;
    public bool uncloak;
}
