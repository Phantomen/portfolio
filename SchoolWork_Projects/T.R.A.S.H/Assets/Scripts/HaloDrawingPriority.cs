using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloDrawingPriority : MonoBehaviour {

    private Component halo;

	// Use this for initialization
	void Start () {
        halo = gameObject.GetComponent("Light");
        halo.GetComponent<Renderer>().sortingLayerName = "Player";
        halo.GetComponent<Renderer>().sortingLayerID = 0;
        halo.GetComponent<Renderer>().sortingOrder = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
