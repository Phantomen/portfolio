using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/AtTargetLocation")]
public class AtTargetLocationDecision : StateDecision {

    public override bool Decide(StateController controller)
    {
        return AtLocation(controller);
    }

    private bool AtLocation(StateController controller)
    {
        if (Vector3.Distance(controller.transform.position, controller.navMeshAgent.destination) <= controller.investigationDistance)
        {
            return true;
        }

        return false;
    }
}
