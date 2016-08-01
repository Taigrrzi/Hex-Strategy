using UnityEngine;
using System.Collections;

public class hexData : MonoBehaviour{
    public int myX;
    public int myY;
    public bool occupied;
    public GameObject occupyingObject;

    public void Empty()
    {
        occupyingObject = null;
        occupied = false;
    }

    public void Fill(GameObject fillingObj)
    {
        occupyingObject = fillingObj;
        occupied = true;
    }
}
