using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour {
    [SerializeField] public Vector2 moveTo;
    [SerializeField] float Speed;
    [SerializeField] float moveDelay;
    [SerializeField] public bool willMove = false;
     

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (willMove)
        {
            if (moveDelay <= 0)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, moveTo, Time.deltaTime * Speed);
            }
            else
            {
                moveDelay -= Time.deltaTime;
            }
        }
	}
}
