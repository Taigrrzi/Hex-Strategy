using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectionControl : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;
    public HashSet<GameObject> validHexes;
    public GameObject hexPrefab;

    float offsetX = 0.858f;
    float offsetY = 0.745f;

    public hexData[,] hexes;
    GameObject hoveredHex;
    public GameObject hexOutline;

    public List<GameObject> Team0Units;
    public List<GameObject> Team1Units;


    public GameObject selectedUnit;

    public List<GameObject> moveListeners;

    // Use this for initialization
    void Awake()
    {

        hexes = new hexData[mapWidth + 2, mapHeight];
        GenerateMap();
        hexOutline = Instantiate((GameObject)Resources.Load("HexOutline"));
        hexOutline.transform.parent = transform;
/*
        for (int i = 0; i < Team0StartUnitAmount; i++)
        {
            GameObject newUnit = (GameObject)Instantiate(Resources.Load("Unit"));
            Team0Units.Add(newUnit);
            AddRandomUnitType(newUnit);
            newUnit.name = "Unit: " + i;
            newUnit.GetComponent<unitData>().team = 0;
            newUnit.GetComponent<unitData>().teamStartHexes = Team0StartHexes;
            CreateOnHex(newUnit, RandomHexInBounds(Team0StartHexes));
            newUnit.GetComponent<unitData>().OnGameStart();
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
            newUnit.GetComponent<unitData>().OnGameStart();
        }
        */
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

        validHexes.Remove(hexes[6, 0].gameObject);
        Destroy(hexes[6, 0].gameObject);
        hexes[7, 3] = ((GameObject)Instantiate(hexPrefab, new Vector3((7 - 0.5f * 3) * offsetX, 3 * offsetY, 0), Quaternion.identity)).GetComponent<hexData>();
        hexes[7, 3].gameObject.name = "Hex " + 7 + "," + 3;
        hexes[7, 3].gameObject.transform.parent = transform;
        hexes[7, 3].myX = 7;
        hexes[7, 3].myY = 3;

        validHexes.Add(hexes[7, 3].gameObject);

        Camera.main.transform.position = hexes[Mathf.FloorToInt(mapWidth / 2), Mathf.FloorToInt(mapHeight / 2)].gameObject.transform.position;
        Camera.main.transform.Translate(Vector3.back * 10);
    }

    public void Back()
    {
        GameObject.Find("SceneManager").GetComponent<SceneControl>().StartMenu();
    }
}
