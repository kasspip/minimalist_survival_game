using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum GameState
{
    NONE,
    PAUSE,
    START_MENU,
    GAME_MENU,
    MAP,
    TARGET_ZONE_SELECTION,
    WORLD
};

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    GameState currentState = GameState.NONE;
    GameState previousState = GameState.NONE;

    public GameState CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        SubscribeToEvents();

    }

    void Start()
    {
        if (this.CurrentState == GameState.NONE)
            EventManager.ChangeState(GameState.START_MENU);
    }

    void Update()
    {

    }

    //Events & callbacks

    void SubscribeToEvents()
    {
        EventManager.OnStateChange += this.OnStateChange;
        EventManager.OnStartMenu += this.OnStartMenu;
        EventManager.OnMap += this.OnMap;
        EventManager.OnWorld += this.OnWorld;
        EventManager.OnGamePaused += this.OnGamePaused;
        EventManager.OnGameResumed += this.OnGameResumed;
        EventManager.OnPressEscape += this.OnPressEscape;
        EventManager.OnTargetZoneStart += this.OnTargetZoneStart;
        EventManager.OnTargetZoneSelected += this.OnTargetZoneSelected;
    }

    void OnTargetZoneSelected()
    {
        currentState = previousState;
    }

    void OnTargetZoneStart()
    {
        previousState = currentState;
        currentState = GameState.TARGET_ZONE_SELECTION;
    }

    void OnStateChange(GameState state)
    {
    }

    void OnStartMenu()
    {
        previousState = currentState;
        currentState = GameState.START_MENU;
    }

    void OnMap()
    {
        previousState = currentState;
        currentState = GameState.MAP;
    }

    void OnWorld()
    {
        previousState = currentState;
        currentState = GameState.WORLD;
    }

    void OnGamePaused()
    {
        previousState = currentState;
        currentState = GameState.PAUSE;
    }

    void OnGameResumed()
    {
        currentState = previousState;
    }

    void OnPressEscape()
    {
        if (CurrentState != GameState.START_MENU)
        {
            if (CurrentState != GameState.PAUSE)
                EventManager.PauseGame();
            else
                EventManager.ResumeGame();
        }
    }
}
