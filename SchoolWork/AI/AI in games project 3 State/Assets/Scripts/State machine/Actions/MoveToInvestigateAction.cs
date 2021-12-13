using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Investigate")]
public class MoveToInvestigateAction : StateAction {
    public override void Act(StateController controller)
    {
        Investigate(controller);
    }

    public void Investigate(StateController controller)
    {
        controller.navMeshAgent.destination = controller.investigateLocation;
        controller.navMeshAgent.isStopped = false;
    }
}
