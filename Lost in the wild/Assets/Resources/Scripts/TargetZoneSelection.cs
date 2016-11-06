using UnityEngine;
using System.Collections;

public class TargetZoneSelection : MonoBehaviour
{

    void Awake()
    {
        SubscribeToEvents();
    }

    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetWidth(0.2f, 0.2f);
        lineRenderer.SetVertexCount(2);
    }

    void Update()
    {
        if (GameManager.instance.CurrentState == GameState.TARGET_ZONE_SELECTION &&
            MapManager.instance.TargetCell)
        {
            transform.position = MapManager.instance.TargetCell.transform.position;
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, MapManager.instance.CurrentCell.transform.position);
            lineRenderer.SetPosition(1, MapManager.instance.TargetCell.transform.position);
        }

    }

    //Events & callbacks

    void SubscribeToEvents()
    {
        EventManager.OnMouseLeft += this.OnMouseLeft;
    }

    void OnMouseLeft()
    {
        if (GameManager.instance.CurrentState == GameState.TARGET_ZONE_SELECTION)
        {
            EventManager.TargetZoneSelected();
        }
    }
}
