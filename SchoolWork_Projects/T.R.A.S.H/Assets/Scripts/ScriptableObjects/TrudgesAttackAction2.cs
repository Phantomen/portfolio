using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/TrudgesAttackTwo")]
public class TrudgesAttackAction2 : StateAction {

    public ShooterPattern shotPattern;

    public List<GameObject> minionPrefabs = new List<GameObject>();
    private List<GameObject> minions = new List<GameObject>();

    private bool spawnedMinions = false;

    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        shotPattern.Shoot(controller.gameObject);
    }

    public override void Reset(StateController controller)
    {
        ResetShooting(controller);

        if (spawnedMinions == false)
        {
            SpawnMinions(controller);
            spawnedMinions = true;
        }
    }

    private void SpawnMinions(StateController controller)
    {
        for (int i = 0; i < minionPrefabs.Count; i++)
        {
            if (minionPrefabs[i] != null)
            {
                var minion = (GameObject)Instantiate(minionPrefabs[i], new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                minions.Add(minion);

                controller.minionList.Add(minion);
            }
        }
    }

    private void ResetShooting(StateController controller)
    {
        shotPattern.Reset(controller.gameObject);
    }
}
