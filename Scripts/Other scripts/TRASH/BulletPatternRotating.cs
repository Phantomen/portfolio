using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletPatternRotating : MonoBehaviour {

    public float degreesPerSecond = 45;

    public bool turningRight = true;

    private int horizontal = 1;

    // Use this for initialization
    void Start () {
		if (turningRight == true)
        {
            horizontal = -1;
        }
        else
        {
            horizontal = 1;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Turn();
	}


    private void Turn()
    {
        //The bullet rotates either to the right or left a certain degrees
        transform.rotation *= Quaternion.Euler(0, 0, degreesPerSecond * Time.deltaTime * horizontal);
    }
}
