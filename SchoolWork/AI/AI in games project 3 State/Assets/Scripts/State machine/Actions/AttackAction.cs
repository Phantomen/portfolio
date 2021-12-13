using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : StateAction {
    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        if (Vector3.Distance(controller.transform.position, controller.playerGameObject.transform.position) <= controller.attackRange)
        {
            Vector3 dirToPlayer = (controller.playerGameObject.transform.position - controller.transform.position).normalized;
            if (Vector3.Angle(controller.transform.forward, dirToPlayer) <= (controller.fieldOfView / 2f))
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(controller.transform.position, dirToPlayer, out raycastHit, controller.attackRange))
                {
                    if (raycastHit.collider.gameObject == controller.playerGameObject)
                    {
                        controller.Attack();
                    }
                }
            }
        }
    }
}
