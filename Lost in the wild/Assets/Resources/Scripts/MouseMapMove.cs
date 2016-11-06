﻿using UnityEngine;
using System.Collections;

public class MouseMapMove : MonoBehaviour {

    private Vector3 ResetCamera;
    private Vector3 Origin;
    private Vector3 Diference;
    private bool Drag = false;

    void Start()
    {
        ResetCamera = Camera.main.transform.position;
    }

    void LateUpdate()
    {
        if (GameManager.instance.CurrentState == GameState.MAP && Input.GetMouseButton(0))
        {
            Diference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (Drag == false)
            {
                Drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            Drag = false;
        }
        if (Drag == true)
        {
            Camera.main.transform.position = Origin - Diference;
        }
    }
}

