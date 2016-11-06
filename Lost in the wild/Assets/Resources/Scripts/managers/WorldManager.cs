using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

    public GameObject landscape = null;
    public GameObject weather = null;
    public GameObject man = null;

    void Awake () {
        SubscribeToEvents();
    }

    void DisplayWorld()
    {
        landscape.SetActive(true);
        weather.SetActive(true);
        man.SetActive(true);
    }

    void HideWorld()
    {
        landscape.SetActive(false);
        weather.SetActive(false);
        man.SetActive(false);
    }

    //Events & callbacks

    void SubscribeToEvents()
    {
        EventManager.OnStateChange += this.OnStateChange;
        EventManager.OnWorld += this.OnWorld;
    }

    void OnStateChange(GameState state)
    {
        if (state != GameState.GAME_MENU || 
            state != GameState.WORLD)
            HideWorld();
    }

    void OnWorld()
    {
        DisplayWorld();
        Camera.main.transform.position = new Vector3(0,0,-10);
    }
}
