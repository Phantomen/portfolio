using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Chase")]
public class ChaseBehaviour : FilteredFlockBehaviour
{
    [Range(0, 500)]
    public float chaseRangeRadius = 10;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> neighborContext, List<Transform> attackContext, List<Transform> chaseContext, Flock flock)
    {
        //If no neighbors, return no adjustment
        if (neighborContext.Count == 0) { return Vector2.zero; }

        neighborContext = GetNearbyObjects(agent);

        //Add all points together and average
        Vector2 chaseMove = Vector2.zero;
        int nChase = 0;
        List<Transform> filteredContext = (filter == null) ? neighborContext : filter.Filter(agent, neighborContext);
        foreach (Transform item in filteredContext)
        {
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) < (chaseRangeRadius * chaseRangeRadius))
            {
                nChase++;
                chaseMove += (Vector2)(item.position - agent.transform.position);
            }
        }

        if (nChase > 0)
        {
            chaseMove /= nChase;
        }

        return chaseMove;
    }


    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, chaseRangeRadius);

        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }

        return context;
    }
}
