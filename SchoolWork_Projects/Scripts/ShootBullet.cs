using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour {

    [Tooltip("List of the points the bullet will shoot from")]
    [SerializeField]
    private List<Transform> spawnList = new List<Transform>();

    [Tooltip("Bullet prefab")]
    [SerializeField]
    private GameObject bulletPrefab;

    [Tooltip("Number of bullets per second")]
    [SerializeField]
    private float fireRate = 1;

    [Tooltip("The distance from spawn it will travel when it spawns")]
    [SerializeField]
    private float bulletOffset = 0;

    [Tooltip("How long the bullet lives")]
    [SerializeField]
    private float destroyTime = 10;

    private Timer timer;



    // Use this for initialization
    void Start ()
    {
        timer = new Timer(1, 1);

		for (int i = 0; i < spawnList.Count; i++)
        {
            if (spawnList[i] == null)
            {
                spawnList.RemoveAt(i);
                i--;
            }
        }

        if (spawnList.Count == 0)
        {
            spawnList.Add(transform);
        }
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        timer.Time += Time.deltaTime * fireRate;

        if (fireRate <= 0)
        {
            Debug.Log("Error: Fire rate is 0 or negative for " + gameObject.name);
            fireRate -= Time.deltaTime * fireRate;
        }

        if (timer.Expired == true)
        {
            timer.Time -= 1;
            Shoot();
        }
	}

    //Shots the bullet
    private void Shoot()
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            if (spawnList[i] != null)
            {
                Vector3 bulletSpawnPosition = spawnList[i].position;
                bulletSpawnPosition += spawnList[i].up * bulletOffset;

                Quaternion bulletRotation = spawnList[i].rotation;

                var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPosition, bulletRotation);

                Destroy(bullet, destroyTime);
            }

            else
            {
                spawnList.RemoveAt(i);
                i--;
            }
        }
    }
}
