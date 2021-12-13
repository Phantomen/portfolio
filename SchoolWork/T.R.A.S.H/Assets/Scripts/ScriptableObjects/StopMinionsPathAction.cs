using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/StopMinionsPathAction")]
public class StopMinionsPathAction : StateAction {
    public override void Act(StateController controller)
    {
        StopPath(controller);
    }

    private void StopPath(StateController controller)
    {
        for (int minionIndex = 0; minionIndex < controller.minionList.Count; minionIndex++)
        {
            PathFollower pathFollower = controller.minionList[minionIndex].GetComponent<PathFollower>();

            if (pathFollower != null)
            {
                pathFollower.bouncingBetweenPoints = false;
                pathFollower.destroyOnEndPoints = true;
            }
        }
    }

    public override void Reset(StateController controller) { }
}
