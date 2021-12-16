using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/NextState")]
public class NextStateDecision : StateDecision {
    public override bool Decide(StateController controller)
    {
        return true;
    }
}
