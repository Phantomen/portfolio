using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotTurnObjectsChildrenWithParent : MonoBehaviour {

    public List<Transform> objectsToTurnList = new List<Transform>();

	// Use this for initialization
	void Start ()
    {
	    for (int i = 0; i < objectsToTurnList.Count; i++)
        {
            if (objectsToTurnList[i] == null)
            {
                objectsToTurnList.RemoveAt(i);
                i -= 1;
            }
        }
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //for each transform
		for (int i = 0; i < objectsToTurnList.Count; i++)
        {
            if (objectsToTurnList[i] != null)
            {
                GameObject parentGameObject = objectsToTurnList[i].transform.parent.gameObject;

                //If transform has a parent
                if (parentGameObject != null)
                {
                    //Takes parents local rotation
                    float newRotX = parentGameObject.transform.localRotation.eulerAngles.x;
                    float newRotY = parentGameObject.transform.localRotation.eulerAngles.y;
                    float newRotZ = parentGameObject.transform.localRotation.eulerAngles.z;

                    //Reverses rotation from parant
                    newRotX = -newRotX;
                    newRotY = -newRotY;
                    newRotZ = -newRotZ;

                    Quaternion newRotation = Quaternion.Euler(newRotX, newRotY, newRotZ);

                    //Sets own rotation to negative parent transformation
                    objectsToTurnList[i].transform.localRotation = newRotation;
                }

                else { Debug.Log("Error: Object has no parent"); }
            }
        }
	}
}
