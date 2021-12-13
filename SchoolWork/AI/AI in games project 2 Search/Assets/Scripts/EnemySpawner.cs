using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab;

    public int enemyNumbers = 5;
    public float enemyWaveMultiplier = 1f / 3f;

    private int enemyCount;

    public float spawnStartDelay = 5f;

    public float spawnDelay = 0.5f;

    private float currentDelay = 0;

    private Vector3 spawnPosition;

    private int currentSpawn = 0;

    private PathFinding_AStar pathFind;

    private List<GameObject> enemies = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        currentDelay = spawnStartDelay;

        pathFind = GetComponent<PathFinding_AStar>();

        spawnPosition = pathFind.nodeStart.transform.position;

        enemyCount = enemyNumbers;

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        currentDelay -= Time.fixedDeltaTime;

        if(currentDelay <= 0)
        {
            Spawn();
            //currentDelay += spawnDelay;
        }
	}

    void Spawn()
    {
        if (enemyPrefab)
        {
            GameObject enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition + enemyPrefab.transform.position, new Quaternion());
            enemies.Add(enemy);
            enemy.name = "Enemy " + currentSpawn;
            currentSpawn++;
            enemy.GetComponent<PathFollow>().pathFind = pathFind;

            if(currentSpawn == enemyCount)
            {
                currentDelay += spawnStartDelay;
                currentSpawn = 0;
                enemyCount += Mathf.RoundToInt((float)enemyCount * enemyWaveMultiplier);
                return;
            }
            currentDelay += spawnDelay;
        }
    }

    public void Remove(GameObject objectToRemove)
    {
        enemies.Remove(objectToRemove);
    }

    public bool CheckObstructing(Node node)
    {
        List<GameObject> cleanedList = new List<GameObject>();
        for(int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i])
            {
                cleanedList.Add(enemies[i]);
            }
        }

        enemies = cleanedList;

        for(int i = 0; i < enemies.Count; i++)
        {
            PathFollow followPath = enemies[i].GetComponent<PathFollow>();
            if (followPath.NextCheckPointIsNode(node))
            {
                return true;
            }

            if (followPath.NodeObstructingPath(node))
            {
                return true;
            }
        }


        return false;
    }

    public void UpdateEnemyPath()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i])
            {
                PathFollow followPath = enemies[i].GetComponent<PathFollow>();
                followPath.UpdatePath();
            }
        }
    }
}
