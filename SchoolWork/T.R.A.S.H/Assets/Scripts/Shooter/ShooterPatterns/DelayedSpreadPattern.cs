using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSpreadPattern : MonoBehaviour {

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bossObject;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private float fireRate = 1;
    private float shootTimer = 0;
    private float tempTimer = 0;

    void Awake()
    {

    }

    // Use this for initialization
    void Start ()
    {
               
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tempTimer += Time.deltaTime;
        shootTimer += Time.deltaTime * fireRate;

        if (shootTimer >= 1)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 bulletPosition = new Vector3(bossObject.transform.position.x, bossObject.transform.position.y, bossObject.transform.position.z);
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
        shootTimer = 0;
    }
}
