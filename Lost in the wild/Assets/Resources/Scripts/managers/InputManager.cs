using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.PressEscape();
        }
        if (Input.GetAxis("Horizontal") > 0.5f)
        {
            EventManager.PressRight();
        }
        if (Input.GetAxis("Horizontal") < -0.5f)
        {
            EventManager.PressLeft();
        }
        if (Input.GetMouseButton(0))
        {
            EventManager.MouseLeft();
        }
    }
}
