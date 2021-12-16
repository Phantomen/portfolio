using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagTriggerRandomShooter : MonoBehaviour {

    private bool triggered = false;

    [Tooltip("The objects to spawn")]
    [SerializeField]
    private List<GameObject> spawnObjectPrefab = new List<GameObject>();


    [Tooltip("Bullets fired per second")]
    [SerializeField]
    private float firerate = 1f;


    [Tooltip("Delay before it starts spawning")]
    [SerializeField]
    private float startSpawnDelay = 0f;

    private Timer currentDelay;


    [Tooltip("The tags that triggers the shooter")]
    [SerializeField]
    private string[] triggerTags;


    [Tooltip("The random X-position from spawnObject")]
    [SerializeField]
    private float xLocalPositionLimitOffset;

    [Tooltip("The random Y-position from spawnObject")]
    [SerializeField]
    private float yLocalPositionLimitOffset;


    [Tooltip("The random degree gets lower at the closer to the end points in the ")]
    [SerializeField]
    private bool degreeChangeCloserToEndPoints = true;

    [Tooltip("Max degrees")]
    [SerializeField]
    private float maxDegreesFromCenter = 45;

    [Tooltip("Min degrees at end point")]
    [SerializeField]
    private float minDegreesFromCenter = 0;


    [Tooltip("Time until destroyed")]
    [SerializeField]
    private float destroyTime = 5;

    private bool currentlySpawning;



    // Use this for initialization
    void Start()
    {
        currentDelay = new Timer(startSpawnDelay, 0);

        xLocalPositionLimitOffset = Mathf.Clamp(xLocalPositionLimitOffset, 0, float.MaxValue);
        maxDegreesFromCenter = Mathf.Clamp(maxDegreesFromCenter, 0, 360);
        minDegreesFromCenter = Mathf.Clamp(minDegreesFromCenter, -360, 360);
        firerate = Mathf.Clamp(firerate, float.MinValue, float.MaxValue);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xLocalPositionLimitOffset = Mathf.Clamp(xLocalPositionLimitOffset, 0, float.MaxValue);
        maxDegreesFromCenter = Mathf.Clamp(maxDegreesFromCenter, 0, 360);
        minDegreesFromCenter = Mathf.Clamp(minDegreesFromCenter, -360, 360);
        firerate = Mathf.Clamp(firerate, float.MinValue, float.MaxValue);

        //If it has been triggered, shoot
        if (triggered == true)
        {
            currentDelay.Time += Time.deltaTime;

            while (currentDelay.Expired == true)
            {
                currentDelay.Time -= currentDelay.Duration;

                if (currentDelay.Duration != (1 / firerate))
                {
                    currentDelay.Duration = 1 / firerate;
                }

                Spawn();
            }
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnObjectPrefab.Count; i++)
        {
            if (spawnObjectPrefab[i] != null)
            {
                //position offset
                float xPositionOffset = Random.Range(-xLocalPositionLimitOffset, xLocalPositionLimitOffset);
                float yPositionOffset = Random.Range(-yLocalPositionLimitOffset, yLocalPositionLimitOffset);

                //Base rotation of bullet
                float newRot = Random.Range(-maxDegreesFromCenter, maxDegreesFromCenter);

                if (degreeChangeCloserToEndPoints == true && xLocalPositionLimitOffset > 0)
                {
                    if (xPositionOffset > 0)
                    {
                        //The multiplication
                        float multX = xPositionOffset / xLocalPositionLimitOffset;

                        //The the degree that has changed (it gets closer to minDegreesFromSenter the closer multX is to 1)
                        float degreeChange = (minDegreesFromCenter * multX) - (maxDegreesFromCenter * (1 - multX));

                        //The new rotation
                        newRot = Random.Range(degreeChange, maxDegreesFromCenter);
                    }

                    else if (xPositionOffset < 0)
                    {
                        float multX = xPositionOffset / -xLocalPositionLimitOffset;

                        float degreeChange = (minDegreesFromCenter * multX) - (maxDegreesFromCenter * (1 - multX));

                        newRot = -1 * Random.Range(degreeChange, maxDegreesFromCenter);
                    }
                }

                //Shoot the bullet
                ShootBullet(new Vector2(xPositionOffset, yPositionOffset),
                    Quaternion.Euler(0, 0, newRot),
                    spawnObjectPrefab[i]);
            }
        }
    }

    //Shoots the bullet
    private void ShootBullet(Vector2 offset, Quaternion newRotation, GameObject bulletPrefab)
    {
        Vector3 newPos = transform.position + new Vector3(offset.x, offset.y, 0);
        var bullet = (GameObject)Instantiate(bulletPrefab, newPos, newRotation * transform.rotation);

        Destroy(bullet, destroyTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If it has't been triggered
        if (triggered == false)
        {
            //If it collided with a gameobject that has the right tag, trigger it
            for (int i = 0; i < triggerTags.Length; i++)
            {
                if (collision.tag == triggerTags[i])
                {
                    triggered = true;
                    break;
                }
            }
        }
    }
}
