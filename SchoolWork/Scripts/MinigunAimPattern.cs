using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunAimPattern : MonoBehaviour {

    private GameObject player;

    public GameObject bulletPrefab;


    public enum LerpType
    {
        Constant = 0,
        Lerp,
        Slerp
    }

    public LerpType lerpType;

    public bool InstantTurnOnStartOfNewWave = false;
    private bool instantTurn = false;
    public bool atStartAimedAtPlayer = false;
    public bool continueToTurnDuringDelayBetweenWaves = false;

    public float turningSpeed = 0.2f;

    public float distanceFromCenter = 0;

    public float timerPerWave = 1;
    public float bulletsPerWave = 10;
    private float delayBetweenBullets;
    private int currentBullet = 0;
    public float delayBetweenWaves = 0;


    public float destroyBulletTime = 5;

    public float startDelay = 0;


    public List<Transform> bulletSpawnPosition = new List<Transform>();


    private Timer currentTimeInWave;
    private Timer currentDelay;


	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        currentTimeInWave = new Timer(timerPerWave, 0);
        currentDelay = new Timer(startDelay, 0);

        //If spawnposition is emty, add self
        if (bulletSpawnPosition.Count == 0)
        {
            bulletSpawnPosition.Add(transform);
        }

        //If it does not have spawnpoint, set it as the own
        for (int i = 0; i < bulletSpawnPosition.Count; i++)
        {
            if (bulletSpawnPosition[i] == null)
            {
                bulletSpawnPosition[i] = transform;
            }
        }

        //Aim directly at player at start
        if (atStartAimedAtPlayer == true)
        {
            for (int i = 0; i < bulletSpawnPosition.Count; i++)
            {
                Vector2 targetDir = bulletSpawnPosition[i].position - player.transform.position;
                bulletSpawnPosition[i].rotation = Quaternion.LookRotation(targetDir, Vector3.forward);
            }
        }

        //Sets delayBetweenBullets
        delayBetweenBullets = timerPerWave / (float)bulletsPerWave;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        currentDelay.Time += Time.deltaTime;

        //If time has expired
        if (currentDelay.Expired == true)
        {
            //If instantTurn is true, that means that this wave is a new one and instantTurnStartOfWaves == true
            if (instantTurn == true)
            {
                instantTurn = false;

                //Turn all points towars player
                for (int i = 0; i < bulletSpawnPosition.Count; i++)
                {
                    Vector2 targetDir = bulletSpawnPosition[i].position - player.transform.position;
                    bulletSpawnPosition[i].rotation = Quaternion.LookRotation(targetDir, Vector3.forward);
                }
            }

            Shoot();
        }

        //else if turn during delay is true, turn
        else if (continueToTurnDuringDelayBetweenWaves == true
            && currentDelay.Duration != delayBetweenBullets)
        {
            RotateSpawnPoints();
        }

        //Else if you are not shooting, and the duration is the delay between bullets, Turn
        else if (timerPerWave > 0
            && currentDelay.Duration == delayBetweenBullets)
        {
            RotateSpawnPoints();
        }
    }


    private void Shoot()
    {
        //If the delay isn't the delay, set it as the delay
        if (currentDelay.Duration != delayBetweenBullets)
        {
            currentDelay.Time -= currentDelay.Duration - delayBetweenBullets;

            currentDelay.Duration = delayBetweenBullets;
        }

        //Rotate the spawnpoints
        RotateSpawnPoints();

        //For each spawnpoint, spawn a bullet from them
        for (int i = 0; i < bulletSpawnPosition.Count; i++)
        {
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPosition[i].position, bulletSpawnPosition[i].rotation);
            bullet.transform.position += distanceFromCenter * Vector3.forward;

            Destroy(bullet, destroyBulletTime);
        }

        //Lowers the time
        currentDelay.Time -= timerPerWave / bulletsPerWave;

        //Adds a bullet
        currentBullet++;

        //If all bullets has been fired in the wave
        //Reset for next wave
        if (currentBullet >= bulletsPerWave)
        {
            currentBullet = 0;

            currentDelay.Duration = delayBetweenWaves;

            if (delayBetweenWaves <= 0)
            {
                currentDelay.Duration = delayBetweenBullets;
            }

            if (InstantTurnOnStartOfNewWave == true)
            {
                instantTurn = true;
            }
        }
    }


    //Rotate all the spawnPoints
    private void RotateSpawnPoints()
    {
        //For each spawnpoint
        for (int i = 0; i < bulletSpawnPosition.Count; i++)
        {
            //Relative aim point
            Vector2 targetDir = bulletSpawnPosition[i].position - player.transform.position;

            //Target rotation is rotation it needs to have to aim directly at it
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.forward);
            //Sets it so it rotates around the z axis
            targetRotation.x = 0;
            targetRotation.y = 0;

            Quaternion newRotation = targetRotation;

            //Current rotation
            Quaternion fromRotation = bulletSpawnPosition[i].rotation;
            fromRotation.x = 0;
            fromRotation.y = 0;


            //Constant turning
            if (lerpType == LerpType.Constant)
            {
                float targetRotZ = targetRotation.eulerAngles.z;
                float currentRotZ = fromRotation.eulerAngles.z;


                //If same side (left or right)
                //Increase rotation
                if (targetRotZ >= currentRotZ
                    && ((targetRotZ >= 180 && currentRotZ >= 180)
                    || (targetRotZ <= 180 && currentRotZ <= 180)))
                {
                    newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, 1));
                }

                //Decrease rotation
                else if (targetRotZ <= currentRotZ
                    && ((targetRotZ >= 180 && currentRotZ >= 180)
                    || (targetRotZ <= 180 && currentRotZ <= 180)))
                {
                    newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, -1));
                }


                //If from left side to right side
                //Increase rotation
                else if (targetRotZ <= currentRotZ + 180
                    && targetRotZ >= 180 && currentRotZ <= 180)
                {
                    newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, 1));
                }

                //Decrease rotation
                else if (targetRotZ >= currentRotZ + 180
                    && targetRotZ >= 180 && currentRotZ <= 180)
                {
                    newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngleSwitch(currentRotZ, targetRotation.eulerAngles.z, -1));
                }



                //If from right side to left side
                //Increase rotation
                else if (targetRotZ <= currentRotZ - 180
                    && targetRotZ <= 180 && currentRotZ >= 180)
                {
                    newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngleSwitch(currentRotZ, targetRotation.eulerAngles.z, 1));
                }

                //Decrease rotation
                else if (targetRotZ >= currentRotZ - 180
                    && targetRotZ <= 180 && currentRotZ >= 180)
                {
                    newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, -1));
                }
            }


            //Lerp rotation
            else if (lerpType == LerpType.Lerp)
            {
                newRotation = Quaternion.Lerp(fromRotation, targetRotation, Time.deltaTime * turningSpeed * Mathf.PI / 180);
            }

            //Slerp rotation
            else if (lerpType == LerpType.Slerp)
            {
                newRotation = Quaternion.Slerp(fromRotation, targetRotation, Time.deltaTime * turningSpeed * Mathf.PI / 180);
            }

            //rotate the spawnPoint
            bulletSpawnPosition[i].transform.rotation = newRotation;
        }
    }



    private float GetConstantAngle(float currentRotZ, float targetRotation, int horizontal)
    {
        //the target rotation for constant angle rotation
        float targetRotZ = currentRotZ + horizontal * turningSpeed * Time.deltaTime;

        //if over turned, aim directly at the player
        if ((targetRotZ > targetRotation
            && horizontal > 0)
            ||
            (targetRotZ < targetRotation
            && horizontal < 0))
        {
            targetRotZ = targetRotation;
        }

        return targetRotZ;
    }

    private float GetConstantAngleSwitch(float currentRotZ, float targetRotation, int horizontal)
    {
        //the target rotation for constant angle rotation
        float targetRotZ = currentRotZ + horizontal * turningSpeed * Time.deltaTime;

        //If rotation is or over 360 and turning right, subtract 360 
        if (targetRotZ >= 360 && horizontal > 0)
        {
            targetRotZ -= 360;
        }

        //else if rotation is or under 0 and turning left, add 360
        else if (targetRotZ <= 0 && horizontal < 0)
        {
            targetRotZ += 360;
        }

        //if over turned, aim directly at the player
        if ((targetRotZ > targetRotation
            && horizontal > 0
            && targetRotZ < targetRotation)
            ||
            (targetRotZ < targetRotation
            && horizontal < 0
            && targetRotZ > targetRotation))
        {
            targetRotZ = targetRotation;
        }

        return targetRotZ;
    }
}
