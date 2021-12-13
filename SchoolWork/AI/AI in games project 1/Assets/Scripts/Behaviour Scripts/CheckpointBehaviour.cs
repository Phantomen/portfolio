using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/CheckpointBehaviour")]
public class CheckpointBehaviour : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> neighborContext, List<Transform> attackContext, List<Transform> chaseContext, Flock flock)
    {
        if(agent.GetCheckpoints.Count == 0)
        {
            return Vector2.zero;
        }

        if (agent.GetCheckpoints.Count == 1)
        {
            return Arrive(agent);
        }

        return Seek(agent);
    }

    private Vector2 Seek(FlockAgent agent)
    {
        Vector2 desiredVelocity = agent.GetCurrentCheckpoint - agent.transform.position;
        return desiredVelocity;
    }

    private Vector2 Arrive(FlockAgent agent)
    {
        Vector2 desiredVelocity = agent.GetCurrentCheckpoint - agent.transform.position;
        //float dist = Vector3.Distance(agent.transform.position, agent.GetCurrentCheckpoint);

        //float speed = dist / agent.AgentFlock.deceleration;

        return desiredVelocity * agent.AgentFlock.deceleration;
    }
}
