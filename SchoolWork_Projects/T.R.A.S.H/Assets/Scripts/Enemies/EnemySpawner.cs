using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;

    public float timeBetweenSpawns = 1;
    private Timer currentDelay;


    public float safeDestroyTime = 120;


	// Use this for initialization
	void Start ()
    {
        currentDelay = new Timer(timeBetweenSpawns, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (currentDelay.Expired == true)
        {
            currentDelay.Time -= timeBetweenSpawns;

            var enemy = (GameObject)Instantiate(enemyPrefab,
                    transform.position,
                    transform.rotation);



            Destroy(enemy.gameObject, safeDestroyTime);
        }
	}
}
