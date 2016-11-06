using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    //delegates
    public delegate void SimpleDelegate();
    public delegate void StateDelegate(GameState state);
    public delegate void TileDelegate(HexCell tile);


    //callbacks events
    public static event SimpleDelegate OnStartMenu;
    public static event SimpleDelegate OnGameMenu;
    public static event SimpleDelegate OnMap;
    public static event SimpleDelegate OnWorld;
    public static event SimpleDelegate OnGamePaused;
    public static event SimpleDelegate OnGameResumed;
    public static event SimpleDelegate OnPressEscape;
    public static event SimpleDelegate OnPressRight;
    public static event SimpleDelegate OnPressLeft;
    public static event SimpleDelegate OnMouseLeft;
    public static event SimpleDelegate OnTargetZoneStart;
    public static event SimpleDelegate OnTargetZoneSelected;
    public static event StateDelegate OnStateChange;
    public static event TileDelegate OnTileClicked;

    //public event triggers
    public static void ChangeState(GameState state)
    {

        if (OnStateChange != null)
        {
            OnStateChange(state);
        }
        switch (state)
        {
            case GameState.START_MENU: OnStartMenu(); break;
            case GameState.GAME_MENU: OnGameMenu(); break;
            case GameState.MAP: OnMap(); break;
            case GameState.WORLD: OnWorld(); break;
            case GameState.TARGET_ZONE_SELECTION: OnTargetZoneStart(); break;
        }
    }

    public static void PressEscape() {
        if (OnPressEscape != null)
            OnPressEscape();
    }

    public static void PressRight()
    {
        if (OnPressRight != null)
            OnPressRight();
    }

    public static void PressLeft()
    {
        if (OnPressLeft != null)
            OnPressLeft();
    }

    public static void MouseLeft()
    {
        if (OnMouseLeft != null)
            OnMouseLeft();
    }

    public static void TileClicked(HexCell tile)
    {
        if (OnTileClicked != null)
            OnTileClicked(tile);
    }

    public static void PauseGame()
    {
        if (OnGamePaused != null)
            OnGamePaused();
    }

    public static void ResumeGame()
    {
        if (OnGameResumed != null)
            OnGameResumed();
    }

    public static void SartTargetZone()
    {
        if (OnTargetZoneStart != null)
            OnTargetZoneStart();
    }

    public static void TargetZoneSelected()
    {
        if (OnTargetZoneSelected != null)
            OnTargetZoneSelected();
    }
}
