using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFollowPattern : MonoBehaviour {
    [SerializeField] private GameObject warningLaser;
    [SerializeField] private GameObject activeLaser;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Vector3 distanceFromCenter;
    [SerializeField] private float laserReadyTimer;
    [SerializeField] private float laserLoadUpTimer;
    [SerializeField] private float laserStayActiveTimer;
    [SerializeField] private float laserMaxSize;
    [SerializeField] private float laserGrowthSpeed;
    [SerializeField] private AudioClip warningLaserSound;
    [SerializeField] private AudioClip activeLaserSound;
    [SerializeField] private AudioClip windDownLaserSound;

    List<GameObject> laserList = new List<GameObject>();
    //GameObject newLaser;
    //GameObject activeNewLaser;
    bool warningLaserSpawned = false;
    bool activeLaserSpawned = false;
    bool laserReady = false;
    bool extractLaser = true;

    float tempTimerHolderReadyTimer;
    float tempTimerHolderLoadUpTimer;
    float tempTimerHolderStayActiveTimer;

    // Use this for initialization
    void Start() {
        CreateLaser(warningLaser);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        tempTimerHolderReadyTimer = laserReadyTimer;
        tempTimerHolderLoadUpTimer = laserLoadUpTimer;
        tempTimerHolderStayActiveTimer = laserStayActiveTimer;
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        if(!laserReady)
        {
            laserList[0].GetComponent<AudioSource>().clip = warningLaserSound;
            if (!laserList[0].GetComponent<AudioSource>().isPlaying)
            {
                laserList[0].GetComponent<AudioSource>().Play();
            }
            laserList[0].transform.up = -playerObject.transform.position + laserList[0].transform.position;
            laserReadyTimer -= Time.deltaTime;
            if(laserReadyTimer <= 0)
            {
                warningLaser.GetComponent<AudioSource>().Stop();
                laserReady = true;
            }
        }
        else
        {
            if(laserLoadUpTimer <= 0)
            {
                laserStayActiveTimer -= Time.deltaTime;
                if (!activeLaserSpawned)
                {
                    Vector3 laserPosition = new Vector3(this.transform.position.x + distanceFromCenter.x, this.transform.position.y + distanceFromCenter.y, this.transform.position.z + distanceFromCenter.z);
                    var laser = (GameObject)Instantiate(activeLaser, laserPosition, laserList[0].transform.rotation);
                    laserList.Add(laser);
                    //activeNewLaser = laser;
                    laserList[1].GetComponent<AudioSource>().clip = activeLaserSound;
                    laserList[1].GetComponent<AudioSource>().Play();
                    activeLaserSpawned = true;
                }

                if (laserList[1].transform.localScale.x <= laserMaxSize && extractLaser)
                {
                    laserList[1].transform.localScale = new Vector3(laserList[1].transform.localScale.x + (Time.deltaTime * laserGrowthSpeed), 1, 1);
                }
                else
                {
                    extractLaser = false;
                }

                if(laserStayActiveTimer <= 0)
                {
                    laserList[1].GetComponent<AudioSource>().clip = windDownLaserSound;
                    if (laserList[1].transform.localScale.x >= 0.01)
                    {
                        if (!laserList[1].GetComponent<AudioSource>().isPlaying)
                        {
                            laserList[1].GetComponent<AudioSource>().Play();
                        }
                        //for (int i = 0; i < activeNewLaser.GetComponent<AudioSource>().time; i++)
                        //{
                        //    activeNewLaser.GetComponent<AudioSource>().pitch -= Time.deltaTime;
                        //}
                        laserList[1].transform.localScale = new Vector3(laserList[1].transform.localScale.x - (Time.deltaTime * laserGrowthSpeed), 1, 1);
                    }
                    else
                    {
                        //activeNewLaser.GetComponent<AudioSource>().Stop();
                        Destroy(laserList[1]);
                        laserList.Remove(laserList[1]);
                        laserReadyTimer = tempTimerHolderReadyTimer;
                        laserLoadUpTimer = tempTimerHolderLoadUpTimer;
                        laserStayActiveTimer = tempTimerHolderStayActiveTimer;
                        extractLaser = true;
                        activeLaserSpawned = false;
                        laserReady = false;
                    }
                }
            }
            else
            {
                laserLoadUpTimer -= Time.deltaTime;
            }
        }
    }

    void CreateLaser(GameObject laserObject)
    { 
        Vector3 laserPosition = new Vector3(this.gameObject.transform.position.x + distanceFromCenter.x, this.gameObject.transform.position.y + distanceFromCenter.y, this.gameObject.transform.position.z + distanceFromCenter.z);
        var laser = (GameObject)Instantiate(laserObject, laserPosition, Quaternion.identity);
        laserList.Add(laser);
        //newLaser = laser;
        warningLaserSpawned = true;
    }
}
