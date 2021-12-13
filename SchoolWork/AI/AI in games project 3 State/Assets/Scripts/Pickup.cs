using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    void OnTriggerEnter(Collider otherCol)
    {
        if(otherCol.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}
