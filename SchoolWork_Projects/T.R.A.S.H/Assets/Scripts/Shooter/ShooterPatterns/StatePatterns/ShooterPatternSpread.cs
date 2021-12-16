using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPatternSpread : ShooterPattern
{

    public int numberOfWaves = 1;
    private int currentWave = 1;


    public List<SpreadShot> spreadShotList = new List<SpreadShot>();




    private int listIndex = 0;



    private Timer currentDelay;


    public float startDelay = 0;



    // Use this for initialization
    //public override void Start()
    //{
    //    //Sets so it shoots in the next update
    //    currentDelay = new Timer(startDelay, 0);

    //    //If it does not have spawnpoint, set it as the own
    //    for (int i = 0; i < spreadShotList.Count; i++)
    //    {
    //        if (spreadShotList[i].bulletSpawnPosition.Count == 0)
    //        {
    //            spreadShotList[i].bulletSpawnPosition.Add(transform);
    //        }

    //        else
    //        {
    //            for (int spawnIndex = 0; spawnIndex < spreadShotList[i].bulletSpawnPosition.Count; spawnIndex++)
    //            {
    //                if (spreadShotList[i].bulletSpawnPosition[spawnIndex] == null)
    //                {
    //                    spreadShotList[i].bulletSpawnPosition[spawnIndex] = transform;
    //                }
    //            }
    //        }
    //    }
    //}

    // Update is called once per frame
    public override void Shoot(GameObject shooterGameObject)
    {
        if (currentWave <= numberOfWaves)
        {
            currentDelay.Time += Time.deltaTime;

            if (currentDelay.Expired == true)
            {
                ShootPattern(shooterGameObject);
            }
        }
    }


    private void ShootPattern(GameObject shooterGameObject)
    {
        if (currentDelay.Duration != spreadShotList[listIndex].delayBetweenShots)
        {
            currentDelay.Duration = spreadShotList[listIndex].delayBetweenShots;
        }


        for (int i = 0; i < spreadShotList[listIndex].bulletsPerShot; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, spreadShotList[listIndex].GetAngle(i));

            for (int spawnIndex = 0; spawnIndex < spreadShotList[listIndex].bulletSpawnPosition.Count; spawnIndex++)
            {
                var bullet = (GameObject)Instantiate(spreadShotList[listIndex].bulletPrefab,
                    spreadShotList[listIndex].bulletSpawnPosition[spawnIndex].position + shooterGameObject.transform.position,
                    spreadShotList[listIndex].bulletSpawnPosition[spawnIndex].rotation * rotation * shooterGameObject.transform.rotation);

                bullet.transform.position += bullet.transform.up * spreadShotList[listIndex].spawnDistanceFromCenter;

                Destroy(bullet.gameObject, spreadShotList[listIndex].bulletLifeTime);
            }

        }

        currentDelay.Time = 0;

        spreadShotList[listIndex].shotIndex++;

        if (spreadShotList[listIndex].shotIndex >= spreadShotList[listIndex].shotNumber)
        {
            NextSpread();
        }
    }


    private void NextSpread()
    {
        spreadShotList[listIndex].shotIndex = 0;
        listIndex++;

        if (listIndex == spreadShotList.Count)
        {
            listIndex = 0;
            currentWave++;
        }

        currentDelay.Duration = spreadShotList[listIndex].delayForFirstShot;
    }

    //public override void Reset()
    //{
    //    //Sets so it shoots in the next update
    //    currentDelay = new Timer(startDelay, 0);

    //    //If it does not have spawnpoint, set it as the own
    //    for (int i = 0; i < spreadShotList.Count; i++)
    //    {
    //        if (spreadShotList[i].bulletSpawnPosition.Count == 0)
    //        {
    //            spreadShotList[i].bulletSpawnPosition.Add(transform);
    //        }

    //        else
    //        {
    //            for (int spawnIndex = 0; spawnIndex < spreadShotList[i].bulletSpawnPosition.Count; spawnIndex++)
    //            {
    //                if (spreadShotList[i].bulletSpawnPosition[spawnIndex] == null)
    //                {
    //                    spreadShotList[i].bulletSpawnPosition[spawnIndex] = transform;
    //                }
    //            }
    //        }
    //    }

    //    for (int i = 0; i < spreadShotList.Count; i++)
    //    {
    //        spreadShotList[i].shotIndex = 0;
    //        listIndex = 0;
    //        currentWave = 1;
    //    }
    //}

    public override void Reset(GameObject shooterGameObject)
    {

    }

    public override void Reset(GameObject shooterGameObject, List<Transform> bulletSpawnList)
    {
        //Sets so it shoots in the next update
        currentDelay = new Timer(startDelay, 0);

        //If it does not have spawnpoint, set it as the own
        for (int i = 0; i < spreadShotList.Count; i++)
        {
            if (spreadShotList[i].bulletSpawnPosition.Count == 0)
            {
                spreadShotList[i].bulletSpawnPosition.Add(transform);
            }

            else
            {
                for (int spawnIndex = 0; spawnIndex < spreadShotList[i].bulletSpawnPosition.Count; spawnIndex++)
                {
                    if (spreadShotList[i].bulletSpawnPosition[spawnIndex] == null)
                    {
                        spreadShotList[i].bulletSpawnPosition[spawnIndex] = transform;
                    }
                }
            }
        }

        for (int i = 0; i < spreadShotList.Count; i++)
        {
            spreadShotList[i].shotIndex = 0;
            listIndex = 0;
            currentWave = 1;
        }
    }
}



[System.Serializable]
public class SpreadShot
{
    public GameObject bulletPrefab;

    public List<Transform> bulletSpawnPosition = new List<Transform>();
    public float spawnDistanceFromCenter = 0;


    public float degreesFromCenter = 45;

    public int bulletsPerShot = 10;
    public int shotNumber = 1;
    public float delayBetweenShots = 1;

    public float delayForFirstShot = 0;


    public float bulletLifeTime = 5;

    [HideInInspector]
    public int shotIndex = 0;


    public float GetAngle(int bulletIndex)
    {
        float degrees = (-degreesFromCenter / 2) + (bulletIndex) * (degreesFromCenter / (bulletsPerShot - 1));
        return degrees * 2;
    }
}
