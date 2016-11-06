using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Panel
{
    START_MENU,
    GAME_MENU,
    MAP,
    WORLD
};

public class UIManager : MonoBehaviour {

    public Text infoText = null;
    public Text mapInfoTextTitle = null;
    public Text mapInfoTextType = null;
    public GameObject mapButtonSelectTarget = null;
    public GameObject panelStartMenu;
    public GameObject panelGameMenu;
    public GameObject panelMap;
    public GameObject panelWorld;

    Dictionary<Panel, GameObject> UIMapping = new Dictionary<Panel, GameObject>();
    
    void Awake()
    {
        SetupUIMapping();
        SubscribeToEvents();
    }

    void Start()
    {

    }

    void Update()
    {
        if (MapManager.instance.TargetCell)
        {
            mapInfoTextTitle.text = "Zone " + MapManager.instance.TargetCell.name;
            mapInfoTextType.text = MapManager.instance.TargetCell.type.ToString();
        }
        infoText.text = GameManager.instance.CurrentState.ToString();
    }

    void SetupUIMapping()
    {
        UIMapping.Add(Panel.START_MENU, panelStartMenu);
        UIMapping.Add(Panel.GAME_MENU, panelGameMenu);
        UIMapping.Add(Panel.MAP, panelMap);
        UIMapping.Add(Panel.WORLD, panelWorld);
    }

    void ShowPanel(Panel panel)
    {
        UIMapping[panel].SetActive(true);
    }

    void HidePanel(Panel panel)
    {
        UIMapping[panel].SetActive(false);
    }

    void HideAllPanels()
    {
        foreach (KeyValuePair<Panel, GameObject> entry in UIMapping)
            entry.Value.SetActive(false);
    }

    //Buttons

    public void BoutonNewGame()
    {
        EventManager.ChangeState(GameState.MAP);
    }

    public void BoutonStartMenu()
    {
        HideAllPanels();
        EventManager.ChangeState(GameState.START_MENU);
    }

    public void BoutonResume()
    {
        HidePanel(Panel.GAME_MENU);
    }

    public void BoutonMap()
    {
        HideAllPanels();
        EventManager.ChangeState(GameState.MAP);
        if (!MapManager.instance.TargetCell)
            mapButtonSelectTarget.SetActive(true);
        else
            mapButtonSelectTarget.SetActive(false);

    }

    public void BoutonWorld()
    {
        HideAllPanels();
        EventManager.ChangeState(GameState.WORLD);
    }

    public void BoutonSelectTargetZone()
    {
        EventManager.SartTargetZone();
    }

    //Events & callbacks

    void SubscribeToEvents()
    {
        EventManager.OnStateChange += this.OnStateChange;
        EventManager.OnStartMenu += this.OnStartMenu;
        EventManager.OnGameMenu += this.OnGameMenu;
        EventManager.OnMap += this.OnMap;
        EventManager.OnWorld += this.OnWorld;
        EventManager.OnPressEscape += this.OnPressEscape;
        EventManager.OnTargetZoneStart += this.OnTargetZoneStart;
    }

    void OnTargetZoneStart()
    {
        mapButtonSelectTarget.SetActive(false);
    }

    void OnStateChange(GameState state)
    {
        if (state != GameState.GAME_MENU)
            HideAllPanels();
    }

    void OnStartMenu()
    {
        UIMapping[Panel.START_MENU].SetActive(true);
    }

    void OnGameMenu()
    {
        UIMapping[Panel.GAME_MENU].SetActive(true);
    }

    void OnMap()
    {
        UIMapping[Panel.MAP].SetActive(true);
    }

    void OnWorld()
    {
        UIMapping[Panel.WORLD].SetActive(true);
    }

    void OnPressEscape()
    {
        if (GameManager.instance.CurrentState != GameState.START_MENU)
        {
            if (UIMapping[Panel.GAME_MENU].activeSelf)
                HidePanel(Panel.GAME_MENU);
            else
                ShowPanel(Panel.GAME_MENU);
        }
    }
}
