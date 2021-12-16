using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour {

    [Tooltip("If empty, destroy")]
    [SerializeField]
    private bool destroyWhenEmpty = true;


    [Tooltip("List of prefabs")]
    [SerializeField]
    private List<PrefabSpawn> prefabList = new List<PrefabSpawn>();

    private List<GameObject> spawnedList = new List<GameObject>();

	void Start ()
    {
        CheckSpawns();
        SpawnPrefabs();
	}

    void Update()
    {
        if (destroyWhenEmpty == true)
        {
            //Checks if all the spawned object dosen't exist
            bool empty = CheckEmpty();

            //If they do not exist, destroy this gameObject
            if (empty == true)
            {
                Destroy(gameObject);
            }
        }
    }

    //Checks all of the spawns
    private void CheckSpawns()
    {
        for (int listIndex = 0; listIndex < prefabList.Count; listIndex++)
        {
            bool transformPositionAdded = false;
            for (int spawnIndex = 0; spawnIndex < prefabList[listIndex].spawnList.Count; spawnIndex++)
            {
                //If spawn is null
                if (prefabList[listIndex].spawnList[spawnIndex] == null)
                {
                    //If the transform position hasn't been added, add itself
                    if (transformPositionAdded == false)
                    {
                        transformPositionAdded = true;
                        prefabList[listIndex].spawnList[spawnIndex] = transform;
                    }

                    //Else if it has already added itsef, remove this spawnpoint
                    else
                    {
                        prefabList[listIndex].spawnList.RemoveAt(spawnIndex);
                        spawnIndex--;
                    }
                }
            }

            if (prefabList[listIndex].spawnList.Count == 0)
            {
                prefabList[listIndex].spawnList.Add(transform);
            }
        }
    }

    //Spawns the prefabs from the spawns
    private void SpawnPrefabs()
    {
        for (int listIndex = 0; listIndex < prefabList.Count; listIndex++)
        {
            for (int spawnIndex = 0; spawnIndex < prefabList[listIndex].spawnList.Count; spawnIndex++)
            {
                //If the prefab isn't null, spawn
                if (prefabList[listIndex].prefab != null)
                {
                    var spawnedPrefab = (GameObject)Instantiate(prefabList[listIndex].prefab,
                        prefabList[listIndex].spawnList[spawnIndex].transform.position,
                        prefabList[listIndex].spawnList[spawnIndex].transform.rotation);

                    //Adds so that the spawned object is a child of the spawner object
                    spawnedPrefab.transform.parent = gameObject.transform;

                    spawnedList.Add(spawnedPrefab);
                }
            }
        }
    }

    //Checks if all the spawned objects doesn't exist
    private bool CheckEmpty()
    {
        bool isEmpty = true;

        for (int i = 0; i < spawnedList.Count; i++)
        {
            if (spawnedList[i] != null)
            {
                isEmpty = false;
                break;
            }

            else
            {
                spawnedList.RemoveAt(i);
                i--;
            }
        }

        return isEmpty;
    }
}


[System.Serializable]
public class PrefabSpawn
{
    public GameObject prefab;

    public List<Transform> spawnList = new List<Transform>();
}
