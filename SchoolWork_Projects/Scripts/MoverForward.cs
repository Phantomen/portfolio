using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverForward : MonoBehaviour {

    [Tooltip("The amount of speed that the object will have (negative number is the other direction)")]
    public float speed = 1;

    private Rigidbody2D myRigidbody;


	// Use this for initialization
	void Start ()
    {
        //gets rigidbody component
        myRigidbody = transform.gameObject.GetComponent<Rigidbody2D>();
        //Sets objects velocity
        myRigidbody.velocity = transform.up * speed;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //makes sure that the object always moves "up" (relative to object)
        myRigidbody.velocity = transform.up * speed;
    }
}
