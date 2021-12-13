using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeNode : MonoBehaviour {

    public LayerMask nodeMask;

    public List<PathFinding_AStar> pathList = new List<PathFinding_AStar>();

    float camrayLength = 100f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BuildNode();
        }

        else if (Input.GetMouseButtonDown(1))
        {
            UnBuildNode();
        }
	}

    void BuildNode()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit nodeHit;

        if (Physics.Raycast(camRay, out nodeHit, camrayLength, nodeMask))
        {
            Node node = nodeHit.collider.GetComponent<Node>();
            int cost = node.CheckUpgrade();

            //If the node is upgradable AND you have enough for the cost
            if (cost == -1 /* || currentMoney < cost*/)
            {
                Debug.Log("cost");
                return;
            }

            for (int i = 0; i < pathList.Count; i++)
            {
                if (!pathList[i].UpgradeableNode(node))
                {
                    Debug.Log("not upgradable");
                    return;
                }

                EnemySpawner enemySpaner = pathList[i].GetComponent<EnemySpawner>();
                if (enemySpaner.CheckObstructing(node))
                {
                    Debug.Log("Blocked by approaching/leaving enemy");
                    return;
                }

                if (enemySpaner.CheckObstructing(node))
                {
                    Debug.Log("Enemy Can't reach goal");
                    return;
                }
            }

            node.UpgradeNode();
            //apply cost


            for (int i = 0; i < pathList.Count; i++)
            {
                pathList[i].UpdatePath();
                pathList[i].GetComponent<EnemySpawner>().UpdateEnemyPath();
            }
        }
    }

    void UnBuildNode()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit nodeHit;

        if (Physics.Raycast(camRay, out nodeHit, camrayLength, nodeMask))
        {
            Node node = nodeHit.collider.GetComponent<Node>();

            if (node.Destructible())
            {
                node.DestroyBuilding();
            }

            if (!node.IsObstructed())
            {
                for (int i = 0; i < pathList.Count; i++)
                {
                    pathList[i].UpdatePath();
                    pathList[i].GetComponent<EnemySpawner>().UpdateEnemyPath();
                }
            }
        }
    }
}
