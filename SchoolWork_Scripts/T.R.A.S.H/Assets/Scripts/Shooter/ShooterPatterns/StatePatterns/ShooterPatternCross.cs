using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPatternCross : ShooterPattern
{

    [Tooltip("What type of bullet the gameobject will shoot")]
    [SerializeField]
    private GameObject bulletPrefab;

    [Tooltip("Amount of bullets to be shot every time the object can shoot (split equally around the object so 4 is 1 bullet at 0 degrees, 1 at 90, 1 at 180 and 1 at 270)")]
    [SerializeField]
    private int bulletsPerWave = 4;

    [Tooltip("Amount of bullets the gameobject will shoot every second")]
    [SerializeField]
    private float fireRate = 1;

    [Tooltip("Amount of seconds before the bullet will get destroyed")]
    [SerializeField]
    private float destroyTimer = 5f;

    [Tooltip("Degrees per second in which the bullets will rotate out of the gameobject (0 for no rotation)")]
    [SerializeField]
    private float rotateSpeed = 0f;

    [Tooltip("Distance from the center that the bullet will spawn from")]
    [SerializeField]
    private float distanceFromCenter = 0f;

    [Tooltip("")]
    [SerializeField]
    private float degreesOffset = 0;


    private float shootTimer = 0f;
    private float tempTimer = 0f;
    private float rotateTimer = 0f;
    private float actualRotation = 0f;



    public override void Shoot(GameObject shooterGameObject)
    {
        tempTimer += Time.deltaTime;
        shootTimer += Time.deltaTime * fireRate;
        rotateTimer += Time.deltaTime * rotateSpeed;

        if (shootTimer >= 1)
        {
            ShootPattern(shooterGameObject);
        }
    }


    private void ShootPattern(GameObject shooterGameObject)
    {
        for (int i = 0; i < bulletsPerWave + 1; i++)
        {
            actualRotation += /*Graderna på en cirkel, 360 är ju en hel cirkel*/ (360 / bulletsPerWave);
            Quaternion rotation = Quaternion.Euler(0, 0, actualRotation + rotateTimer);
            Vector3 bulletPosition = new Vector3(shooterGameObject.transform.position.x, shooterGameObject.transform.position.y, shooterGameObject.transform.position.z);
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletPosition, rotation);

            Destroy(bullet, destroyTimer);

            bullet.transform.position += bullet.transform.up * distanceFromCenter;
        }

        shootTimer -= 1;
    }


    public override void Reset(GameObject shooterGameObject)
    {

    }

    public override void Reset(GameObject shooterGameObject, List<Transform> bulletSpawnList)
    {

    }
}


