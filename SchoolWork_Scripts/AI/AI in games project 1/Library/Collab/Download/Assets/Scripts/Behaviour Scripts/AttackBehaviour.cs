using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Attack")]
public class AttackBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> neighborContext, List<Transform> attackContext, List<Transform> chaseContext, Flock flock)
    {

        float distance = 1000000;
        Transform closestAgent = null;

        List<Transform> filteredContext = (filter == null) ? attackContext : filter.Filter(agent, neighborContext);
        foreach (Transform item in filteredContext)
        {
            if (Vector2.Distance(item.position, agent.transform.position) < distance)
            {
                closestAgent = item;
                distance = Vector2.Distance(item.position, agent.transform.position);
            }
        }


        return Vector2.zero;
    }
}
