using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic;

public class mapControl : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;
    public HashSet<GameObject> validHexes;
    public GameObject hexPrefab;
    //float offsetPortraitX = 0.746f;
    //float offsetPortraitY = 0.8642f;
    float offsetX = 0.858f;
    float offsetY = 0.745f;
    public hexData[,] hexes;
    GameObject hoveredHex;
    public GameObject hexOutline;
    public int Team0StartUnitAmount;
    public int Team1StartUnitAmount;
    public List<GameObject> Team0Units;
    public List<GameObject> Team1Units;
    public int gamePhase;
    public HashSet<GameObject> Team0StartHexes;
    public HashSet<GameObject> Team1StartHexes;
    public static mapControl globalMap;
    public int teamTurn;
    public int turnAmount;
    public int startActionPoints=2;
    public int currentActionPoints;

    public Image turnDisplay;
    public Text actionPointDisplay;
    public GameObject selectedUnit;

    public List<GameObject> moveListeners;

    // Use this for initialization
    void Awake () {

        globalMap = this;
        hexes = new hexData[mapWidth + 2, mapHeight];
        GenerateMap();
        hexOutline = Instantiate((GameObject)Resources.Load("HexOutline"));
        hexOutline.transform.parent = transform;
        PopulateStartZones();
        turnAmount = -2;
        teamTurn = Mathf.FloorToInt(Random.Range(0,2)) ;
        if (teamTurn==0)
        {
            turnDisplay.color = Color.green;
        } else
        {
            turnDisplay.color = Color.red;
        }
        gamePhase = 0; //0=Unit Placement, 1=Actual Game, 2=End??
        for (int i = 0; i < Team0StartUnitAmount; i++)
        {
            GameObject newUnit = (GameObject)Instantiate(Resources.Load("Unit"));
            Team0Units.Add(newUnit);
            AddRandomUnitType(newUnit);
            newUnit.name = "Unit: " + i;
            newUnit.GetComponent<unitData>().team = 0;
            newUnit.GetComponent<unitData>().teamStartHexes = Team0StartHexes;
            CreateOnHex(newUnit, RandomHexInBounds(Team0StartHexes));
            newUnit.GetComponent<unitData>().UpdateSprite();
        }

        for (int i = 0; i < Team1StartUnitAmount; i++)
        {
            GameObject newUnit = (GameObject)Instantiate(Resources.Load("Unit"));
            Team1Units.Add(newUnit);
            AddRandomUnitType(newUnit);
            newUnit.name = "Enemy: " + i;
            newUnit.GetComponent<unitData>().team = 1;
            newUnit.GetComponent<unitData>().teamStartHexes = Team1StartHexes;
            CreateOnHex(newUnit, RandomHexInBounds(Team1StartHexes));
            newUnit.GetComponent<SpriteRenderer>().color = Color.red;
            newUnit.GetComponent<unitData>().UpdateSprite();
        }

    }

    public void AddRandomUnitType(GameObject unitToGiveType)
    {
        switch (Mathf.FloorToInt(Random.Range(0,12)))
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
            default:
                Debug.Log("Random is screwy");
                break;
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

    void PopulateStartZones()
    {
        Team0StartHexes = new HashSet<GameObject>();
        Team1StartHexes = new HashSet<GameObject>();

        Team0StartHexes.Add(hexes[0, 0].gameObject); //RectToHash(0,2,0,4);
        Team0StartHexes.Add(hexes[1, 0].gameObject); 
        Team0StartHexes.Add(hexes[1, 1].gameObject); 
        Team0StartHexes.Add(hexes[1, 2].gameObject); 
        Team0StartHexes.Add(hexes[2, 1].gameObject); 
        Team0StartHexes.Add(hexes[2, 2].gameObject);
        Team0StartHexes.Add(hexes[2, 3].gameObject); 
        Team0StartHexes.Add(hexes[3, 3].gameObject);

        Team1StartHexes.Add(hexes[7, 3].gameObject); //RectToHash(4, 6, 0, 4);
        Team1StartHexes.Add(hexes[6, 3].gameObject);
        Team1StartHexes.Add(hexes[6, 2].gameObject);
        Team1StartHexes.Add(hexes[6, 1].gameObject);
        Team1StartHexes.Add(hexes[5, 2].gameObject);
        Team1StartHexes.Add(hexes[5, 1].gameObject);
        Team1StartHexes.Add(hexes[5, 0].gameObject);
        Team1StartHexes.Add(hexes[4, 0].gameObject);

    }

    void CreateOnHex(GameObject unit, GameObject hex)
    {
        
        unit.GetComponent<unitData>().occupyingHex = hex;
        hex.GetComponent<hexData>().Fill(unit);
        unit.transform.position = hex.transform.position;
    }

    public void UnitMoved(GameObject movedUnit)
    {
        foreach (GameObject currentMoveListener in moveListeners)
        {
            currentMoveListener.GetComponent<unitData>().OnGlobalMove(movedUnit);
        }
    }

    public void StopMeListening(GameObject stopUnit)
    {
        if (moveListeners.Contains(stopUnit))
        {
            moveListeners.Remove(stopUnit);
        }
    }

    void Update()
    {
        if (gamePhase == 0)
        {
            currentActionPoints = startActionPoints;
        } 
        actionPointDisplay.text = ""+currentActionPoints;
        hexOutline.SetActive((selectedUnit!=null) ? true : false);

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

        validHexes.Remove(hexes[6, 0].gameObject);
        Destroy(hexes[6, 0].gameObject);
        hexes[7, 3] = ((GameObject)Instantiate(hexPrefab, new Vector3((7 - 0.5f * 3 )* offsetX, 3 * offsetY, 0), Quaternion.identity)).GetComponent<hexData>();
        hexes[7, 3].gameObject.name = "Hex " + 7 + "," + 3;
        hexes[7, 3].gameObject.transform.parent = transform;
        hexes[7, 3].myX = 7;
        hexes[7, 3].myY = 3;

        validHexes.Add(hexes[7, 3].gameObject);

        Camera.main.transform.position = hexes[Mathf.FloorToInt(mapWidth / 2), Mathf.FloorToInt(mapHeight / 2)].gameObject.transform.position;
        Camera.main.transform.Translate(Vector3.back * 10);
    }

    /*public void HighLightInRange(GameObject centralHex, int range, Color highlightColor,bool unoccupied)
    {
        HashSet<GameObject> hexesInRange = new HashSet<GameObject>();
        if (unoccupied == true)
        {
            hexesInRange = SelectInRangeUnoccupied(centralHex, range);
        }
        else
        {
            hexesInRange = SelectInRange(centralHex, range);
        }
        foreach (GameObject currentHex in hexesInRange)
        {
            currentHex.transform.GetChild(0).GetComponent<SpriteRenderer>().color = highlightColor;
        };
    }*/

    public void HighlightHash(HashSet<GameObject> hexesToHighlight, Color highlightColor)
    {
        foreach (GameObject currentHex in hexesToHighlight)
        {
            currentHex.transform.GetChild(0).GetComponent<SpriteRenderer>().color = highlightColor;
        };
    }
/*
    public void HighlightRect(int xStart,int xEnd,int yStart,int yEnd,Color highlightColor)
    {
        for (int i = xStart; i < xEnd; i++)
        {
            for (int j = yStart; j < yEnd; j++)
            {
                hexes[i,j].transform.GetChild(0).GetComponent<SpriteRenderer>().color = highlightColor;
            }
        }
    }

    public HashSet<GameObject> RectToHash(int xStart, int xEnd, int yStart, int yEnd)
    {
        HashSet<GameObject> hexesInRect = new HashSet<GameObject>();
        for (int i = xStart; i < xEnd; i++)
        {
            for (int j = yStart; j < yEnd; j++)
            {
                hexesInRect.Add(hexes[i,j].gameObject);
            }
        }
        return hexesInRect;
    }
*/
    public HashSet<GameObject> SelectInRangeUnoccupied(GameObject centralHex, int range)
    {

        HashSet<GameObject> hexesInRange = new HashSet<GameObject>();
        foreach (GameObject currentHex in SelectInRange(centralHex, range))
        {
            if (!currentHex.GetComponent<hexData>().occupied)
            {
                hexesInRange.Add(currentHex);
            }
        }
        return hexesInRange;
    }

    public HashSet<GameObject> SelectInRangeOccupied(GameObject centralHex, int range)
    {

        HashSet<GameObject> hexesInRange = new HashSet<GameObject>();
        foreach (GameObject currentHex in SelectInRange(centralHex, range))
        {
            if (currentHex.GetComponent<hexData>().occupied)
            {
                hexesInRange.Add(currentHex);
            }
        }
        return hexesInRange;
    }

    public void EndTurnButtonPressed()
    {
        turnAmount++;
        ClearHighlights();
        if (gamePhase == 0) {
            teamTurn = 1 - teamTurn;
            foreach (GameObject currentUnit in Team0Units)
            {
                currentUnit.GetComponent<unitData>().UpdateSprite();
            }
            foreach (GameObject currentUnit in Team1Units)
            {
                currentUnit.GetComponent<unitData>().UpdateSprite();
            }
            if (turnAmount==0) {
                gamePhase = 1;
            }
        } else
        {
                foreach (GameObject currentUnit in Team0Units)
                {
                    currentUnit.GetComponent<unitData>().OnTurnEnd();
                }
                foreach (GameObject currentUnit in Team1Units)
                {
                    currentUnit.GetComponent<unitData>().OnTurnEnd();
                }       
            currentActionPoints = startActionPoints;
            selectedUnit = null;
            teamTurn = 1 - teamTurn;
            foreach (GameObject currentUnit in Team0Units)
            {
                currentUnit.GetComponent<unitData>().OnTurnStart();
            }
            foreach (GameObject currentUnit in Team1Units)
            {
                currentUnit.GetComponent<unitData>().OnTurnStart();
            }
        }
        if (teamTurn == 0)
        {
            turnDisplay.color = Color.green;
        }
        else
        {
            turnDisplay.color = Color.red;
        }
    }

    public HashSet<GameObject> SelectInRange(GameObject centralHex, int range) {
        //int hexBuffer = ((centralHex.GetComponent<hexData>().myY % 2 == 0) ? 0 : 1);
        HashSet<GameObject> hexesInRange = new HashSet<GameObject>();
        int HexX = centralHex.GetComponent<hexData>().myX; 
        int HexY = centralHex.GetComponent<hexData>().myY;

        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                if (CheckHexExists(HexX + j, HexY + i))
                {
                    if (Mathf.Abs(i) + Mathf.Abs(j) > range && i * j < 0)
                    {
                        //Accidentally did it backwards...
                    } else
                    {
                        hexesInRange.Add(hexes[HexX + j, HexY + i].gameObject);
                    }
                }
            }
        }

        /*if (CheckHexExists(HexX+1,HexY)) {
            hexesInRange.Add(hexes[HexX+1, HexY].gameObject);
            if (range>1)
            {
                hexesInRange.UnionWith(SelectInRange(hexes[HexX+1,HexY].gameObject, range - 1));
            }
        }

        if (CheckHexExists(HexX - 1, HexY))
        {
            hexesInRange.Add(hexes[HexX-1, HexY].gameObject);
            if (range > 1)
            {
                hexesInRange.UnionWith(SelectInRange(hexes[HexX-1, HexY].gameObject, range - 1));
            }
        }

        if (CheckHexExists((HexX-1)+hexBuffer, HexY+1))
        {
            hexesInRange.Add(hexes[(HexX - 1) + hexBuffer, HexY + 1].gameObject);
            if (range > 1)
            {
                hexesInRange.UnionWith(SelectInRange(hexes[(HexX - 1) + hexBuffer, HexY + 1].gameObject, range - 1));
            }
        }

        if (CheckHexExists((HexX) + hexBuffer, HexY + 1))
        {
            hexesInRange.Add(hexes[HexX + hexBuffer, HexY+1].gameObject);
            if (range > 1)
            {
                hexesInRange.UnionWith(SelectInRange(hexes[HexX + hexBuffer, HexY+1].gameObject, range - 1));
            }
        }

        if (CheckHexExists((HexX-1) + hexBuffer, HexY - 1))
        {
            hexesInRange.Add(hexes[(HexX-1) + hexBuffer, HexY - 1].gameObject);
            if (range > 1)
            {
                hexesInRange.UnionWith(SelectInRange(hexes[(HexX-1) + hexBuffer, HexY - 1].gameObject, range - 1));
            }
        }

        if (CheckHexExists((HexX) + hexBuffer, HexY - 1))
        {
            hexesInRange.Add(hexes[HexX + hexBuffer, HexY - 1].gameObject);
            if (range > 1)
            {
                hexesInRange.UnionWith(SelectInRange(hexes[HexX + hexBuffer, HexY - 1].gameObject, range - 1));
            }
        }*/
        return hexesInRange;
    }

    public bool CheckHexExists(int X,int Y)
    {
        if (X >= 0 && Y >= 0 && X <= mapWidth + 1 && Y < mapHeight)
        {
            if (hexes[X, Y] != null)
            {
                return true;
            }
        }
        return false;
    }

    public void ClearHighlights()
    {
        /*for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth + 1; j++)
            {
                if (hexes[j , i] != null)
                {

                    hexes[j, i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;

                }
            }
        }
        hexes[7, 3].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        */
        foreach (GameObject hex in validHexes)
        {
            hex.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void MovePressed()
    {
        if (selectedUnit!=null)
        {
            selectedUnit.GetComponent<unitData>().OnMovePressed();
        }
    }

    public void AttackPressed()
    {
        if (selectedUnit != null)
        {
            selectedUnit.GetComponent<unitData>().OnAttackPressed();
        }
    }

    public void ActivePressed()
    {
        if (selectedUnit != null)
        {
            selectedUnit.GetComponent<unitData>().OnActivePressed();
        }
    }
}
