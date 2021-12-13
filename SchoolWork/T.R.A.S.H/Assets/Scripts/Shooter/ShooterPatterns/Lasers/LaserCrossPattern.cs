using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCrossPattern : MonoBehaviour {
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Vector3 distanceFromCenter;
    [SerializeField] private int laserAmount = 4;
    [SerializeField] private int angleOffset = 360;
    [SerializeField] private float startingRotation = 0;
    [SerializeField] private float maxSize;
    [SerializeField] private float growthSpeed;
    [SerializeField] private bool EnableRotation = false;
    [SerializeField] private bool startRotatingRight = true;
    [SerializeField] private float timePerPause = 3f;
    [SerializeField] private float laserStopTimer = 2f;
    [SerializeField] private float turnSpeed = 3f;
    GameObject newLaser1;
    GameObject newLaser2;
    GameObject newLaser3;
    GameObject newLaser4;
    Vector3 currentPos;
    List<GameObject> laserList = new List<GameObject>();
    float extractTimer;
    float tempCurrentTime = 0f;
    float laserPauseTimer = 0f;
    bool laserSpawned = false;
    bool timerGo;
    int angle = 0;


    // Use this for initialization
    void Start()
    {
        Laser();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        extractTimer += Time.deltaTime * growthSpeed;

        if (timerGo)
        {
            laserPauseTimer += Time.deltaTime;
            tempCurrentTime += Time.deltaTime;
        }

        if (laserPauseTimer >= timePerPause && EnableRotation)
        {
            if (laserStopTimer <= 0)
            {
                laserPauseTimer = 0;
                laserStopTimer = 2f;
            }
            else
            {
                for(int i = 0; i < laserAmount; i++)
                {
                    rotateLaser(laserList[i], false);
                    retractLaser(laserList[i]);
                }

                laserStopTimer -= Time.deltaTime;
            }
        }
        else
        {
            for (int i = 0; i < laserAmount; i++)
            {
                extractLaser(laserList[i]);
            }

            if (EnableRotation)
            {
                for (int i = 0; i < laserAmount; i++)
                {
                    rotateLaser(laserList[i], true);
                }
            }
        }
    }

    void Laser()
    {
        for (int i = 0; i < laserAmount; i++)
        {
            angle += angleOffset / laserAmount;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 laserPosition = new Vector3(this.transform.position.x + distanceFromCenter.x, this.transform.position.y + distanceFromCenter.y, this.transform.position.z + distanceFromCenter.z);
            var laser = (GameObject)Instantiate(laserPrefab, laserPosition, rotation);
            laserList.Add(laser);
        }
        angle = 0;
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
                float degrees = Time.deltaTime * turnSpeed;
                Quaternion rotZ = new Quaternion();
                rotZ.eulerAngles = new Vector3(0, 0, laser.transform.rotation.eulerAngles.z + degrees);
                laser.transform.rotation = rotZ;
            }
            else
            {
                timerGo = true;
                float degrees = Time.deltaTime * turnSpeed;
                Quaternion rotZ = new Quaternion();
                rotZ.eulerAngles = new Vector3(0, 0, laser.transform.rotation.eulerAngles.z - degrees);
                laser.transform.rotation = rotZ;
            }
        }
        else
        {
            timerGo = false;
        }
    }
}
