using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class HexMetrics
{

    public const float outerRadius = 2.55f;
    public const float innerRadius = outerRadius * 0.866025404f;

    public static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
    };
}

public class MapManager : MonoBehaviour {

    public static MapManager instance = null;

    public bool debug = false;
    public int rings = 1;
    public GameObject man = null;
    public GameObject circlePrefab = null;
    public GameObject crossPrefab = null;
    public float pourcentageWater = 0;
    public float poucentageMountain = 0;
    public float pourcentageForest = 0;
    public List<HexCell> biomeWater = null; 
    public List<HexCell> biomeDirt = null; 
    public List<HexCell> biomeMountains = null; 
    public List<HexCell> biomeForest = null;
    public GameObject debug_hex = null;

    private HexCell currentCell = null;
    private HexCell targetCell = null;
    private List<HexCell> map = new List<HexCell>();
    private GameObject circle = null;

    public List<HexCell> Map
    {
        get
        {
            return map;
        }

        set
        {
            map = value;
        }
    }

    public HexCell CurrentCell
    {
        get
        {
            return currentCell;
        }

        set
        {
            currentCell = value;
        }
    }

    public HexCell TargetCell
    {
        get
        {
            return targetCell;
        }

        set
        {
            targetCell = value;
        }
    }

    void Awake() {
        if (instance == null)
            instance = this;
        SubscribeToEvents();
    }

    void Start()
    {

    }

    void SetStartTile(HexCell cell)
    {
        CurrentCell = cell;

        circle = GameObject.Instantiate<GameObject>(circlePrefab);
        circle.transform.position = cell.transform.position;

        man.SetActive(true);
        man.transform.position = new Vector3(CurrentCell.transform.position.x, CurrentCell.transform.position.y + 0.4f, CurrentCell.transform.position.z - 0.1f);
        man.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    void CameraOnCurrentTile()
    {
        Camera.main.transform.Translate(CurrentCell.transform.position);
    }

    public List<HexCell> CreateMap() {
        int size = 1 + rings * 2;
        for (int index = 0; index < size; index++)
            StartCoroutine(CreateLayers(index));
        return map;
    }

    IEnumerator CreateLayers(int index) {
        
        //Middle layer
        HexCell cell = CreateCell(index, 0);

        if (index == rings)
            SetStartTile(cell);
        int counter = 0;
        int top_offset = 1;
        int bot_offset = 0;

        for (int layer = 0; layer < rings; layer++)
        {

            if (index > layer)
            {
                //Top layers
                CreateCell(index - top_offset, layer + 1);

                //bottom layers
                CreateCell(index - bot_offset, -(layer + 1));

                counter++;
                if (counter == 1)
                    bot_offset++;
                else if (counter == 2)
                {
                    top_offset++;
                    counter = 0;
                }
            }
            yield return new WaitForSeconds(0);

        }
    }

    HexCell PickRandomBiome() {
        float biome_pick = Random.value * 100;
        List<HexCell> list = null;

        if (biome_pick > pourcentageForest + pourcentageWater + poucentageMountain)
            list = biomeDirt;
        else if (biome_pick > pourcentageForest + pourcentageWater)
            list = biomeMountains;
        else if (biome_pick > pourcentageForest)
            list = biomeWater;
        else
            list = biomeForest;

        return list[Random.Range(0, list.Count)];
    }

    HexCell CreateCell(int x, int y)
    {
        Vector3 position;
        position.x = (x + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f) / 2;
        position.y = y * (HexMetrics.outerRadius * 1.5f) / 2;
        position.z = 0f;

        HexCell cell = Instantiate<HexCell>( PickRandomBiome() );
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.name = x.ToString() + "," + y.ToString();
        map.Add(cell);

        if (debug == true) {
            GameObject hex = Instantiate<GameObject>( debug_hex );
            hex.transform.SetParent(transform, false);
            hex.transform.localPosition = position;
        }
        return cell;
    }

    HexCell PickTarget()
    {
        return currentCell;
    }

    void HideMap()
    {
        foreach (HexCell cell in map)
            cell.gameObject.SetActive(false);
        if (circle)
            circle.SetActive(false);
        if (man)
            man.SetActive(false);
    }

    void DisplayMap()
    {
        foreach (HexCell cell in map)
            cell.gameObject.SetActive(true);
        if (circle)
            circle.SetActive(true);
        if (man)
            man.SetActive(true);
    }

    //Events & callbacks

    void SubscribeToEvents()
    {
        EventManager.OnStateChange += this.OnStateChange;
        EventManager.OnMap += this.OnMap;
        EventManager.OnTargetZoneStart += this.OnTargetZoneStart;
    }

    void OnMap()
    {
        if (map.Count == 0)
            CreateMap();
        else
            DisplayMap();

        CameraOnCurrentTile();
    }

    void OnStateChange(GameState state)
    {
        if (state != GameState.GAME_MENU ||
            state != GameState.MAP)
            HideMap();
    }

    void OnTargetZoneStart()
    {
        GameObject.Instantiate(crossPrefab);
    }
}
