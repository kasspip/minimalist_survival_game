using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Direction
{
    public static float Right = 1;
    public static float Left = -1;
}

public class Rail : MonoBehaviour {

    public float speed = 5;
    public float depth = 0;
    public float spaceBetween = 0;
    public float noiseSpaceBetween = 0;
    public Color colorShift = new Color(1,1,1);
    public List<GameObject> elementLibray = new List<GameObject>();
    List<GameObject> rail = new List<GameObject>();
    Vector3 stageDimensions = new Vector3();

    public float NoiseSpaceBetween
    {
        get
        {
            float val = Random.Range(0, noiseSpaceBetween);
            if (Random.value > 0.5f)
                val = -val;
            return val;
        }

        set
        {
            noiseSpaceBetween = value;
        }
    }

    void Start()
    {
       stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float current_pos = -(stageDimensions.x + 3);
        float width = 0;
        do {
            GameObject elem = GenerateElementAt(current_pos);
            rail.Add(elem);
            width = elem.GetComponent<SpriteRenderer>().bounds.size.x;
           current_pos += width + (spaceBetween + NoiseSpaceBetween);
        } while (current_pos < stageDimensions.x + width + 3);
            
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Move(Direction.Left);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move(Direction.Right);
        }
    }

    private GameObject GenerateElementAt(float pos)
    {
        int id = Random.Range(1, elementLibray.Count);
        GameObject element = GameObject.Instantiate(elementLibray[id-1], new Vector3(pos, elementLibray[id - 1].transform.position.y, depth), Quaternion.identity) as GameObject;
        element.GetComponent<SpriteRenderer>().color = colorShift;
        if (Random.value < 0.5f)
            element.GetComponent<SpriteRenderer>().flipX = true;
        return element;
    }

    public void Move(float direction)
    {
        rail.RemoveAll(delegate (GameObject o) { return o == null; });
        for (int i = rail.Count-1; i >= 0; i--) {
            GameObject obj = rail[i];
            if (obj == null)
                continue;
            float move_width = speed * Time.deltaTime * direction;
            obj.transform.Translate(new Vector3(move_width, 0, 0));
            float spriteWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x;

            if (direction == Direction.Left && obj.transform.position.x + spriteWidth < -(stageDimensions.x + spaceBetween + NoiseSpaceBetween))             //moving left
            {
                GameObject last = GetDistalElem(Direction.Right);
                spriteWidth = last.GetComponent<SpriteRenderer>().bounds.size.x;
                rail.Add(GenerateElementAt(last.transform.position.x + spriteWidth + spaceBetween + move_width + NoiseSpaceBetween));
                rail.Remove(rail[i]);
                Destroy(obj);
            }
            else if (direction == Direction.Right && obj.transform.position.x - spriteWidth > (stageDimensions.x + spaceBetween + NoiseSpaceBetween))            //moving right
            {
                GameObject first = GetDistalElem(Direction.Left);
                spriteWidth = first.GetComponent<SpriteRenderer>().bounds.size.x;
                rail.Add(GenerateElementAt(first.transform.position.x - (spriteWidth + spaceBetween + NoiseSpaceBetween - move_width)));
                rail.Remove(rail[i]);
                Destroy(obj);
            }
        }
    }

    GameObject GetDistalElem(float dir)
    {
        GameObject ret = null;
        float distal_pos = 0;
        foreach (GameObject obj in rail) {
            if (dir == Direction.Right && obj.transform.position.x > distal_pos) {
                distal_pos = obj.transform.position.x;
                ret = obj;
            }
            else if (dir == Direction.Left && obj.transform.position.x < distal_pos)
            {
                distal_pos = obj.transform.position.x;
                ret = obj;
            }
        }
        return ret;
    }
}
