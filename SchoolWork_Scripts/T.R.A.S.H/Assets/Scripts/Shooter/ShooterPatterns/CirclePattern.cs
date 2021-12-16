using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePattern : MonoBehaviour {

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject playerObject;
    [Tooltip("Will the object start shooting to the right or not")]
    [SerializeField] private bool Right = false;
    [Tooltip("Will the object change direction every 360 degrees")]
    [SerializeField] private bool changeEveryWave = false;
    [Tooltip("How fast the object will turn every second")]
    [SerializeField] private float timePerWave = 1;
    [Tooltip("The maximum amount of bullets the object will shoot")]
    [SerializeField] private int totalBullets = 250;
    [Tooltip("The maxiumu amount of bullets the object will shoot every 360 degrees")]
    [SerializeField] private int bulletsPerWave = 25;
    [Tooltip("How many seconds after the bullet have been created that it will be destroyed")]
    [SerializeField] private int bulletLifeTime = 10;
    //Variables not available in the inspector
    //Start
    private float shootTimer = 0;
    private float tempTime = 0;
    private int isRight = 1;
    private int shotsFired = 0;
    private int currentBulletsPerWave = 0;
    //End


	// Use this for initialization
	void Start () {
        if (!Right)
        {
            isRight = 1;
        }
        else
        {
            isRight = -1;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        tempTime += Time.deltaTime;
        shootTimer += Time.deltaTime;

        if (shootTimer >= timePerWave / bulletsPerWave)
        {
            if (shotsFired < bulletsPerWave)
            {
                if (changeEveryWave)
                {
                    if (currentBulletsPerWave < bulletsPerWave)
                    {
                        Shoot();
                        shootTimer = 0;
                    }
                    else
                    {
                        isRight = -isRight;
                        currentBulletsPerWave = 0;
                    }
                }
                else
                {
                    Shoot();
                    shootTimer = 0;
                }
            }
        }


	}

    private void Shoot()
    {
        //float degrees = -isRight * Mathf.PingPong((tempTime - Time.deltaTime) * (4f / timePerWave) + 1, 2)
        //    + isRight;

        //degrees *= 360;

        //Debug.Log(degrees);

        //Quaternion rotation = Quaternion.Euler(0, 0, degrees);


        Quaternion rotation = Quaternion.Euler(0, 0, (360 * tempTime) * isRight);
        Vector3 bulletPosition = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, playerObject.transform.position.z);
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletPosition, bulletPrefab.transform.rotation * rotation);
            shotsFired++;
            currentBulletsPerWave++;
            Destroy(bullet.gameObject, bulletLifeTime);
            //Debug.Log(shotsFired);
    }
}
