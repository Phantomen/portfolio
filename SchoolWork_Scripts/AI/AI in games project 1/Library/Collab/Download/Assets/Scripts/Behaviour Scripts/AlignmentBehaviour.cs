using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class AlignmentBehaviour : FilteredFlockBehaviour {

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> neighborContext, List<Transform> attackContext, List<Transform> chaseContext, Flock flock)
    {
        //If no neighbors, maintain current alignment
        if (neighborContext.Count == 0) { return agent.transform.up; }

        //Add all points together and average
        Vector2 alignmentMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? neighborContext : filter.Filter(agent, neighborContext);
        foreach (Transform item in filteredContext)
        {
            alignmentMove += (Vector2)item.transform.up;
        }
        alignmentMove /= neighborContext.Count;

        return alignmentMove;
    }
}
