using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverForward : MonoBehaviour {

    [Tooltip("The amount of speed that the object will have (negative number is the other direction)")]
    public float speed = 1;

    private Rigidbody2D rigidbody;


	// Use this for initialization
	void Start ()
    {
        rigidbody = transform.gameObject.GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.up * speed;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        rigidbody.velocity = transform.up * speed;
    }
}
