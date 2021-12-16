using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Decision/MinionsGoneDecision")]
public class MinionsGoneDecision : StateDecision {

    public int minionsLeft = 0;

    public override bool Decide(StateController controller)
    {
        return DecisionMinions(controller);
    }

    private bool DecisionMinions(StateController controller)
    {
        if (minionsLeft < 0)
        {
            minionsLeft = 0;
        }

        return (controller.minionList.Count <= minionsLeft);
    }
}
