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
        for (int i = 0; i < spawnList.Count; i++)
        {
            spawnList[i].currentTime += Time.deltaTime;

            if (spawnList[i].currentDelay < 0)
            {
                spawnList[i].currentDelay = spawnList[i].spawnStartDelay;
            }

            if (spawnList[i].currentTime >= spawnList[i].currentDelay
                && spawnList[i].currentSpawnCount < spawnList[i].numberOfSpawns)
            {
                for (int m = 0; m < spawnList[i].minionPrefabs.Count; i++)
                {
                    var minion = (GameObject)Instantiate(spawnList[i].minionPrefabs[m], controller.transform.position, controller.transform.rotation);
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
