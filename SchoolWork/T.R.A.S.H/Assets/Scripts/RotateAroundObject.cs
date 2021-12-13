using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour {
    [Tooltip("The Gameobject that this object will rotate around")]
    //[SerializeField] private GameObject centerPoint;
    public GameObject centerPoint;
    [Tooltip("The rotation speed that the object will have")]
    [SerializeField] private float rotateSpeed = 100f;

    [Tooltip("The rotation speed that the object will have")]
    [SerializeField]
    private Vector2 orbitOffset = new Vector2();


    // Use this for initialization
    void Start () {
        Vector3 offsetposition = transform.position + new Vector3(orbitOffset.x, orbitOffset.y, 0);
        transform.position = offsetposition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.RotateAround(centerPoint.transform.position + new Vector3(orbitOffset.x, orbitOffset.y, 0), centerPoint.transform.forward, rotateSpeed * Time.deltaTime);
    }
}
