using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotTurnChild : MonoBehaviour {


    private GameObject parentGameObject;
	// Use this for initialization
	void Start () {
        parentGameObject = transform.parent.gameObject;
        if (parentGameObject != null)
        {
            Quaternion rotation = Quaternion.Euler(-parentGameObject.transform.rotation.x, -parentGameObject.transform.rotation.y, -parentGameObject.transform.rotation.z);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //if it has a parent gameObject
        if (parentGameObject != null)
        {
            float newRotX = parentGameObject.transform.localRotation.x;
            float newRotY = parentGameObject.transform.localRotation.y;
            float newRotZ = parentGameObject.transform.localRotation.z;

            newRotZ = newRotZ / 2;

            Quaternion newRotation = new Quaternion();
            //Sets new rotation to negative rotation of parent
            newRotation.Set(-newRotX, -newRotY, -newRotZ, parentGameObject.transform.rotation.w);

            //Sets new rotation
            transform.localRotation = newRotation;
        }
    }
}
