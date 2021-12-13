using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/SpawnInstantly")]
public class SpawnInstantlyAction : StateAction
{

    public List<SpawnMinionClass> spawnList = new List<SpawnMinionClass>();

    public override void Act(StateController controller)
    {
        Spawn(controller);
    }

    public override void Reset(StateController controller)
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            spawnList[i].currentTime = 0;
            spawnList[i].currentDelay = spawnList[i].spawnStartDelay;
            spawnList[i].currentSpawnCount = 0;
        }
    }


    private void Spawn(StateController controller)
    {
        //For list, try to spawn
        for (int i = 0; i < spawnList.Count; i++)
        {
            spawnList[i].currentTime += Time.deltaTime;

            if (spawnList[i].currentDelay < 0)
            {
                spawnList[i].currentDelay = spawnList[i].spawnStartDelay;
            }

            //If the time has expired for the spawnlist and it has not spawned everything
            if (spawnList[i].currentTime >= spawnList[i].currentDelay
                && spawnList[i].currentSpawnCount < spawnList[i].numberOfSpawns)
            {
                //For each object that it will spawn
                for (int m = 0; m < spawnList[i].objectPrefabs.Count; i++)
                {
                    var minion = (GameObject)Instantiate(spawnList[i].objectPrefabs[m], controller.transform.position, controller.transform.rotation);
                    Destroy(minion, spawnList[i].lifeTime);
                    controller.minionList.Add(minion);

                    if (spawnList[i].currentDelay != spawnList[i].delayBetweenSpawns)
                    {
                        spawnList[i].currentDelay = spawnList[i].delayBetweenSpawns;
                    }
                }
                spawnList[i].currentSpawnCount++;
            }
        }
    }
}
