using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPatternSpread : ShooterPattern
{
    [Tooltip("How many waves it will shoot")]
    [SerializeField]
    private int numberOfWaves = 1;
    private int currentWave = 1;


    [Tooltip("List of how it will shoot")]
    [SerializeField]
    private List<SpreadShot> spreadShotList = new List<SpreadShot>();


    private int listIndex = 0;



    private Timer currentDelay;


    [Tooltip("Delay before it starts fireing")]
    [SerializeField]
    private float startDelay = 0;


    public override void Shoot(GameObject shooterGameObject)
    {
        ClampValues();
        if (currentWave <= numberOfWaves)
        {
            currentDelay.Time += Time.deltaTime;

            if (currentDelay.Expired == true)
            {
                ShootPattern();
            }
        }
    }


    private void ShootPattern()
    {
        if (currentDelay.Duration != spreadShotList[listIndex].delayBetweenShots)
        {
            currentDelay.Time -= currentDelay.Duration;
            currentDelay.Duration = spreadShotList[listIndex].delayBetweenShots;
            currentDelay.Time += currentDelay.Duration;
        }


        for (int i = 0; i < spreadShotList[listIndex].bulletsPerShot; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, spreadShotList[listIndex].GetAngle(i));

            //Shoots everybullet as a spread shot
            for (int spawnIndex = 0; spawnIndex < spreadShotList[listIndex].bulletSpawnPosition.Count; spawnIndex++)
            {
                var bullet = (GameObject)Instantiate(spreadShotList[listIndex].bulletPrefab,
                    spreadShotList[listIndex].bulletSpawnPosition[spawnIndex].position,
                    spreadShotList[listIndex].bulletSpawnPosition[spawnIndex].rotation * rotation);

                bullet.transform.position += bullet.transform.up * spreadShotList[listIndex].spawnDistanceFromCenter;

                Destroy(bullet.gameObject, spreadShotList[listIndex].bulletLifeTime);
            }

        }

        currentDelay.Time -= currentDelay.Duration;

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
        currentDelay.Time = 0;
    }


    public override void Reset(GameObject shooterGameObject)
    {
        //Sets start delay
        currentDelay = new Timer(startDelay, 0);


        GameObjectsTransformList tl = shooterGameObject.GetComponent<GameObjectsTransformList>();

        for (int i = 0; i < spreadShotList.Count; i++)
        {
            spreadShotList[i].bulletSpawnPosition.Clear();

            //If there is bulletspawns and the transformlist is not empty
            if (spreadShotList[i].bulletSpawnList.Count > 0
                && tl.transformList.Count > 0
                && tl != null)
            {
                for (int pi = 0; pi < spreadShotList[i].bulletSpawnList.Count; pi++)
                {
                    for (int si = 0; si < spreadShotList[i].bulletSpawnList[pi].spawnPointIndex.Length; si++)
                    {
                        //If that transform isn't null
                        if (tl.transformList[spreadShotList[i].bulletSpawnList[pi].phaseIndex].bulletSpawnList[spreadShotList[i].bulletSpawnList[pi].spawnPointIndex[si]] != null)
                        {
                            bool alreadyInList = false;
                            //Checks if if that transform already is being used
                            for (int t = 0; t < spreadShotList[i].bulletSpawnPosition.Count; t++)
                            {
                                //If that transform is being used, break and set bool to true
                                if (spreadShotList[i].bulletSpawnPosition[t] == tl.transformList[spreadShotList[i].bulletSpawnList[pi].phaseIndex].bulletSpawnList[spreadShotList[i].bulletSpawnList[pi].spawnPointIndex[si]])
                                {
                                    alreadyInList = true;
                                    break;
                                }
                            }

                            //Add it if it isn't in the list
                            if (alreadyInList == false)
                            {
                                spreadShotList[i].bulletSpawnPosition.Add(tl.transformList[spreadShotList[i].bulletSpawnList[pi].phaseIndex].bulletSpawnList[spreadShotList[i].bulletSpawnList[pi].spawnPointIndex[si]]);
                            }
                        }
                    }
                }
            }

            if (spreadShotList[i].bulletSpawnPosition.Count == 0)
            {
                spreadShotList[i].bulletSpawnPosition.Add(shooterGameObject.transform);
            }
        }


        for (int i = 0; i < spreadShotList.Count; i++)
        {
            spreadShotList[i].shotIndex = 0;
            listIndex = 0;
            currentWave = 1;
        }

        ClampValues();
    }

    public override void ClampValues()
    {
        numberOfWaves = Mathf.Clamp(numberOfWaves, 1, int.MaxValue);
    }
}



[System.Serializable]
public class SpreadShot
{
    public GameObject bulletPrefab;

    public List<AttackActionBulletSpawnList> bulletSpawnList = new List<AttackActionBulletSpawnList>();

    [HideInInspector]
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
