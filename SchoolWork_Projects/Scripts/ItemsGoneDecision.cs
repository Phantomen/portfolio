using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Decision/ItemsGoneDecision")]
public class ItemsGoneDecision : StateDecision
{

    public override bool Decide(StateController controller)
    {
        return DecisionItems(controller);
    }

    private bool DecisionItems(StateController controller)
    {
        return (controller.itemList.Count == 0);
    }
}
