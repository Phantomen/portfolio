using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/LookAround")]
public class LookAroundAction : StateAction {

    public bool lookRight = true;

    public override void Act(StateController controller)
    {
        LookAround(controller);
    }

    private void LookAround(StateController controller)
    {
        int lookRot = 1;
        if (!lookRight) { lookRot = -1; }

        float rotY = lookRot * controller.navMeshAgent.angularSpeed * Time.fixedDeltaTime;

        Vector3 rot = controller.transform.rotation.eulerAngles;
        rot.y += rotY;

        controller.transform.rotation = Quaternion.Euler(rot);
    }
}
