using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnShooterOrigin : MonoBehaviour {

    private GameObject parentGameObject;

    // Use this for initialization
    void Start()
    {
        parentGameObject = transform.parent.parent.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (parentGameObject != null)
        {
            //Säg åt shooter att uppdatera rotationen innan du skjuter
            transform.rotation = Quaternion.Euler(0, 0, transform.parent.rotation.x * 180);
        }
    }
}
