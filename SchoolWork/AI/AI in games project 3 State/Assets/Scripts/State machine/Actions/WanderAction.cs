using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Wander")]
public class WanderAction : StateAction {
    public override void Act(StateController controller)
    {
        Wander(controller);
    }

    private void Wander(StateController controller)
    {
        NavMeshHit hit = new NavMeshHit();

        while(!hit.hit)
        {
            Vector3 randomDir = Random.insideUnitSphere * controller.wanderDistance;
            randomDir += controller.transform.position;

            NavMesh.SamplePosition(randomDir, out hit, controller.wanderDistance, 1);
        }

        controller.investigateLocation = hit.position;
        controller.navMeshAgent.destination = hit.position;
        controller.navMeshAgent.isStopped = false;
    }
}
