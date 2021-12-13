using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementSideTest : MonoBehaviour {

    public GameObject bulletPrefab;

    //ShootingPattern will be the abstract class for all the bullet patterns
    //public ShootingPattern shootpattern;


    public float speed = 1;

    public float middleStopYPosition = 5;
    //public float middleYPosAdd = 5;

    
    //[HideInInspector] public 


    private Transform spawnPosition;

	// Use this for initialization
	void Start () {
        spawnPosition = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
