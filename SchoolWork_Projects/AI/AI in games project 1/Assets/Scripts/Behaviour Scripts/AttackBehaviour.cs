using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Attack")]
public class AttackBehaviour : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> neighborContext, List<Transform> attackContext, List<Transform> chaseContext, Flock flock)
    {

        //float distance = 1000000;
        //Transform closestAgent = null;

        /*
        List<Transform> filteredContext = (filter == null) ? neighborContext : filter.Filter(agent, neighborContext);
        foreach (Transform item in filteredContext)
        {
            FlockAgent enemyAgent = item.GetComponent<FlockAgent>();
            if (enemyAgent.dying == false && Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAttackRadius)
            {
                agent.AgentFlock.AddAgent(item.transform.position, item.transform.rotation);
                enemyAgent.KillAgent();
            }
        }
        */

        List<FlockAgent> enemyAgentFlock = flock.GetEnemyFlock.GetAgentList;

        foreach (FlockAgent efg in enemyAgentFlock)
        {
            FlockAgent enemyAgent = efg.GetComponent<FlockAgent>();

            float distance = Vector2.Distance(efg.transform.position, agent.transform.position);

            if (enemyAgent.dying == false && distance * distance < flock.SquareAttackRadius)
            {
                agent.AgentFlock.AddAgent(efg.transform.position, efg.transform.rotation);
                enemyAgent.KillAgent();
            }
        }


        return Vector2.zero;
    }
}
