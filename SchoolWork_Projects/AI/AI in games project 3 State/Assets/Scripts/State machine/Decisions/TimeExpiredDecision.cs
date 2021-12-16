using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/TimeExpired")]
public class TimeExpiredDecision : StateDecision {

    [SerializeField]
    private float duration = 5;

    public override bool Decide(StateController controller)
    {
        return TimeExpired(controller);
    }

    private bool TimeExpired(StateController controller)
    {
        if (controller.CheckIfCountDownElapsed(duration))
        {
            return true;
        }

        return false;
    }
}
