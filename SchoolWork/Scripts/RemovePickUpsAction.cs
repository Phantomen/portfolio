using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/RemovePickups")]
public class RemovePickUpsAction : StateAction {

    public float time = 0;

    private bool removed = false;

    public override void Act(StateController controller)
    {
        RemovePickUps(controller);
    }

    private void RemovePickUps(StateController controller)
    {
        if (removed == false)
        {
            removed = true;
            controller.ItemListDestroyClear(time);
        }
    }

    public override void Reset(StateController controller)
    {
        removed = false;
    }
}
