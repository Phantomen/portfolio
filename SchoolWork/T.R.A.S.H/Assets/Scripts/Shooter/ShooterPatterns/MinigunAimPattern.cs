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
    //public float lerpSpeed = 0.2f;

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

        if (atStartAimedAtPlayer == true)
        {
            for (int i = 0; i < bulletSpawnPosition.Count; i++)
            {
                Vector2 targetDir = bulletSpawnPosition[i].position - player.transform.position;
                bulletSpawnPosition[i].rotation = Quaternion.LookRotation(targetDir, Vector3.forward);
            }
        }

        delayBetweenBullets = timerPerWave / (float)bulletsPerWave;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
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


    //private void Shoot()
    //{
    //    if (currentDelay.Duration != timerPerWave / bulletsPerWave)
    //    {
    //        currentDelay.Duration = timerPerWave / bulletsPerWave;
    //    }

    //    for (int i = 0; i < bulletSpawnPosition.Count; i++)
    //    {
    //        Vector2 targetDir = bulletSpawnPosition[i].position - player.transform.position;

    //        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.forward);
    //        targetRotation.x = 0;
    //        targetRotation.y = 0;

    //        Quaternion newRotation = targetRotation;

    //        Quaternion fromRotation = bulletSpawnPosition[i].rotation;
    //        fromRotation.x = 0;
    //        fromRotation.y = 0;



    //        if (lerpType == LerpType.Constant)
    //        {
    //            //Debug.Log("Error, No constant speed code set");

    //            float targetRotZ = targetRotation.eulerAngles.z;
    //            float currentRotZ = fromRotation.eulerAngles.z;


    //            //If same side
    //            //+
    //            if (targetRotZ >= currentRotZ
    //                && ((targetRotZ >= 180 && currentRotZ >= 180)
    //                || (targetRotZ <= 180 && currentRotZ <= 180)))
    //            {
    //                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, 1));
    //            }

    //            //-
    //            else if (targetRotZ <= currentRotZ
    //                && ((targetRotZ >= 180 && currentRotZ >= 180)
    //                || (targetRotZ <= 180 && currentRotZ <= 180)))
    //            {
    //                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, -1));
    //            }


    //            //If from left side to right side
    //            //+
    //            else if (targetRotZ <= currentRotZ + 180
    //                && targetRotZ >= 180 && currentRotZ <= 180)
    //            {
    //                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, 1));
    //            }

    //            //-
    //            else if (targetRotZ >= currentRotZ + 180
    //                && targetRotZ >= 180 && currentRotZ <= 180)
    //            {
    //                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngleSwitch(currentRotZ, targetRotation.eulerAngles.z, -1));
    //            }



    //            //If from right side to left side
    //            //+
    //            else if (targetRotZ <= currentRotZ - 180
    //                && targetRotZ <= 180 && currentRotZ >= 180)
    //            {
    //                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngleSwitch(currentRotZ, targetRotation.eulerAngles.z, 1));
    //            }

    //            //-
    //            else if (targetRotZ >= currentRotZ - 180
    //                && targetRotZ <= 180 && currentRotZ >= 180)
    //            {
    //                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, -1));
    //            }
    //        }



    //        else if (lerpType == LerpType.Lerp)
    //        {
    //            newRotation = Quaternion.Lerp(fromRotation, targetRotation, Time.deltaTime * turningSpeed * Mathf.PI / 180);
    //        }

    //        else if (lerpType == LerpType.Slerp)
    //        {
    //            newRotation = Quaternion.Slerp(fromRotation, targetRotation, Time.deltaTime * turningSpeed * Mathf.PI / 180);
    //        }

    //        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPosition[i].position, newRotation);

    //        bulletSpawnPosition[i].transform.rotation = newRotation;

    //        //oldRotation = newRotation;

    //        Destroy(bullet, destroyBulletTime);
    //    }

    //    currentDelay.Time -= timerPerWave / bulletsPerWave;

    //    currentBullet++;

    //    if (currentBullet >= bulletsPerWave)
    //    {
    //        currentBullet = 0;

    //        currentDelay.Duration = delayBetweenWaves;

    //        if (delayBetweenWaves <= 0)
    //        {
    //            currentDelay.Duration = timerPerWave / bulletsPerWave;
    //        }

    //        if (InstantTurnOnStartOfNewWave == true)
    //        {
    //            instantTurn = true;
    //        }
    //    }
    //}


    private void Shoot()
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

            Destroy(bullet, destroyBulletTime);
        }

        currentDelay.Time -= timerPerWave / bulletsPerWave;

        currentBullet++;

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
                newRotation = Quaternion.Lerp(fromRotation, targetRotation, Time.deltaTime * turningSpeed * Mathf.PI / 180);
            }

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
        float targetRotZ = currentRotZ + horizontal * turningSpeed * Time.deltaTime;

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
}
