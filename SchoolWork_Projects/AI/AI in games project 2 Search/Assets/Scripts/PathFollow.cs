using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour {

    //public NodeGrid nodeGrid;

    private List<NodeGridCost> finalPath;
    private int waypointIndex = 1;

    public float speed = 5f;

    private Transform target;

    [HideInInspector]
    public PathFinding_AStar pathFind;

    private EnemyHealth health;

	// Use this for initialization
	void Start ()
    {
        if (pathFind)
        {
            finalPath = pathFind.finalPath;
            target = finalPath[1].node.transform;
            //pathFind = GetComponent<PathFinding_AStar>();
        }

        health = GetComponent<EnemyHealth>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (health.isAlive && target != null)
        {
            Vector3 dir = new Vector3(target.position.x, 0, target.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            float distance = dir.magnitude;

            if (distance < speed * Time.fixedDeltaTime)
            {
                float remaningDistance = speed * Time.fixedDeltaTime - distance;

                transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
                NextCheckPoint();

                if (health.isAlive)
                {
                    Update_OverShoot(remaningDistance);
                }
            }

            else
            {
                transform.position += dir.normalized * speed * Time.fixedDeltaTime;
            }
        }
	}

    void Update_OverShoot(float remaningDistanceToTravel)
    {
        Vector3 dir = new Vector3(target.position.x, 0, target.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        float distance = dir.magnitude;

        if (distance < remaningDistanceToTravel)
        {
            float remaningDistance = remaningDistanceToTravel - distance;

            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
            NextCheckPoint();

            if (gameObject == null)
            {
                Update_OverShoot(remaningDistance);
            }
        }

        else
        {
            transform.position += dir.normalized * remaningDistanceToTravel;
        }
    }

    private void NextCheckPoint()
    {
        waypointIndex++;
        if(waypointIndex >= finalPath.Count)

        {
            DestinationReached();
            return;
        }

        target = finalPath[waypointIndex].node.transform;
    }

    private void DestinationReached()
    {
        if (gameObject.GetComponent<EnemyHealth>().isAlive)
        {
            gameObject.GetComponent<EnemyHealth>().Kill();
        }
    }

    public bool NextCheckPointIsNode(Node node)
    {
        if(finalPath[waypointIndex].node == node)
        {
            return true;
        }

        if(waypointIndex > 0)
        {
            if(finalPath[waypointIndex-1].node == node)
            {
                return true;
            }
        }

        return false;
    }


    public bool NodeObstructingPath(Node node)
    {
        NodeGridCost nodeCost = pathFind.nodeGrid.GetNodeInGrid(node);

        //If node is in finalPath
        for (int i = waypointIndex; i < finalPath.Count; i++)
        {
            //NodeGridCost nodeCost = pathFind.nodeGrid.GetNodeInGrid(node);
            if (finalPath[i] == nodeCost)
            {
                pathFind.nodeGrid.nodeGrid[nodeCost.GridPos[0], nodeCost.GridPos[1]].tempBlocked = true;
                List<NodeGridCost> foundOtherPath = pathFind.FindPath(finalPath[waypointIndex], finalPath[finalPath.Count - 1]);
                pathFind.nodeGrid.nodeGrid[nodeCost.GridPos[0], nodeCost.GridPos[1]].tempBlocked = false;

                if (foundOtherPath == null)
                {
                    return true;
                }

                //finalPath = foundOtherPath;
            }
        }


        //If node is in finalPath diagonal parent
        for (int i = waypointIndex; i < finalPath.Count; i++)
        {
            if (finalPath[i].diagonalBlocked)
            {
                //NodeGridCost nodeCost = pathFind.nodeGrid.GetNodeInGrid(node);
                if (nodeCost == finalPath[i].diagonalParentBlock[0])
                {
                    pathFind.nodeGrid.nodeGrid[nodeCost.GridPos[0], nodeCost.GridPos[1]].tempBlocked = true;
                    List<NodeGridCost> foundOtherPath = pathFind.FindPath(finalPath[waypointIndex], finalPath[finalPath.Count - 1]);
                    pathFind.nodeGrid.nodeGrid[nodeCost.GridPos[0], nodeCost.GridPos[1]].tempBlocked = false;

                    if (foundOtherPath == null)
                    {
                        return true;
                    }

                    //finalPath = foundOtherPath;

                }

                else if (nodeCost == finalPath[i].diagonalParentBlock[1])
                {
                    pathFind.nodeGrid.nodeGrid[nodeCost.GridPos[0], nodeCost.GridPos[1]].tempBlocked = true;
                    List<NodeGridCost> foundOtherPath = pathFind.FindPath(finalPath[waypointIndex], finalPath[finalPath.Count - 1]);
                    pathFind.nodeGrid.nodeGrid[nodeCost.GridPos[0], nodeCost.GridPos[1]].tempBlocked = false;

                    if (foundOtherPath == null)
                    {
                        return true;
                    }

                    //finalPath = foundOtherPath;
                }
            }
        }

        return false;
    }



    public void UpdatePath()
    {
        finalPath = pathFind.FindPath(finalPath[waypointIndex], finalPath[finalPath.Count - 1]);
        waypointIndex = 0;
        return;
    }



    public float GetDistanceLeftToGoal()
    {
        float dist = Vector3.Distance(transform.position, new Vector3(finalPath[waypointIndex].node.transform.position.x, transform.position.y, finalPath[waypointIndex].node.transform.position.z));
        for (int i = waypointIndex; i < finalPath.Count - 1; i++)
        {
            dist += Vector3.Distance(finalPath[i].node.transform.position, finalPath[i + 1].node.transform.position);
        }
        return dist;
    }
}
