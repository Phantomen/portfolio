using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterCrossPattern : MonoBehaviour {

    [Tooltip("What type of bullet the gameobject will shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [Tooltip("Select the gameobject in which this script exists")]
    [SerializeField] private GameObject bossObject;
    [Tooltip("Select the player")]
    [SerializeField] private GameObject playerObject;
    [Tooltip("Amount of bullets to be shot every time the object can shoot (split equally around the object so 4 is 1 bullet at 0 degrees, 1 at 90, 1 at 180 and 1 at 270)")]
    [SerializeField] private int bulletsPerWave = 4;
    [Tooltip("Amount of bullets the gameobject will shoot every second")]
    [SerializeField] private float fireRate = 1;
    [Tooltip("Amount of seconds before the bullet will get destroyed")]
    [SerializeField] private float destroyTimer = 5f;
    [Tooltip("Degrees per second in which the bullets will rotate out of the gameobject (0 for no rotation)")]
    [SerializeField] private float rotateSpeed = 0f;
    private float shootTimer = 0f;
    private float tempTimer = 0f;
    private float rotateTimer = 0f;
    private float actualRotation = 0f;
    //private float rotationkebab = 0f;
    //private float tempRotateSpeed = 0f;
    //private float tempFireRate = 0f;
    //GameObject temp;

    //List<GameObject> bulletList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        //tempRotateSpeed = rotateSpeed;
        //tempFireRate = fireRate;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        tempTimer += Time.deltaTime;
        shootTimer += Time.deltaTime * fireRate;
        rotateTimer += Time.deltaTime * rotateSpeed;

        if (shootTimer >= 1)
        {
            Shoot();
        }

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    for (int i = 0; i < bulletList.Count; i++)
        //    {
        //        if (bulletList[i] != null)
        //        {
        //            bulletList[i].GetComponent<MoverForward>().speed = -2;
        //            playerObject.GetComponent<PlayerMovement>().speed = 3;
        //            bossObject.GetComponent<TemporaryBossMove>().movementSpeed = 0.5f;
        //            rotateSpeed = tempRotateSpeed / 3;
        //            fireRate = tempFireRate / 3;
        //        }
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < bulletList.Count; i++)
        //    {
        //        if (bulletList[i] != null)
        //        {
        //            bulletList[i].GetComponent<MoverForward>().speed = -6;
        //            playerObject.GetComponent<PlayerMovement>().speed = 6;
        //            bossObject.GetComponent<TemporaryBossMove>().movementSpeed = 2.0f;
        //            rotateSpeed = tempRotateSpeed;
        //            fireRate = tempFireRate;
        //        }
        //    }
        //}
    }

    void Shoot()
    {
        for (int i = 0; i < bulletsPerWave + 1; i++)
        {
            actualRotation += /*Graderna på en cirkel, 360 är ju en hel cirkel*/ (360 / bulletsPerWave);
            Quaternion rotation = Quaternion.Euler(0, 0, actualRotation + rotateTimer);
            Vector3 bulletPosition = new Vector3(bossObject.transform.position.x, bossObject.transform.position.y, bossObject.transform.position.z);
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletPosition, rotation);           
        }
        //Quaternion rotation1 = Quaternion.Euler(0, 0, bulletPrefab.transform.rotation.z +)
        //Vector3 bulletPosition = new Vector3(bossObject.transform.position.x, bossObject.transform.position.y, bossObject.transform.position.z);
        //var bullet = (GameObject)Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
        shootTimer = 0;
        
    }

}
