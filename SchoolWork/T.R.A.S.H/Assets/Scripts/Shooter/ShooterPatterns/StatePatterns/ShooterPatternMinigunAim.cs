using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPatternMinigunAim : ShooterPattern {

    [Tooltip("What type of bullet the gameobject will shoot")]
    [SerializeField]
    private GameObject bulletPrefab;


    private GameObject player;


    //[Tooltip("List of shooterPoints")]
    //[SerializeField]
    private List<Transform> bulletSpawnPosition = new List<Transform>();


    public enum LerpType
    {
        Constant = 0,
        Lerp,
        Slerp
    }


    [Tooltip("Should it turn with a constant speed, or speed down the closer it is")]
    [SerializeField]
    private LerpType lerpType;

    [Tooltip("At the beginning of a new wave, aim directly at the player")]
    [SerializeField]
    private bool instantlyTurnOnNewWave = false;
    private bool instantTurn = false;

    [Tooltip("Should it aim at player at the start?")]
    [SerializeField]
    private bool aimAtPlayerAtStart = false;

    [Tooltip("Turn during the delay between waves")]
    [SerializeField]
    private bool turnDuringDelay = false;


    [Tooltip("The turnrate at which the shooting point turns")]
    [SerializeField]
    private float turnrate = 45;

    [Tooltip("The distance from the center that the bullets spawn from")]
    [SerializeField]
    private float distanceFromCenter = 0;

    [Tooltip("The duration the wave will have, unless it has not fired all bullets")]
    [SerializeField]
    private float timePerWave = 1;

    [Tooltip("The delay between waves")]
    [SerializeField]
    private float delayBetweenWaves = 1;


    [Tooltip("Number of bullets per wave")]
    [SerializeField]
    private int bulletsPerWave = 10;

    private float delayBetweenBullets = 0;
    private int currentBullet = 0;

    [Tooltip("How long the bullet lives")]
    [SerializeField]
    private float bulletLifeTime = 10;

    [Tooltip("Start delay")]
    [SerializeField]
    private float startDelay = 0;



    private Timer currentDelay;


    public override void Shoot(GameObject shooterGameObject)
    {
        currentDelay.Time += Time.deltaTime;

        

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

            ShootPattern(shooterGameObject);
        }

        //else if turn during delay is true, turn
        else if (turnDuringDelay == true
            && currentDelay.Duration != delayBetweenBullets)
        {
            RotateSpawnPoints();
        }

        //Else if you are not shooting, and the duration is the delay between bullets, Turn
        else if (timePerWave > 0
            && currentDelay.Duration == delayBetweenBullets)
        {
            RotateSpawnPoints();
        }
    }


    private void ShootPattern(GameObject shooterGameObject)
    {
        if (currentDelay.Duration != delayBetweenBullets)
        {
            currentDelay.Time -= currentDelay.Duration - delayBetweenBullets;

            currentDelay.Duration = delayBetweenBullets;
        }

        for (int i = 0; i < bulletSpawnPosition.Count; i++)
        {
            RotateSpawnPoints();

            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPosition[i].position, bulletSpawnPosition[i].rotation);
            bullet.transform.position += distanceFromCenter * Vector3.forward;

            Destroy(bullet, bulletLifeTime);
        }

        currentDelay.Time -= timePerWave / bulletsPerWave;

        currentBullet++;

        if (currentBullet >= bulletsPerWave)
        {
            currentBullet = 0;

            currentDelay.Duration = delayBetweenWaves;

            if (delayBetweenWaves <= 0)
            {
                currentDelay.Duration = delayBetweenBullets;
            }

            if (instantlyTurnOnNewWave == true)
            {
                instantTurn = true;
            }
        }
    }


    private void RotateSpawnPoints()
    {
        for (int i = 0; i < bulletSpawnPosition.Count; i++)
        {
            //Relative aim point
            Vector2 targetDir = bulletSpawnPosition[i].position - player.transform.position;

            //Target rotation is rotation it needs to have to aim directly at it
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.forward);
            targetRotation.x = 0;
            targetRotation.y = 0;

            Quaternion newRotation = targetRotation;

            //Current rotation
            Quaternion fromRotation = bulletSpawnPosition[i].rotation;
            fromRotation.x = 0;
            fromRotation.y = 0;



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



            else if (lerpType == LerpType.Lerp)
            {
                newRotation = Quaternion.Lerp(fromRotation, targetRotation, Time.deltaTime * turnrate * Mathf.PI / 180);
            }

            else if (lerpType == LerpType.Slerp)
            {
                newRotation = Quaternion.Slerp(fromRotation, targetRotation, Time.deltaTime * turnrate * Mathf.PI / 180);
            }

            //rotate the spawnPoint
            bulletSpawnPosition[i].transform.rotation = newRotation;
        }
    }


    private float GetConstantAngle(float currentRotZ, float targetRotation, int horizontal)
    {
        //the target rotation for constant angle rotation
        float targetRotZ = currentRotZ + horizontal * turnrate * Time.deltaTime;

        //if over turned, aim directly at the player
        if ((targetRotZ > targetRotation
            && horizontal > 0)
            || (targetRotZ < targetRotation
            && horizontal < 0))
        {
            targetRotZ = targetRotation;
        }

        return targetRotZ;


        //targetRotZ = currentRotZ + turningSpeed * Time.deltaTime;

        //if (targetRotZ < targetRotation.eulerAngles.z
        //    || targetRotZ > currentRotZ)
        //{
        //    targetRotZ = targetRotation.eulerAngles.z;
        //}

        //newRotation.eulerAngles = new Vector3(0, 0, targetRotZ);
    }

    private float GetConstantAngleSwitch(float currentRotZ, float targetRotation, int horizontal)
    {
        //the target rotation for constant angle rotation
        float targetRotZ = currentRotZ + horizontal * turnrate * Time.deltaTime;

        //If rotation is over or 360, subtract 360 and turning right
        if (targetRotZ >= 360 && horizontal > 0)
        {
            targetRotZ -= 360;
        }

        //else if rotation is less than 0, add 360 and turning left
        else if (targetRotZ <= 0 && horizontal < 0)
        {
            targetRotZ += 360;
        }

        //if over turned, aim directly at the player
        if ((targetRotZ > targetRotation
            && horizontal > 0
            && targetRotZ < targetRotation)
            || (targetRotZ < targetRotation
            && horizontal < 0
            && targetRotZ > targetRotation))
        {
            targetRotZ = targetRotation;
        }

        return targetRotZ;
    }


    //public override void Reset()
    //{

    //}

    public override void Reset(GameObject shooterGameObject)
    {

    }

    public override void Reset(GameObject shooterGameObject, List<Transform> bulletSpawnList)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //currentTimeInWave = new Timer(timerPerWave, 0);
        currentDelay = new Timer(startDelay, 0);

        bulletSpawnPosition = bulletSpawnList;

        //If it does not have spawnpoint, set it as the own
        if (bulletSpawnPosition.Count == 0)
        {
            bulletSpawnPosition.Add(shooterGameObject.transform);
        }

        for (int i = 0; i < bulletSpawnPosition.Count; i++)
        {
            if (bulletSpawnPosition[i] == null)
            {
                bulletSpawnPosition[i] = shooterGameObject.transform;
            }
        }

        if (aimAtPlayerAtStart == true)
        {
            for (int i = 0; i < bulletSpawnPosition.Count; i++)
            {
                Vector2 targetDir = bulletSpawnPosition[i].position - player.transform.position;
                bulletSpawnPosition[i].rotation = Quaternion.LookRotation(targetDir, Vector3.forward);
            }
        }

        delayBetweenBullets = timePerWave / (float)bulletsPerWave;
    }
}
