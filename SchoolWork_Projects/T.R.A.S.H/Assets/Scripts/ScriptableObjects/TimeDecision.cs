using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Decision/TimeDecision")]
public class TimeDecision : StateDecision {

    [Tooltip("The duration of this state (next state after duration has elapsed)")]
    [SerializeField]
    private float duration = 5;

    public override bool Decide(StateController controller)
    {
        return DecisionTime(controller);
    }

    private bool DecisionTime(StateController controller)
    {
        return controller.CheckIfCountDownElapsed(duration);
    }
}
