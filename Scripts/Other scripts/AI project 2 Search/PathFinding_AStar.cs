using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(NodeGrid))]
public class PathFinding_AStar : MonoBehaviour {

    public NodeGrid nodeGrid;

    public Node nodeStart, nodeTarget;

    public List<NodeGridCost> finalPath;

    NodeGridCost nodeInGridStart, nodeInGridTarget;

	// Use this for initialization
	void Start ()
    {
        nodeGrid = GetComponentInParent<NodeGrid>();
        nodeInGridStart = nodeGrid.GetNodeInGrid(nodeStart);
        nodeInGridTarget = nodeGrid.GetNodeInGrid(nodeTarget);

        finalPath = FindPath(nodeInGridStart, nodeInGridTarget);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //FindPath();
	}

    public List<NodeGridCost> FindPath(NodeGridCost nGStart, NodeGridCost nGTarget)
    {
        if(nGStart == null || nGTarget == null)
        {
            return null;
        }

        List<NodeGridCost> OpenList = new List<NodeGridCost>();
        HashSet<NodeGridCost> ClosedList = new HashSet<NodeGridCost>();

        OpenList.Add(nGStart);

        while (OpenList.Count > 0)
        {
            NodeGridCost currentNode = OpenList[0];
            //Get the node with the lowest cost
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < currentNode.FCost || OpenList[i].FCost == currentNode.FCost && OpenList[i].hCost < currentNode.hCost)
                {
                    currentNode = OpenList[i];
                }
            }

            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            if (currentNode == nGTarget)
            {
                return GetFinalPath(nGStart, nGTarget);
            }

            //Get all the neighbornodes
            foreach(NodeGridCost neighborNode in nodeGrid.GetNeighboringNodes(currentNode))
            {
                if (neighborNode.node.IsObstructed() || ClosedList.Contains(neighborNode) || neighborNode.tempBlocked)
                {
                    continue;
                }

                float moveCost = currentNode.gCost + Vector3.Distance(currentNode.node.transform.position, neighborNode.node.transform.position);//GetManhattenDistance(currentNode, neighborNode);

                if(moveCost < neighborNode.gCost || !OpenList.Contains(neighborNode))
                {
                    neighborNode.gCost = moveCost;
                    neighborNode.hCost = Vector3.Distance(neighborNode.node.transform.position, nGTarget.node.transform.position);//GetManhattenDistance(neighborNode, nGTarget);
                    neighborNode.parent = currentNode;

                    if (!OpenList.Contains(neighborNode))
                    {
                        OpenList.Add(neighborNode);
                    }
                }
            }
        }

        return null;
    }

    List<NodeGridCost> GetFinalPath(NodeGridCost startNode, NodeGridCost targetNode)
    {
        List<NodeGridCost> finalPath = new List<NodeGridCost>();
        NodeGridCost currentNode = targetNode;

        while(currentNode != startNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        finalPath.Add(currentNode);

        finalPath.Reverse();

        //If diagonally blocked, set it as that and set the blocking pair
        for (int i = 0; i < finalPath.Count - 1; i++)
        {
            if (finalPath[i].GridPos[0] != finalPath[i+1].GridPos[0]
                && finalPath[i].GridPos[0] != finalPath[i+1].GridPos[0])
            {
                int xTemp = finalPath[i].GridPos[0] + (finalPath[i+1].GridPos[0] - finalPath[i].GridPos[0]);
                int yTemp = finalPath[i].GridPos[1] + (finalPath[i+1].GridPos[1] - finalPath[i].GridPos[1]);

                finalPath[i].diagonalBlocked = true;
                finalPath[i].diagonalParentBlock[0] = nodeGrid.nodeGrid[finalPath[i].GridPos[0], yTemp];
                finalPath[i].diagonalParentBlock[1] = nodeGrid.nodeGrid[xTemp, finalPath[i].GridPos[1]];
            }
        }

        return finalPath;
    }


    private int GetManhattenDistance(NodeGridCost nodeA, NodeGridCost nodeB)
    {
        int[] gridA = nodeA.GridPos;
        int[] gridB = nodeB.GridPos;

        int x = Mathf.Abs(gridA[0] - gridB[0]);
        int y = Mathf.Abs(gridA[1] - gridB[1]);

        return x + y;
    }


    public bool UpgradeableNode(Node nodeToUpgrade)
    {
        NodeGridCost node = nodeGrid.GetNodeInGrid(nodeToUpgrade);
        if (node == null)
        {
            Debug.Log("not in grid");
            return false;
        }

        if(node == nodeInGridStart || node == nodeInGridTarget)
        {
            return false;
        }

        //If nodes is in the path, check if you can get another path around it
        if (finalPath.Contains(node))
        {
            node.tempBlocked = true;
            List<NodeGridCost> foundOtherPath = FindPath(nodeInGridStart, nodeInGridTarget);
            node.tempBlocked = false;

            if(foundOtherPath == null)
            {
                return false;
            }

            //finalPath = foundOtherPath;

            return true;
        }

        //Check if it is a digonaly blocked pair, if so, see if you can find another path
        for(int i = 0; i < finalPath.Count; i++)
        {
            if (finalPath[i].diagonalBlocked)
            {
                if(node == finalPath[i].diagonalParentBlock[0])
                {
                    node.tempBlocked = true;
                    List<NodeGridCost> foundOtherPath = FindPath(nodeInGridStart, nodeInGridTarget);
                    node.tempBlocked = false;

                    if (foundOtherPath == null)
                    {
                        return false;
                    }

                    //finalPath = foundOtherPath;

                    return true;
                }

                else if(node == finalPath[i].diagonalParentBlock[1])
                {
                    node.tempBlocked = true;
                    List<NodeGridCost> foundOtherPath = FindPath(nodeInGridStart, nodeInGridTarget);
                    node.tempBlocked = false;

                    if (foundOtherPath == null)
                    {
                        return false;
                    }

                    //finalPath = foundOtherPath;

                    return true;
                }
            }
        }



        return true;
    }


    public void UpdatePath()
    {
        finalPath = FindPath(nodeInGridStart, nodeInGridTarget);
    }



    void OnDrawGizmos()
    {
        if (nodeGrid != null)
        {
            if (finalPath != null)
            {
                foreach (NodeGridCost node in finalPath)
                {
                    if (node.diagonalParentBlock[0] != null)
                    {
                        if (node.diagonalBlocked)
                        {
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawCube(node.diagonalParentBlock[0].node.transform.position, Vector3.one * (nodeGrid.nodeDiameter - nodeGrid.distance));
                            Gizmos.DrawCube(node.diagonalParentBlock[1].node.transform.position, Vector3.one * (nodeGrid.nodeDiameter - nodeGrid.distance));
                        }
                    }

                    Gizmos.color = Color.green;

                    if (node.node) { Gizmos.DrawCube(node.node.transform.position, Vector3.one * (nodeGrid.nodeDiameter - nodeGrid.distance)); }
                    else { Gizmos.DrawCube(node.nullPosition, Vector3.one * (nodeGrid.nodeDiameter - nodeGrid.distance)); }
                }
            }
        }
    }

}
