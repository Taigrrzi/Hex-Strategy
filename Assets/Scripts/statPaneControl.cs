using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class statPaneControl : MonoBehaviour {

    public string unitType;
    public int currentHealth;
    public int moveSpeed;
    public int maxHealth;
    public int attack;
    public string desc;
    unitData unit;
    public Text activeText;
    public Text descPane;
	
	// Update is called once per frame
	void Update () {
	    if (mapControl.globalMap.selectedUnit==null)
        {
            transform.parent.parent.GetChild(0).gameObject.SetActive(false);
            transform.parent.parent.GetChild(1).gameObject.SetActive(false);
            transform.parent.parent.GetChild(2).gameObject.SetActive(false);
            //GetComponent<Text>().text = "\nUnit Type:\n\n\nHealth:\n/\n\nAttack:\n\n\n";
            GetComponent<Text>().text = "\nUnit Type:\n\nHealth:\n\n\nAttack: Speed:\n\n";
            activeText.text = "Active Ability";
            descPane.text = "";
        } else
        {

            unit = mapControl.globalMap.selectedUnit.GetComponent<unitData>();

            if (mapControl.globalMap.teamTurn == unit.team)
            {
                transform.parent.parent.GetChild(0).gameObject.SetActive(true);
                transform.parent.parent.GetChild(1).gameObject.SetActive(true);
                transform.parent.parent.GetChild(2).gameObject.SetActive(true);
            } else if (mapControl.globalMap.teamTurn == -1)
            {
                transform.parent.parent.GetChild(0).gameObject.SetActive(true);
                transform.parent.parent.GetChild(1).gameObject.SetActive(false);
                transform.parent.parent.GetChild(2).gameObject.SetActive(false);
            } else
            {
                transform.parent.parent.GetChild(0).gameObject.SetActive(false);
                transform.parent.parent.GetChild(1).gameObject.SetActive(false);
                transform.parent.parent.GetChild(2).gameObject.SetActive(false);
            }
            unitType = unit.unitName;
            currentHealth = unit.currentHealth;
            maxHealth = unit.maxHealth;
            moveSpeed = unit.buffMoveSpeed + unit.baseMoveSpeed;
            attack = unit.baseAttack+unit.buffAttack;
            descPane.text = unit.unitDesc;
            activeText.text = unit.activeName;
            GetComponent<Text>().text = "\nUnit Type:\n" + unitType + "\nHealth:\n" + currentHealth + "/" + maxHealth + "\nAttack: Speed:\n" + attack + "       "+moveSpeed;
        }
    }
}
