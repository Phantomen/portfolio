using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LasernodeBehaviour : MonoBehaviour {
    [SerializeField] GameObject laserNode;
    [SerializeField] int nodeAmount = 8;
    [SerializeField] float firstNodePositionX = -3f;
    [SerializeField] float NodePositionY = 2f;
    [SerializeField] float nodeLaserDelay = 2f;
    [SerializeField] Vector3 distanceFromCenter;
    [SerializeField] float secondsBeforeMovement;
    [SerializeField] float movementSpeed;
    public enum ActiveLaserPattern
    {
        LaserWavePattern,
        LaserCrossPattern,
        LaserFollowPattern
    }
    [SerializeField] ActiveLaserPattern SetLaserPattern;


    float mathshit;
    int listInteger = 0;
    float nodePosition = 0;
    List<GameObject> nodeList = new List<GameObject>();
    ObjectMover objectmover;
    float nodePositionY;
    float absolutePositionX;
    float absolutePositionY;
    Vector3 stageDimensions;
    bool nodeDone = false;
    bool lastNode = false;
    


    // Use this for initialization
    void Start () {
        mathshit = (Mathf.Abs(firstNodePositionX) * 2) / (nodeAmount - 1);
        stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        SendoutNode(nodeAmount);
        //objectmover = laserNode.GetComponent<ObjectMover>();
        moveNode();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(lastNode)
        {
            if (nodeLaserDelay <= 0)
            {
                activateLaser();
            }
            else
            {
                nodeLaserDelay -= Time.deltaTime;
            }
        }


	}

    void SendoutNode(int nodeAmount)
    {
        for (int i = 0; i < nodeAmount; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            Vector3 nodePosition = new Vector3(this.transform.position.x + distanceFromCenter.x, this.transform.position.y + distanceFromCenter.y, this.transform.position.z + distanceFromCenter.z);
            var lasernode = (GameObject)Instantiate(laserNode, nodePosition, rotation);
            nodeList.Add(lasernode);
        }
    }

    void moveNode()
    {
        for (int i = 0; i < nodeAmount; i++)
        {
            nodeList[i].GetComponent<ObjectMover>().moveTo = new Vector2(firstNodePositionX, NodePositionY);
            nodeList[i].GetComponent<ObjectMover>().willMove = true;
            firstNodePositionX += mathshit;
            if(i >= nodeAmount - 1)
            {
                lastNode = true;
            }
        }

    }

     void activateLaser()
    {
        for (int i = 0; i < nodeAmount; i++)
        {
            switch(SetLaserPattern)
            {
                case ActiveLaserPattern.LaserCrossPattern:
                    nodeList[i].GetComponent<LaserCrossPattern>().enabled = true;
                break;
                case ActiveLaserPattern.LaserFollowPattern:
                    nodeList[i].GetComponent<LaserFollowPattern>().enabled = true;
                break;
                case ActiveLaserPattern.LaserWavePattern:
                    nodeList[i].GetComponent<LaserPattern>().enabled = true;
                break;
            }

            
        }
    }
}
