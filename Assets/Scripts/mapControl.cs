using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic;

public class mapControl : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;
    public GameObject hexPrefab;
    //float offsetPortraitX = 0.746f;
    //float offsetPortraitY = 0.8642f;
    float offsetX = 0.858f;
    float offsetY = 0.745f;
    public hexData[,] hexes;
    GameObject hoveredHex;
    public GameObject hexOutline;
    public int Team1StartUnitAmount;
    public int Team2StartUnitAmount;
    public List<GameObject> Team0Units;
    public List<GameObject> Team1Units;
    public int gamePhase;
    public HashSet<GameObject> Team0StartSquares;
    public HashSet<GameObject> Team1StartSquares;
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
        hexes = new hexData[mapWidth, mapHeight];
        GenerateMap();
        hexOutline = Instantiate((GameObject)Resources.Load("HexOutline"));
        hexOutline.transform.parent = transform;
        Team0StartSquares = RectToHash(0,2,0,4);
        Team1StartSquares = RectToHash(4, 6, 0, 4);
        turnAmount = 0;
        teamTurn = -1 ;

        gamePhase = 0; //0=Unit Placement, 1=Actual Game, 2=End??
        for (int i = 0; i < Team1StartUnitAmount; i++)
        {
            GameObject newUnit = (GameObject)Instantiate(Resources.Load("Unit"));
            Team0Units.Add(newUnit);
            AddRandomUnitType(newUnit);
            newUnit.name = "Unit: " + i;
            newUnit.GetComponent<unitData>().team = 0;
            CreateOnHex(newUnit, RandomHexInBounds(0, 2, 0, 4));
        }

        for (int i = 0; i < Team2StartUnitAmount; i++)
        {
            GameObject newUnit = (GameObject)Instantiate(Resources.Load("Unit"));
            Team1Units.Add(newUnit);
            AddRandomUnitType(newUnit);
            newUnit.name = "Enemy: " + i;
            newUnit.GetComponent<unitData>().team = 1;
            CreateOnHex(newUnit, RandomHexInBounds(4, 6, 0, 4));
            newUnit.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void AddRandomUnitType(GameObject unitToGiveType)
    {
        switch (Mathf.FloorToInt(Random.Range(0,11)))
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
            default:
                Debug.Log("Random is screwy");
                break;
        }
    }

    GameObject RandomHexInBounds(int xMin,int xMax,int yMin, int yMax)
    { // Some Checking to do when i can be arsed. ie stop infinite loop if bound is full
        GameObject currentHex;
        do {
            currentHex = hexes[Mathf.FloorToInt(Random.Range(xMin, xMax)), Mathf.FloorToInt(Random.Range(yMin, yMax))].gameObject;
        }
        while (currentHex.GetComponent<hexData>().occupied == true);
        return currentHex ;
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
        if (teamTurn == 0) {
            turnDisplay.color = Color.green;
        } else if(teamTurn==1)
        {
            turnDisplay.color = Color.red;
        } else
        {
            currentActionPoints = startActionPoints;
            turnDisplay.color = Color.yellow;
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
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                float yPosition = i * offsetY;
                float xPosition = j * offsetX;
                if (i % 2 == 1)
                {
                    xPosition += offsetX/2;
                }
                hexes[j, i] = ((GameObject)Instantiate(hexPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity)).GetComponent<hexData>();
                hexes[j, i].gameObject.name = "Hex " + j + "," + i;
                hexes[j, i].gameObject.transform.parent = transform;
                hexes[j, i].myX = j;
                hexes[j, i].myY = i;
            }
        }
        Camera.main.transform.position = hexes[Mathf.FloorToInt(mapWidth / 2), Mathf.FloorToInt(mapHeight / 2)].gameObject.transform.position;
        Camera.main.transform.Translate(Vector3.back * 10);
    }

    public void HighLightInRange(GameObject centralHex, int range, Color highlightColor,bool unoccupied)
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
    }

    public void HighlightHash(HashSet<GameObject> hexesToHighlight, Color highlightColor)
    {
        foreach (GameObject currentHex in hexesToHighlight)
        {
            currentHex.transform.GetChild(0).GetComponent<SpriteRenderer>().color = highlightColor;
        };
    }

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
        if (gamePhase == 0) {
            gamePhase = 1;
            teamTurn = 1;
        } else
        {
            if (teamTurn==0)
            {
                foreach (GameObject currentUnit in Team0Units)
                {
                    currentUnit.GetComponent<unitData>().OnTurnEnd();
                }
            } else
            {
                foreach (GameObject currentUnit in Team1Units)
                {
                    currentUnit.GetComponent<unitData>().OnTurnEnd();
                }
            }
            teamTurn = 1 - teamTurn;
            currentActionPoints = startActionPoints;
            selectedUnit = null;
        }
    }

    public HashSet<GameObject> SelectInRange(GameObject centralHex, int range) {
        int hexBuffer = ((centralHex.GetComponent<hexData>().myY % 2 == 0) ? 0 : 1);
        HashSet<GameObject> hexesInRange = new HashSet<GameObject>();
        int HexX = centralHex.GetComponent<hexData>().myX; 
        int HexY = centralHex.GetComponent<hexData>().myY;

        if (CheckHexExists(HexX+1,HexY)) {
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
        }
        return hexesInRange;
    }

    public bool CheckHexExists(int X,int Y)
    {
        if (X >= 0&& X < mapWidth)
        {
            if (Y >= 0&&Y<mapHeight)
            {
                if (hexes[X,Y] != null)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
        else return false;
    }

    public void ClearHighlights()
    {
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                hexes[j, i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }
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
