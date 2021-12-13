using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Spawn")]
public class SpawnAction : StateAction {

	public List<SpawnMinionClass> spawnList = new List<SpawnMinionClass>();

    public List<SpawnMinionClass> orbitSpawnList = new List<SpawnMinionClass>();

    public List<SpawnMinionClass> pickUpList = new List<SpawnMinionClass>();

	public override void Act (StateController controller)
	{
		SpawnMinion(controller);
        SpawnOrbitMinion(controller);
        SpawnPickup(controller);
	}

    public override void Reset(StateController controller)
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            spawnList[i].currentTime = 0;
            spawnList[i].currentDelay = spawnList[i].spawnStartDelay;
            spawnList[i].currentSpawnCount = 0;
        }

        for (int i = 0; i < orbitSpawnList.Count; i++)
        {
            orbitSpawnList[i].currentTime = 0;
            orbitSpawnList[i].currentDelay = orbitSpawnList[i].spawnStartDelay;
            orbitSpawnList[i].currentSpawnCount = 0;
        }

        for (int i = 0; i < pickUpList.Count; i++)
        {
            pickUpList[i].currentTime = 0;
            pickUpList[i].currentDelay = pickUpList[i].spawnStartDelay;
            pickUpList[i].currentSpawnCount = 0;
        }
    }


    private void SpawnMinion(StateController controller)
	{
		for (int i = 0; i < spawnList.Count; i++)
		{

			spawnList [i].currentTime += Time.deltaTime;

			if (spawnList [i].currentDelay < 0)
			{
				spawnList [i].currentDelay = spawnList [i].spawnStartDelay;
			}

			if (spawnList [i].currentTime >= spawnList [i].currentDelay
				&& spawnList [i].currentSpawnCount < spawnList [i].numberOfSpawns)
			{
				spawnList [i].currentTime -= spawnList [i].currentDelay;
				spawnList [i].currentSpawnCount++;

                for (int m = 0; m < spawnList[i].minionPrefabs.Count; m++)
                {
                    if (spawnList[i].minionPrefabs[m] != null)
                    {
                        GameObject minion; //= (GameObject)Instantiate(spawnList[i].minionPrefabs[m]);

                        if (spawnList[i].spawnFromObject == true)
                        {
                            //minion.transform.position = controller.transform.position;
                            minion = (GameObject)Instantiate(spawnList[i].minionPrefabs[m], controller.transform);
                            Debug.Log(minion.transform.position);
                        }

                        else
                        {
                            minion = (GameObject)Instantiate(spawnList[i].minionPrefabs[m]);
                        }

                        if (spawnList[i].destroySpawnedAfterTime == true)
                        {
                            Destroy(minion, spawnList[i].lifeTime);
                        }

                        controller.minionList.Add(minion);
                    }

                    else
                    {
                        Debug.Log("Error: Missing prefab");
                    }
                }

				if (spawnList [i].currentDelay != spawnList [i].delayBetweenSpawns)
				{
					spawnList [i].currentDelay = spawnList [i].delayBetweenSpawns;
				}
			}
		}
	}

    private void SpawnOrbitMinion(StateController controller)
    {
        for (int i = 0; i < orbitSpawnList.Count; i++)
        {
            orbitSpawnList[i].currentTime += Time.deltaTime;

            if (orbitSpawnList[i].currentDelay < 0)
            {
                orbitSpawnList[i].currentDelay = orbitSpawnList[i].spawnStartDelay;
            }

            if (orbitSpawnList[i].currentTime >= orbitSpawnList[i].currentDelay
                && orbitSpawnList[i].currentSpawnCount < orbitSpawnList[i].numberOfSpawns)
            {
                for (int m = 0; m < orbitSpawnList[i].minionPrefabs.Count; m++)
                {
                    if (orbitSpawnList[i].minionPrefabs[m] != null)
                    {
                        GameObject minion; //= (GameObject)Instantiate(orbitSpawnList[i].minionPrefabs[m], controller.transform);

                        if (orbitSpawnList[i].spawnFromObject == true)
                        {
                            //minion.transform.position = controller.transform.position;
                            minion = (GameObject)Instantiate(orbitSpawnList[i].minionPrefabs[m], controller.transform);
                        }

                        else
                        {
                            minion = (GameObject)Instantiate(orbitSpawnList[i].minionPrefabs[m]);
                        }

                        if (orbitSpawnList[i].destroySpawnedAfterTime == true)
                        {
                            Destroy(minion, orbitSpawnList[i].lifeTime);
                        }

                        controller.minionList.Add(minion);

                        RotateAroundObject rotateAround = minion.GetComponent<RotateAroundObject>();

                        if (rotateAround != null)
                        {
                            rotateAround.centerPoint = controller.gameObject;
                        }

                        else
                        {
                            Debug.Log("Error: orbit object does not have the 'RotateAround' script" + "\n"
                                + "Spawn object: " + controller.gameObject.name + "\n"
                                + "Orbit object prefab: " + minion.name);
                        }
                    }

                    else
                    {
                        Debug.Log("Error: Missing prefab");
                    }
                }

                if (orbitSpawnList[i].currentDelay != orbitSpawnList[i].delayBetweenSpawns)
                {
                    orbitSpawnList[i].currentDelay = orbitSpawnList[i].delayBetweenSpawns;
                }

                orbitSpawnList[i].currentTime -= orbitSpawnList[i].currentDelay;
                orbitSpawnList[i].currentSpawnCount++;
            }
        }
    }

    private void SpawnPickup(StateController controller)
    {
        for (int i = 0; i < pickUpList.Count; i++)
        {
            pickUpList[i].currentTime += Time.deltaTime;

            if (pickUpList[i].currentDelay < 0)
            {
                pickUpList[i].currentDelay = pickUpList[i].spawnStartDelay;
            }

            if (pickUpList[i].currentTime >= pickUpList[i].currentDelay
                && pickUpList[i].currentSpawnCount < pickUpList[i].numberOfSpawns)
            {
                for (int m = 0; m < pickUpList[i].minionPrefabs.Count; m++)
                {
                    if (pickUpList[i].minionPrefabs[m] != null)
                    {
                        GameObject item; //= (GameObject)Instantiate(spawnList[i].minionPrefabs[m], controller.transform);

                        if (pickUpList[i].spawnFromObject == true)
                        {
                            //minion.transform.position = controller.transform.position;
                            item = (GameObject)Instantiate(pickUpList[i].minionPrefabs[m], controller.transform);
                            Debug.Log(item.transform.position);
                        }

                        else
                        {
                            item = (GameObject)Instantiate(pickUpList[i].minionPrefabs[m]);
                        }

                        if (pickUpList[i].destroySpawnedAfterTime == true)
                        {
                            Destroy(item, pickUpList[i].lifeTime);
                        }

                        controller.itemList.Add(item);
                    }

                    else
                    {
                        Debug.Log("Error: Missing prefab");
                    }
                }

                if (pickUpList[i].currentDelay != pickUpList[i].delayBetweenSpawns)
                {
                    pickUpList[i].currentDelay = pickUpList[i].delayBetweenSpawns;
                }

                pickUpList[i].currentTime -= pickUpList[i].currentDelay;
                pickUpList[i].currentSpawnCount++;
            }
        }
    }
}


[System.Serializable]
public class SpawnMinionClass
{
	public List<GameObject> minionPrefabs = new List<GameObject>();
    //The object themselvs spawn orbit objects
	public float spawnStartDelay = 1;
	public float delayBetweenSpawns = 1;
	public int numberOfSpawns = 1;
	[HideInInspector] public int currentSpawnCount = 0;

    public bool destroySpawnedAfterTime = true;

    [Tooltip("If the minion spawns from the object or not")]
    public bool spawnFromObject = false;

	public float lifeTime = 10;

	[HideInInspector] public float currentDelay = -1;
	[HideInInspector] public float currentTime = 0;
}
