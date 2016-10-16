using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public Animator anim;
	
	// Update is called once per frame
	void Update () {
        float speed = Input.GetAxis("Horizontal");
        if (speed > 0.5f)
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else if (speed < -0.5f)
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        anim.SetFloat("walk", Mathf.Abs(speed));
    }
}
