using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
    private Rigidbody2D rBody;
	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Grav"))
        {
            rBody.gravityScale *= 2;
        }
        if (Input.GetButtonUp("Grav"))
        {
            rBody.gravityScale /= 2;
        }
	}
}
