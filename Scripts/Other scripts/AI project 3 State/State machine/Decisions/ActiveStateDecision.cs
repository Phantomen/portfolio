using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ActiveState")]
public class ActiveStateDecision : StateDecision {
    public override bool Decide(StateController controller)
    {
        return TargetActive(controller);
    }

    private bool TargetActive(StateController controller)
    {
        return controller.playerGameObject.activeSelf;
    }
}
