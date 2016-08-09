using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectionControl : Control {

    public int mapWidth;
    public int mapHeight;
    public HashSet<GameObject> validHexes;
    public GameObject hexPrefab;

//    public int teamTurn = 0;

    float offsetX = 0.858f;
    float offsetY = 0.745f;

    public hexData[,] hexes;
    GameObject hoveredHex;
    public GameObject hexOutline;

    public List<GameObject> hexTeam;
    
    public static CollectionControl globalCollection;

    public List<int> team;

//    public GameObject selectedUnit;

    public List<GameObject> moveListeners;

    // Use this for initialization
    void Awake()
    {
        globalCollection = this;
        hexes = new hexData[mapWidth + 2, mapHeight];
        GenerateMap();
        hexOutline = Instantiate((GameObject)Resources.Load("HexOutline"));
        hexOutline.transform.parent = transform;
        gamePhase = -1;
        PopulateCollection();
    }

    void Update()
    {
        hexOutline.SetActive((selectedUnit != null) ? true : false);

        if (selectedUnit != null)
        {
            hexOutline.transform.position = selectedUnit.transform.position;
            if (Input.touchCount >= 1)
            {
                if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
                    if (hit.collider != null)
                    {
                        GameObject touchedHex = hit.collider.gameObject;
                        selectedUnit.GetComponent<unitData>().OnHexTouchedSelected(touchedHex);
                    }
                }
            }
        }
        else
        {
            if (Input.touchCount >= 1)
            {
                if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
                    if (hit.collider != null)
                    {
                        GameObject touchedHex = hit.collider.gameObject;
                        if (touchedHex.GetComponent<hexData>().occupied)
                        {
                            selectedUnit = touchedHex.GetComponent<hexData>().occupyingObject;
                        }
                    }
                }
            }
        }
    }

    void GenerateMap()
    {
        validHexes = new HashSet<GameObject>();
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth + 1; j++)
            {
                float yPosition = i * offsetY;
                float xPosition = (j - 0.5f * i) * offsetX;

                hexes[j, i] = ((GameObject)Instantiate(hexPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity)).GetComponent<hexData>();
                hexes[j, i].gameObject.name = "Hex " + j + "," + i;
                hexes[j, i].gameObject.transform.parent = transform;
                hexes[j, i].myX = j;
                hexes[j, i].myY = i;

                validHexes.Add(hexes[j, i].gameObject);
            }
        }
        //Exclusive to 4 x 6
        validHexes.Remove(hexes[0, 1].gameObject);
        validHexes.Remove(hexes[0, 2].gameObject);
        validHexes.Remove(hexes[0, 3].gameObject);
        validHexes.Remove(hexes[1, 3].gameObject);
        Destroy(hexes[0, 1].gameObject);
        Destroy(hexes[0, 2].gameObject);
        Destroy(hexes[0, 3].gameObject);
        Destroy(hexes[1, 3].gameObject);

        validHexes.Remove(hexes[mapWidth, 0].gameObject);
        Destroy(hexes[mapWidth, 0].gameObject);
        hexes[mapWidth + 1, 3] = ((GameObject)Instantiate(hexPrefab, new Vector3((mapWidth + 1 - 0.5f * 3) * offsetX, 3 * offsetY, 0), Quaternion.identity)).GetComponent<hexData>();
        hexes[mapWidth + 1, 3].gameObject.name = "Hex " + mapWidth + 1 + "," + 3;
        hexes[mapWidth + 1, 3].gameObject.transform.parent = transform;
        hexes[mapWidth + 1, 3].myX = 7;
        hexes[mapWidth + 1, 3].myY = 3;

        validHexes.Add(hexes[mapWidth + 1, 3].gameObject);

        Camera.main.transform.position = hexes[Mathf.FloorToInt(mapWidth / 2), Mathf.FloorToInt(mapHeight / 2)].gameObject.transform.position;
        Camera.main.transform.Translate(Vector3.back * 10);
    }

    void PopulateCollection()
    {
        for (int i = 0; i < 31; i++)
        {
            GameObject newUnit = (GameObject)Instantiate(Resources.Load("Unit"));
            AddUnitType(newUnit,i);
            newUnit.name = "Unit: " + i;
            CreateOnHex(newUnit, RandomHexInBounds(validHexes));
        }
    }

    GameObject RandomHexInBounds(HashSet<GameObject> hexBound)
    { // Some Checking to do when i can be arsed. ie stop infinite loop if bound is full
        GameObject[] tempArray = new GameObject[hexBound.Count];
        hexBound.CopyTo(tempArray);

        int rand = Random.Range(0, hexBound.Count);
        while (tempArray[rand].GetComponent<hexData>().occupied)
        {
            rand = Random.Range(0, hexBound.Count);
        }
        return tempArray[rand];

    }

    public void CreateOnHex(GameObject unit, GameObject hex)
    {
        unit.GetComponent<unitData>().occupyingHex = hex;
        hex.GetComponent<hexData>().Fill(unit);
        unit.transform.position = hex.transform.position;
    }

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
            default:
                Debug.Log("Random is screwy");
                break;
        }

        unitToGiveType.GetComponent<unitData>().id = id;
    }

    public void Back()
    {
        GameObject.Find("SceneManager").GetComponent<SceneControl>().StartMenu();
    }


    public void SetTeam(int pos)
    {
        team[pos] = selectedUnit.GetComponent<unitData>().id;
        hexTeam[pos].GetComponentInChildren<UnityEngine.UI.Image>().sprite = selectedUnit.GetComponent<unitData>().uncloakedSprite;
    }

    public void SaveTeam(int t)
    {
        GameObject scene = GameObject.Find("SceneManager");
        if (t == 0)
        {
            scene.GetComponent<SceneControl>().Team0.Clear();
            scene.GetComponent<SceneControl>().Team0.AddRange(team);
        } else
        {
            scene.GetComponent<SceneControl>().Team1.Clear();
            scene.GetComponent<SceneControl>().Team1.AddRange(team);
        }
    }
}
