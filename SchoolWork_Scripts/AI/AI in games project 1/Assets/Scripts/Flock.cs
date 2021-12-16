using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agentList = new List<FlockAgent>();
    public List<FlockAgent> GetAgentList { get { return agentList; } }
    List<FlockAgent> agentsToAdd = new List<FlockAgent>();
    List<FlockAgent> agentsToRemove = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    float agentIndex = 0;

    [Range(0, 500)]
    public int startingAgents = 5;
    const float agentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(0.1f, 1f)]
    public float deceleration = 0.3f;

    [Range(0f, 10f)]
    public float rotationSpeed = 0.5f;


    [Range(1f, 10f)]
    public float attackRadius = 20f;
    [Range(1f, 100f)]
    public float chaseRadius = 40f;

    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;
    [Range(0f, 1f)]
    public float AttackRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    float squareAttackRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    public float SquareAttackRadius { get { return squareAttackRadius; } }

    public Flock enemyFlock;
    public Flock GetEnemyFlock { get { return enemyFlock; } }

    // Use this for initialization
    void Start ()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        squareAttackRadius = squareNeighborRadius * AttackRadiusMultiplier * AttackRadiusMultiplier;

        for (int i = 0; i < startingAgents; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingAgents * agentDensity + (Vector2)transform.position,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform);

            newAgent.name = "Agent " + agentIndex;
            newAgent.Initialize(this);
            agentList.Add(newAgent);
            agentIndex++;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach(FlockAgent item in agentsToRemove)
        {
            agentList.Remove(item);
        }
        agentsToRemove.Clear();

        foreach (FlockAgent item in agentsToAdd)
        {
            agentList.Add(item);
        }
        agentsToAdd.Clear();


        bool addCheckpoint = false;
        Vector3 checkpointPos = new Vector3();
        if (Input.GetMouseButtonDown(0))
        {
            checkpointPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            checkpointPos.z = 0f;
            addCheckpoint = true;
        }


        foreach (FlockAgent agent in agentList)
        {
            if (addCheckpoint)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    agent.AddCheckPoint(checkpointPos);
                }

                else
                {
                    agent.SetCheckPoint(checkpointPos);
                }
            }


            List<Transform> neighborContext = GetNearbyObjects(agent, neighborRadius);
            List<Transform> attackContext = new List<Transform>(); //GetNearbyObjects(agent, attackRadius);
            List<Transform> chaseContext = new List<Transform>(); //GetNearbyObjects(agent, chaseRadius);

            Vector2 move = behaviour.CalculateMove(agent, neighborContext, attackContext, chaseContext, this);
            move *= driveFactor;

            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }

            agent.Move(move);

            //For demo
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 4f);
        }
	}

    List<Transform> GetNearbyObjects(FlockAgent agent, float radius)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, radius);

        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }

        return context;
    }

    public void RemoveAgent(FlockAgent agentToRemove)
    {
        agentsToRemove.Add(agentToRemove);
        //agentList.Remove(agentToRemove);
    }

    public void AddAgent(Vector3 agentPos, Quaternion agentRotation)
    {
        FlockAgent newAgent = Instantiate(
                agentPrefab,
                agentPos,
                agentRotation,
                transform);

        newAgent.name = "Agent " + agentIndex;
        newAgent.Initialize(this);
        agentsToAdd.Add(newAgent);
        agentIndex++;
    }
}
