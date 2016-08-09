using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
     public GameObject selectedUnit;
     public int gamePhase;
     public int teamTurn;
    public int unitAmount = 35;

    public void AddUnitType(GameObject unitToGiveType, int id)
    {
        switch (id)
        {
            case 0:
                unitToGiveType.AddComponent<soldierData>();
                break;
            case 1:
                unitToGiveType.AddComponent<scoutData>();
                break;
            case 2:
                unitToGiveType.AddComponent<sniperData>();
                break;
            case 3:
                unitToGiveType.AddComponent<tankData>();
                break;
            case 4:
                unitToGiveType.AddComponent<healerData>();
                break;
            case 5:
                unitToGiveType.AddComponent<missileData>();
                break;
            case 6:
                unitToGiveType.AddComponent<armorerData>();
                break;
            case 7:
                unitToGiveType.AddComponent<weaponerData>();
                break;
            case 8:
                unitToGiveType.AddComponent<acceleratorData>();
                break;
            case 9:
                unitToGiveType.AddComponent<glasscannonData>();
                break;
            case 10:
                unitToGiveType.AddComponent<grenadierData>();
                break;
            case 11:
                unitToGiveType.AddComponent<shielderData>();
                break;
            case 12:
                unitToGiveType.AddComponent<leaderData>();
                break;
            case 13:
                unitToGiveType.AddComponent<cushionerData>();
                break;
            case 14:
                unitToGiveType.AddComponent<visageData>();
                break;
            case 15:
                unitToGiveType.AddComponent<mortarData>();
                break;
            case 16:
                unitToGiveType.AddComponent<fastSoldierData>();
                break;
            case 17:
                unitToGiveType.AddComponent<conduitData>();
                break;
            case 18:
                unitToGiveType.AddComponent<gasserData>();
                break;
            case 19:
                unitToGiveType.AddComponent<vampireData>();
                break;
            case 20:
                unitToGiveType.AddComponent<boobyTrapData>();
                break;
            case 21:
                unitToGiveType.AddComponent<investigatorData>();
                break;
            case 22:
                unitToGiveType.AddComponent<ragerData>();
                break;
            case 23:
                unitToGiveType.AddComponent<chargerData>();
                break;
            case 24:
                unitToGiveType.AddComponent<paladinData>();
                break;
            case 25:
                unitToGiveType.AddComponent<sprinterData>();
                break;
            case 26:
                unitToGiveType.AddComponent<turretData>();
                break;
            case 27:
                unitToGiveType.AddComponent<demonData>();
                break;
            case 28:
                unitToGiveType.AddComponent<murdererData>();
                break;
            case 29:
                unitToGiveType.AddComponent<pathFinderData>();
                break;
            case 30:
                unitToGiveType.AddComponent<warperData>();
                break;
            case 31:
                unitToGiveType.AddComponent<debufferData>();
                break;
            case 32:
                unitToGiveType.AddComponent<queenData>();
                break;
            case 33:
                unitToGiveType.AddComponent<shamanData>();
                break;
            case 34:
                unitToGiveType.AddComponent<phoenixData>();
                break;
            default:
                Debug.Log("Random is screwy");
                break;
        }
        unitToGiveType.GetComponent<unitData>().id = id;
    }

}
