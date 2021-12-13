using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPattern : MonoBehaviour {

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Vector3 distanceFromCenter;
    [SerializeField] private float maxSize;
    [SerializeField] private float growthSpeed;
    [SerializeField] private bool EnableRotation = false;
    [SerializeField] private bool startRotatingRight = true;
    [SerializeField] private float timePerWave = 0f;
    [Tooltip("The lasers max and min degree that it will turn (45 means that it will ping pong between 45 degrees and -45 degrees")]
    [SerializeField] private float degreesFromCenter = 45f;
    [SerializeField] private float timePerPause = 3f;
    [SerializeField] private float laserStopTimer = 2f;
    GameObject newLaser;
    Vector3 currentPos;
    float extractTimer;
    float tempCurrentTime = 0f;
    float laserPauseTimer = 0f;
    bool laserSpawned = false;
    bool timerGo;

    float tempTimerHolder1;


    // Use this for initialization
    void Start () {
        Laser();
        tempTimerHolder1 = laserStopTimer;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        extractTimer += Time.deltaTime * growthSpeed;

        if (timerGo)
        {
            laserPauseTimer += Time.deltaTime;
            tempCurrentTime += Time.deltaTime;
        }

        if(laserPauseTimer >= timePerPause && EnableRotation)
        {
            if (laserStopTimer <= 0)
            {
                laserPauseTimer = 0;
                laserStopTimer = tempTimerHolder1;
            }
            else
            {
                rotateLaser(newLaser, false);
                retractLaser(newLaser);
                laserStopTimer -= Time.deltaTime;
            }
        } else
        {
            extractLaser(newLaser);
            if (EnableRotation)
            {
                rotateLaser(newLaser, true);
            }
        }
    }

    void Laser()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        Vector3 laserPosition = new Vector3(this.gameObject.transform.position.x + distanceFromCenter.x, this.gameObject.transform.position.y + distanceFromCenter.y, this.gameObject.transform.position.z + distanceFromCenter.z);
        var laser = (GameObject)Instantiate(laserPrefab, laserPosition, Quaternion.identity);
        newLaser = laser;
        laser.transform.localScale = new Vector3(1, 1, 1);
        laserSpawned = true;
    }

    void extractLaser(GameObject laser)
    {
        if (laser.transform.localScale.x <= maxSize)
        { 
            laser.transform.localScale = new Vector3(laser.transform.localScale.x + (Time.deltaTime * growthSpeed), 1, 1);
        }
    }

    void retractLaser(GameObject laser)
    {
        if (laser.transform.localScale.x >= 0.01)
        {
            laser.transform.localScale = new Vector3(laser.transform.localScale.x - (Time.deltaTime * growthSpeed), 1, 1);
        }
    }

    void rotateLaser(GameObject laser, bool willRotate)
    {
            if (willRotate)
            {
                if (startRotatingRight)
                {
                    timerGo = true;
                    float degrees = Mathf.Sin((tempCurrentTime - Time.deltaTime) * (2f / timePerWave) * Mathf.PI);
                    degrees *= -degreesFromCenter;
                    laser.transform.rotation = Quaternion.Euler(0, 0, degrees);
                }
                else
                {
                    timerGo = true;
                    float degrees = Mathf.Sin((tempCurrentTime - Time.deltaTime) * (2f / timePerWave) * Mathf.PI);
                    degrees *= degreesFromCenter;
                    laser.transform.rotation = Quaternion.Euler(0, 0, degrees);
                }
            }
            else
            {
                timerGo = false;
            }
    }
}
