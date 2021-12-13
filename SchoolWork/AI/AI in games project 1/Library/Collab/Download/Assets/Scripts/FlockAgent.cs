using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour {

    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    private List<Vector3> checkPointList = new List<Vector3>();
    public Vector3 GetCurrentCheckpoint { get { return checkPointList[0]; } }
    public List<Vector3> GetCheckpoints { get { return checkPointList; } }

    [Range(0, 20)]
    public float checkpointSeekDistance = 3;
    private float checkpointSeekDistanceSqr;

    public bool dying = false;


    public void SetCheckPoint(Vector3 pos)
    {
        checkPointList.Clear();
        checkPointList.Add(pos);
        checkpointSeekDistanceSqr = checkpointSeekDistance * checkpointSeekDistance;
    }

    public void AddCheckPoint(Vector3 pos)
    {
        checkPointList.Add(pos);
    }

    public void CheckNextCheckpoint()
    {
        if(checkPointList.Count > 1)
        {
            List<Vector3> newCheckPointList = new List<Vector3>();

            for (int i = 1; i < checkPointList.Count; i++)
            {
                newCheckPointList.Add(checkPointList[i]);
            }

            checkPointList = newCheckPointList;
        }
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
        SetCheckPoint(new Vector3(0, 0, 0));
        //checkPointList.Add(new Vector3(10, 20, 0));
        //checkPointList.Add(new Vector3(-10, -20, 0));
    }

	// Use this for initialization
	void Start ()
    {
        agentCollider = GetComponent<Collider2D>();
	}
	
    public void Move(Vector2 velocity)
    {
        transform.up = velocity; //Vector3.RotateTowards(transform.up, velocity, agentFlock.rotationSpeed * Time.deltaTime, 0.0f);
        transform.position += (Vector3)velocity * Time.deltaTime;

        if(checkPointList.Count > 1 && Vector2.SqrMagnitude(((Vector2)transform.position - (Vector2)checkPointList[0])) < checkpointSeekDistanceSqr)
        {
            CheckNextCheckpoint();
        }
    }

    public void KillAgent()
    {
        dying = true;
        agentFlock.RemoveAgent(this);
        Destroy(gameObject);
    }
}
