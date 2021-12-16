using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/PlayerInView")]
public class PlayerInViewDecision : StateDecision
{
    public override bool Decide(StateController controller)
    {
        return InView(controller);
    }

    private bool InView(StateController controller)
    {
        if(Vector3.Distance(controller.transform.position, controller.playerGameObject.transform.position) <= controller.viewDistance)
        {
            Vector3 dirToPlayer = (controller.playerGameObject.transform.position - controller.transform.position).normalized;
            if(Vector3.Angle(controller.transform.forward, dirToPlayer) <= (controller.fieldOfView / 2f))
            {
                RaycastHit raycastHit;
                if(Physics.Raycast(controller.transform.position, dirToPlayer, out raycastHit, controller.viewDistance))
                {
                    if (raycastHit.collider.gameObject == controller.playerGameObject)
                    {
                        controller.investigateLocation = controller.playerGameObject.transform.position;
                        //player hit
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
