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

public class Map : MonoBehaviour {

    public int rings = 1;
    public float pourcentageWater = 0;
    public float poucentageMountain = 0;
    public float pourcentageForest = 0;
    public bool debug = false;
    public List<HexCell> biomeWater; 
    public List<HexCell> biomeDirt; 
    public List<HexCell> biomeMountains; 
    public List<HexCell> biomeForest;
    public GameObject man;
    public GameObject debug_hex;
    private HexCell currentCell;

    //public List<Color> biomes = new List<Color>(); 

    void Awake()
    {
        CreateMap();
    }

    void SetStartTile(HexCell cell)
    {
        currentCell = cell;
        currentCell.gameObject.GetComponent<SpriteRenderer>().color = Color.green;

        man.SetActive(true);
        man.transform.position = new Vector3(currentCell.transform.position.x, currentCell.transform.position.y + 0.7f, currentCell.transform.position.z - 0.1f);
        man.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        Camera.main.transform.Translate(currentCell.transform.position);
    }

    void CreateMap() {
        int size = 1 + rings * 2;
        for (int index = 0; index < size; index++)
            StartCoroutine(CreateLayers(index));
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

        if (debug == true) {
            GameObject hex = Instantiate<GameObject>( debug_hex );
            hex.transform.SetParent(transform, false);
            hex.transform.localPosition = position;
        }

        return cell;
    }

}
