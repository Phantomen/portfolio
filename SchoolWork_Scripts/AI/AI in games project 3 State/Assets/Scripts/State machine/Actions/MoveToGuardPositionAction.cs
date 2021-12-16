using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Guard")]
public class MoveToGuardPositionAction : StateAction {
    public override void Act(StateController controller)
    {
        MoveToGuard(controller);
    }

    private void MoveToGuard(StateController controller)
    {
        controller.navMeshAgent.destination = controller.guardTransform;
        controller.navMeshAgent.isStopped = false;
    }
}
