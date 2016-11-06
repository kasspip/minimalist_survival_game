using UnityEngine;
using System.Collections;

public enum ZoneType
{
    NONE,
    LAKE,
    MOUNTAIN,
    FOREST,
    PLAIN
};

public class HexCell : MonoBehaviour
{
    public ZoneType type = ZoneType.PLAIN;

    void OnMouseEnter()
    {
        MapManager.instance.TargetCell = this;
    }

    void OnMouseDown()
    {
        EventManager.TileClicked(this);
    }
}