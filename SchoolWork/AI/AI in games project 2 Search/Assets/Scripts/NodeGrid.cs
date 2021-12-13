using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    //public Transform startPosition;
    public LayerMask nodeMask;
    public Vector2 gridSize;
    public float nodeRadius = 0.5f;
    public float distance = 0;


    public NodeGridCost[,] nodeGrid;
    public List<NodeGridCost> finalPath;

    [HideInInspector]
    public float nodeDiameter;
    int gridSizeX, gridSizeY;


	// Use this for initialization
	void Awake ()
    {
        nodeDiameter = nodeRadius * 2;

        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

        CreateGrid();
    }

    

    void CreateGrid()
    {
        nodeGrid = new NodeGridCost[gridSizeX, gridSizeY];
        Vector3 bottonLeft = transform.position - new Vector3(gridSize.x / 2, 0, gridSize.y / 2);

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = bottonLeft + new Vector3(x * nodeDiameter + nodeRadius, 0, y * nodeDiameter + nodeRadius);

                Collider[] nodeCol = Physics.OverlapSphere(worldPoint, nodeRadius, nodeMask);

                if (nodeCol.Length != 0)
                {
                    nodeGrid[x, y] = new NodeGridCost(nodeCol[0].GetComponent<Node>(), x, y);
                }
                else
                {
                    nodeGrid[x, y] = new NodeGridCost(null, x, y);
                    nodeGrid[x, y].nullPosition = worldPoint;
                }
            }
        }
    }


    public NodeGridCost GetNodeInGrid(Node nodeToFind)
    {
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                if (nodeGrid[x, y].node == nodeToFind)
                {
                    return nodeGrid[x, y];
                }
            }
        }

        return null; //new NodeGridCost(null, -1, -1);
    }


    public List<NodeGridCost> GetNeighboringNodes(NodeGridCost homeNode)
    {
        List<NodeGridCost> neighboringNodes = new List<NodeGridCost>();

        int[] gridPos = homeNode.GridPos;
        int xCheck = homeNode.GridPos[0];
        int yCheck = homeNode.GridPos[1];

        int gridX = gridSizeX - 1;
        int gridY = gridSizeY - 1;


        //check x  nodes
        if (xCheck > 0) { neighboringNodes.Add(nodeGrid[xCheck - 1, yCheck]); }
        if (xCheck < gridX) { neighboringNodes.Add(nodeGrid[xCheck + 1, yCheck]); }

        //Check y nodes
        if (yCheck > 0) { neighboringNodes.Add(nodeGrid[xCheck, yCheck - 1]); }
        if (yCheck < gridY) { neighboringNodes.Add(nodeGrid[xCheck, yCheck + 1]); }


        //check xy nodes
        if (xCheck > 0 && yCheck > 0)
        {
            //Check if the nodes are covering it
            if (!nodeGrid[xCheck - 1, yCheck].node.IsObstructed() && !nodeGrid[xCheck, yCheck - 1].node.IsObstructed())
            {
                neighboringNodes.Add(nodeGrid[xCheck - 1, yCheck - 1]);
                //if(!nodeGrid[xCheck - 1, yCheck - 1].diagonalBlocked)
                //{
                //    nodeGrid[xCheck - 1, yCheck - 1].diagonalBlocked = true;
                //    nodeGrid[xCheck - 1, yCheck - 1].diagonalParentBlock[0] = nodeGrid[xCheck - 1, yCheck];
                //    nodeGrid[xCheck - 1, yCheck - 1].diagonalParentBlock[1] = nodeGrid[xCheck, yCheck - 1];

                //    if(xCheck - 1 == 10 && yCheck - 1 == 8)
                //    {
                //        Debug.Log("1");
                //    }
                //}
            }
        }
        if (xCheck < gridX && yCheck < gridY)
        {
            if (!nodeGrid[xCheck + 1, yCheck].node.IsObstructed() && !nodeGrid[xCheck, yCheck + 1].node.IsObstructed())
            {
                neighboringNodes.Add(nodeGrid[xCheck + 1, yCheck + 1]);
                //if (!nodeGrid[xCheck + 1, yCheck + 1].diagonalBlocked)
                //{
                //    nodeGrid[xCheck + 1, yCheck + 1].diagonalBlocked = true;
                //    nodeGrid[xCheck + 1, yCheck + 1].diagonalParentBlock[0] = nodeGrid[xCheck + 1, yCheck];
                //    nodeGrid[xCheck + 1, yCheck + 1].diagonalParentBlock[1] = nodeGrid[xCheck, yCheck + 1];

                //    if (xCheck + 1 == 10 && yCheck + 1 == 8)
                //    {
                //        Debug.Log("2");
                //    }
                //}
            }
        }
        if (xCheck < gridX && yCheck > 0)
        {
            if (!nodeGrid[xCheck + 1, yCheck].node.IsObstructed() && !nodeGrid[xCheck, yCheck - 1].node.IsObstructed())
            {
                neighboringNodes.Add(nodeGrid[xCheck + 1, yCheck - 1]);
                //if (!nodeGrid[xCheck + 1, yCheck - 1].diagonalBlocked)
                //{
                //    nodeGrid[xCheck + 1, yCheck - 1].diagonalBlocked = true;
                //    nodeGrid[xCheck + 1, yCheck - 1].diagonalParentBlock[0] = nodeGrid[xCheck + 1, yCheck];
                //    nodeGrid[xCheck + 1, yCheck - 1].diagonalParentBlock[1] = nodeGrid[xCheck, yCheck - 1];

                //    if (xCheck + 1 == 10 && yCheck - 1 == 8)
                //    {
                //        Debug.Log("3");
                //    }
                //}
            }
        }
        if (xCheck > 0 && yCheck < gridY)
        {
            if (!nodeGrid[xCheck - 1, yCheck].node.IsObstructed() && !nodeGrid[xCheck, yCheck + 1].node.IsObstructed())
            {
                neighboringNodes.Add(nodeGrid[xCheck - 1, yCheck + 1]);
                //if (!nodeGrid[xCheck - 1, yCheck + 1].diagonalBlocked)
                //{
                //    nodeGrid[xCheck - 1, yCheck + 1].diagonalBlocked = true;
                //    nodeGrid[xCheck - 1, yCheck + 1].diagonalParentBlock[0] = nodeGrid[xCheck - 1, yCheck];
                //    nodeGrid[xCheck - 1, yCheck + 1].diagonalParentBlock[1] = nodeGrid[xCheck, yCheck + 1];

                //    if (xCheck - 1 == 10 && yCheck + 1 == 8)
                //    {
                //        Debug.Log("4");
                //    }
                //}
            }
        }


        return neighboringNodes;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if (nodeGrid != null)
        {
            //foreach (NodeGridCost node in nodeGrid)
            //{
            //    if (node.node == null || node.node.IsObstructed())
            //    {
            //        Gizmos.color = Color.red;
            //    }
            //    else
            //    {
            //        Gizmos.color = Color.white;
            //    }

            //    if (node.node) { Gizmos.DrawCube(node.node.transform.position, Vector3.one * (nodeDiameter - distance)); }
            //    else { Gizmos.DrawCube(node.nullPosition, Vector3.one * (nodeDiameter - distance)); }
            //}

            if (finalPath != null)
            {
                foreach (NodeGridCost node in finalPath)
                {
                    if (node.diagonalParentBlock[0] != null)
                    {
                        if (node.diagonalBlocked)
                        {
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawCube(node.diagonalParentBlock[0].node.transform.position, Vector3.one * (nodeDiameter - distance));
                            Gizmos.DrawCube(node.diagonalParentBlock[1].node.transform.position, Vector3.one * (nodeDiameter - distance));
                        }
                    }

                    Gizmos.color = Color.green;

                    if (node.node) { Gizmos.DrawCube(node.node.transform.position, Vector3.one * (nodeDiameter - distance)); }
                    else { Gizmos.DrawCube(node.nullPosition, Vector3.one * (nodeDiameter - distance)); }
                }
            }
        }
    }
}




public class NodeGridCost
{
    public Node node;
    int gridX, gridY;
    public float gCost, hCost = 0;

    public Vector3 nullPosition;

    public NodeGridCost parent;

    public bool diagonalBlocked = false;
    public NodeGridCost[] diagonalParentBlock = new NodeGridCost[2];

    public bool tempBlocked = false;

    public int[] GridPos { get { return new int[] { gridX, gridY }; } }

    public float FCost { get { return gCost + hCost; } }


    public NodeGridCost(Node n, int xGrid, int yGrid)
    {
        node = n;
        gridX = xGrid;
        gridY = yGrid;
    }
}

