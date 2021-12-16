using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterWavePattern : MonoBehaviour {
    public float bulletLifeTime = 5;

    public Transform bulletSpawnPosition;

    public GameObject bulletPrefab;

    public float spawnDistanceFromCenter = 0;

    //t.ex. 45 degrees on each side of the center   /\
    public float degreesFromCenter = 45;

    public bool startHeadingRight = true;
    private int horizontal = 0;
    public bool changeWaveDirectionEveryWave = false;

    //Own script if start from center!?!
    public bool startFromCenter = false;

    //Slows down at the ends and speeds up towards center
    //Cos if not startFromCenter sin if from center?
    public bool smoothWaveTurning = true;

    //Per minute
    private float timeUntilNextShot = 0f;

    //back to starting point from same side
    public int numberOfWaves = 2;
    private int currentWave = 1;
    //Seconds
    public float timePerWave = 2;

    public int bulletsPerWave = 20;     //+ bullet at the end of the wave
    private int currentBulletsShotInWave = 0;

    public float delayBetweenWaves = 0f;
    private float currentDelay = 0; 


    private float tempCurrentTime = 0f;




    Timer zigZagTimer = new Timer();


    // Use this for initialization
    void Start ()
    {
        if (bulletSpawnPosition == null)
        {
            bulletSpawnPosition = transform;
        }

        if (startHeadingRight == true)
        {
            horizontal = 1;
        }
        else
        {
            horizontal = -1;
        }

        zigZagTimer.Set(timePerWave / 2, 0);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //If it's not the last wave and last bullet was fired
        if ((currentBulletsShotInWave == bulletsPerWave && currentWave < numberOfWaves))
        {
            currentDelay -= Time.deltaTime;

            //After delay start the next wave
            if (currentDelay <= 0)
            {
                currentWave++;
                currentBulletsShotInWave = 0;
                tempCurrentTime = 0;
                timeUntilNextShot = 0;

                if (changeWaveDirectionEveryWave == true)
                {
                    horizontal = -horizontal;
                }
            }
        }

        //If not max bullets, edit timers
        if (currentBulletsShotInWave < bulletsPerWave)
        {
            tempCurrentTime += Time.deltaTime;
            timeUntilNextShot -= Time.deltaTime;

            //Shoots if timer expierd
            if (timeUntilNextShot <= 0 && currentWave <= numberOfWaves)
            {
                Shoot();
            }
        }

        //Shots last bullet to make the shootin symmetrical
        else if (currentBulletsShotInWave == bulletsPerWave)
        {
            timeUntilNextShot -= Time.deltaTime;
            if (timeUntilNextShot <= 0)
            {
                ShootEndBullet();
            }
        }
	}

    private void Shoot()
    {
        //Bullet rotation
        Quaternion rotation;

        if (smoothWaveTurning == true)
        {
            rotation = Quaternion.Euler(0, 0, WaveMotion());
        }

        else
        {
            rotation = Quaternion.Euler(0, 0, ZigZagMotion());
        }

        //Spawn bullet
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation * rotation);

        Destroy(bullet.gameObject, bulletLifeTime);

        currentBulletsShotInWave++;
        timeUntilNextShot += timePerWave / bulletsPerWave;

        if (currentBulletsShotInWave == bulletsPerWave)
        {
            currentDelay = delayBetweenWaves;
        }
    }

    private void ShootEndBullet()
    {
        tempCurrentTime = Time.deltaTime;
        if (currentWave == numberOfWaves)
        {
            Quaternion rotation;

            if (smoothWaveTurning == true)
            {
                rotation = Quaternion.Euler(0, 0, WaveMotion());
            }

            else
            {
                rotation = Quaternion.Euler(0, 0, ZigZagMotion());
            }

            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation * rotation);

            Destroy(bullet.gameObject, bulletLifeTime);
            currentBulletsShotInWave++;
        }

        //currentWave++;
        //tempCurrentTime = 0;
        //currentDelay = delayBetweenWaves;
    }

    //Smooth wave
    private float WaveMotion()
    {
        float degrees = 0f;
        if (startFromCenter == true)
        {
            degrees = -horizontal * Mathf.Sin((tempCurrentTime - Time.deltaTime) * (2f / timePerWave) * Mathf.PI);
        }

        else
        {
            degrees = horizontal * Mathf.Cos((tempCurrentTime - Time.deltaTime) * (2f / timePerWave) * Mathf.PI);
        }

        degrees *= degreesFromCenter;

        return degrees;
    }

    //None smooth wave
    private float ZigZagMotion()
    {
        float degrees = 0f;

        if (startFromCenter == true)
        {
            zigZagTimer.Time += Time.deltaTime;

            //change direction
            if (zigZagTimer.Expired == true)
            {
                horizontal = -horizontal;
                zigZagTimer.Time -= timePerWave / 2;
            }

            degrees = -horizontal * Mathf.PingPong((tempCurrentTime - Time.deltaTime) * (4f / timePerWave) + 1, 2)
                + horizontal;
        }

        else
        {
            degrees = -horizontal * Mathf.PingPong((tempCurrentTime - Time.deltaTime) * (4f / timePerWave), 2)
                + horizontal;
        }

        degrees *= degreesFromCenter;
        return degrees;
    }
}
