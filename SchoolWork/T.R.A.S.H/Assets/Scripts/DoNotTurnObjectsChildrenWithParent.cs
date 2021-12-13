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
		for (int i = 0; i < objectsToTurnList.Count; i++)
        {
            if (objectsToTurnList[i] != null)
            {
                GameObject parentGameObject = objectsToTurnList[i].transform.parent.gameObject;

                if (parentGameObject != null)
                {
                    float newRotX = parentGameObject.transform.localRotation.eulerAngles.x;
                    float newRotY = parentGameObject.transform.localRotation.eulerAngles.y;
                    float newRotZ = parentGameObject.transform.localRotation.eulerAngles.z;

                    newRotX = -newRotX;
                    newRotY = -newRotY;
                    newRotZ = -newRotZ;

                    //Debug.Log(parentGameObject.transform.rotation.eulerAngles.z + "\n" + newRotation.eulerAngles.z);

                    //Aims directly, up does not depend on parent
                    //newRotZ = 0;
                    //Quaternion newRotation = Quaternion.Euler(newRotX, newRotY, newRotZ);

                    Quaternion newRotation = Quaternion.Euler(newRotX, newRotY, newRotZ);

                    objectsToTurnList[i].transform.localRotation = newRotation;
                }

                else { Debug.Log("Error: Object has no parent"); }
            }
        }
	}
}
