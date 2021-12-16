using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour {

    [Tooltip("The selected backgrounds moving speed in the Y axis (both background objects must have the same speed)")]
    [SerializeField] private float scrollSpeed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 positionMove = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * scrollSpeed), transform.position.z);
        transform.position = positionMove;	
	}

    private void OnBecameInvisible()
    {
        transform.position = new Vector3(transform.position.x, 14.98f, transform.position.z);
    }
}
