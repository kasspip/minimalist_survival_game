using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public Animator anim;
	
    void Awake()
    {
        SubscribeToEvents();
    }

	// Update is called once per frame
	void Update () {
        if (GameManager.instance.CurrentState == GameState.PAUSE)
            return;
        float speed = Input.GetAxis("Horizontal");
        anim.SetFloat("walk", Mathf.Abs(speed));
    }

    void SubscribeToEvents()
    {
        EventManager.OnPressRight += this.OnPressRight;
        EventManager.OnPressLeft += this.OnPressLeft;
    }

    void OnPressRight()
    {
        if (GameManager.instance.CurrentState != GameState.PAUSE)
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    void OnPressLeft()
    {
        if (GameManager.instance.CurrentState != GameState.PAUSE)
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
    }
}
