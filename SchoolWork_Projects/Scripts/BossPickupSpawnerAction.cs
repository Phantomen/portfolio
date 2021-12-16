using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/BossPickupSpawn")]
public class BossPickupSpawnerAction : StateAction {

    [Tooltip("The list of the pickups")]
    [SerializeField]
    private GameObject[] pickupList = new GameObject[5];
    private int currentSpawnIndex = 0;

    [Tooltip("The delay before the first spawn")]
    [SerializeField]
    private float startDelay = 0;
    private float currentTime = 0;

    private GameObject currentPickupInScene = null; 


    public override void Act(StateController controller)
    {
        //If there is no pickup in scene and it has not spawned enough
        if (currentPickupInScene == null
            && currentSpawnIndex < 5)
        {
            SpawnPickups();
        }
    }

    private void SpawnPickups()
    {
        if (currentTime < startDelay)
        {
            currentTime += Time.deltaTime;
        }

        if (currentTime >= startDelay)
        {
            if (pickupList[currentSpawnIndex] != null)
            {
                var pickup = (GameObject)Instantiate(pickupList[currentSpawnIndex]);
                currentPickupInScene = pickup;
            }

            currentSpawnIndex++;
        }
    }


    //Destroys the current pickup in scene and reset time and index
    public override void Reset(StateController controller)
    {
        currentTime = 0;

        currentSpawnIndex = 0;

        if (currentPickupInScene != null)
        {
            Destroy(currentPickupInScene);
        }

        currentPickupInScene = null;
    }
}
