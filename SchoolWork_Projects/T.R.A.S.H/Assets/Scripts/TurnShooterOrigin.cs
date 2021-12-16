using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnShooterOrigin : MonoBehaviour {

    private GameObject parentGameObject;
    // Use this for initialization
    void Start()
    {
        parentGameObject = transform.parent.parent.gameObject;
        Debug.Log(parentGameObject.gameObject);
        //Quaternion rotation = Quaternion.Euler(transform.rotation.x, -parentGameObject.transform.rotation.y, transform.rotation.z);
        //transform.rotation.Set(transform.rotation.x, -parentGameObject.transform.rotation.y, parentGameObject.transform.rotation.y, 1);
        //transform.rotation.Set(transform.rotation.x, -parentGameObject.transform.rotation.y, parentGameObject.transform.rotation.x, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Quaternion rotation = Quaternion.Euler(transform.rotation.x, -parentGameObject.transform.rotation.y, parentGameObject.transform.rotation.y);
        //transform.rotation.Set(rotation);

        //transform.rotation.Set(transform.rotation.x, -parentGameObject.transform.rotation.y, parentGameObject.transform.rotation.y, 1);
        //transform.rotation.Set(transform.rotation.x, -parentGameObject.transform.rotation.y, parentGameObject.transform.rotation.x, 1);


        //Säg åt shooter att uppdatera rotationen innan du skjuter
        transform.rotation = Quaternion.Euler(0, 0, transform.parent.rotation.x * 180);
    }
}
