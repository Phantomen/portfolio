using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotTurnChild : MonoBehaviour {


    private GameObject parentGameObject;
	// Use this for initialization
	void Start () {
        parentGameObject = transform.parent.gameObject;
        Quaternion rotation = Quaternion.Euler(-parentGameObject.transform.rotation.x, -parentGameObject.transform.rotation.y, -parentGameObject.transform.rotation.z);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float newRotX = parentGameObject.transform.localRotation.x;
        float newRotY = parentGameObject.transform.localRotation.y;
        float newRotZ = parentGameObject.transform.localRotation.z;

        newRotZ = newRotZ / 2;

        Quaternion newRotation = new Quaternion();
        newRotation.Set(-newRotX, -newRotY, -newRotZ, parentGameObject.transform.rotation.w);

        //Quaternion newRotation = Quaternion.Euler(-parentGameObject.transform.rotation.x, -parentGameObject.transform.rotation.y, -parentGameObject.transform.rotation.z);
        //Quaternion newRotation = Quaternion.Euler(0, 0, 0);
        transform.localRotation = newRotation;
        //Debug.Log("Parent: " + parentGameObject.transform.rotation.eulerAngles + "\n newRotation: " + -parentGameObject.transform.rotation.eulerAngles);
        Debug.Log("Parent: " + parentGameObject.transform.localRotation.eulerAngles + "\n newRotation: " + newRotation.eulerAngles);
        //Debug.Log("Parent: " + -parentGameObject.transform.rotation.eulerAngles.z + "\n newRotation: " + newRotation.eulerAngles.z);
    }
}
