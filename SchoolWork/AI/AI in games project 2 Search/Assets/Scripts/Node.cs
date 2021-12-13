using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    bool isWall, isBuilding, isTurret = false;
    //bool isObstructed = false;

    GameObject wall;
    GameObject building;
    GameObject wallBuilding;
    GameObject turret;


    public int turretCost = 20;
    public int buildingCost = 10;

    [Range(0f, 1f)]
    public float sellingProcent = 0.5f;

    //public int gridX;
    //public int gridY;

    //public Node parent;

    //int gCost;
    //int hCost;
    //public int FCost { get { return gCost + hCost; } }


    //static List<Node> nodesList = new List<Node>();


    void Awake()
    {
        wall = transform.Find("Wall").gameObject;
        building = transform.Find("Building").gameObject;
        wallBuilding = transform.Find("WallBuilding").gameObject;
        turret = transform.Find("Turret").gameObject;

        if(isWall && isBuilding)
        {
            wallBuilding.SetActive(true);
            if (isTurret)
            {
                turret.SetActive(true);
            }
        }

        else if (isBuilding)
        {
            building.SetActive(true);
            if (isTurret)
            {
                turret.SetActive(true);
            }
        }

        else if (isWall)
        {
            wall.SetActive(true);
            if (isTurret)
            {
                turret.SetActive(false);
                isTurret = false;
            }
        }



        if (!isWall || !isBuilding)
        {
            wallBuilding.SetActive(false);
        }

        if (!isBuilding)
        {
            building.SetActive(false);
            if (isTurret)
            {
                turret.SetActive(false);
                isTurret = false;
            }
        }

        if (!isWall)
        {
            wall.SetActive(false);
        }
    }

    void Start()
    {
        //if(isWall || isBuilding)
        //{
        //    //isObstructed = true;
        //}
    }

    public void CreateBuilding()
    {
        isBuilding = true;
        //isObstructed = true;
    }

    public int DestroyBuilding()
    {
        if (isTurret)
        {
            isTurret = false;
            turret.SetActive(false);

            return Mathf.RoundToInt(turretCost * sellingProcent);
        }

        isBuilding = false;
        building.SetActive(false);

        return Mathf.RoundToInt(buildingCost * sellingProcent);
    }

    public bool Destructible()
    {
        if (isTurret)
        {
            return true;
        }

        else if (isBuilding && !isWall)
        {
            return true;
        }

        return false;
    }

    public int CheckUpgrade()
    {
        if(isWall && isBuilding && isTurret)
        {
            return -1;
        }

        else if (isWall && isBuilding)
        {
            return turretCost;
        }

        else if (isBuilding && isTurret)
        {
            return -1;
        }

        else if (isBuilding)
        {
            return turretCost;
        }

        else if (!isWall)
        {
            return buildingCost;
        }

        return -1;
    }

    public void UpgradeNode()
    {
        if (!isBuilding)
        {
            isBuilding = true;
            building.SetActive(true);
            return;
        }

        isTurret = true;
        turret.SetActive(true);
    }

    public bool IsObstructed()
    {
        bool obstructed = isWall || isBuilding;
        return obstructed;
        //return isObstructed;
    }

    //public Node(int a_gridX, int a_gridY)
    //{
    //    gridX = a_gridX;
    //    gridY = a_gridY;
    //}

    //void OnDrawGizmos()
    //{
    //    if (isObstructed)
    //    {
    //        if (isWall && isBuilding)
    //        {
    //            Gizmos.color = Color.yellow;
    //        }

    //        else if (isWall)
    //        {
    //            Gizmos.color = Color.black;
    //        }

    //        else if (isBuilding)
    //        {
    //            Gizmos.color = Color.green;
    //        }

    //        Gizmos.DrawCube(transform.position, Vector3.one);
    //    }
    //}
}
