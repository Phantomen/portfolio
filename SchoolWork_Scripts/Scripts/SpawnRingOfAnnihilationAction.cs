using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Actions/SpawnRingOfAnnihilation")]
public class SpawnRingOfAnnihilationAction : StateAction {

    [Tooltip("The blank")]
    [SerializeField]
    private GameObject blankPrefab;

    private bool triggered = false;

    public override void Act(StateController controller)
    {
        if (triggered == false)
        {
            triggered = true;
            TriggedBlank(controller);
        }
    }

    //Spawns the blank from the controllers position
    private void TriggedBlank(StateController controller)
    {
        Instantiate(blankPrefab, controller.gameObject.transform.position, new Quaternion());
    }


    public override void Reset(StateController controller)
    {
        triggered = false;
    }
}
