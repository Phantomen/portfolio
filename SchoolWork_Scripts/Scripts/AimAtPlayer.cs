using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtPlayer : MonoBehaviour {

    private GameObject player;

    [Tooltip("List to turn")]
    [SerializeField]
    private List<TurnPoints> rotationList = new List<TurnPoints>();


    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        RotatePoints();
	}


    private void RotatePoints()
    {
        for (int rotIndex = 0; rotIndex < rotationList.Count; rotIndex++)
        {
            switch (rotationList[rotIndex].turnType)
            {
                case TurnPoints.TurnType.Constant:
                    RotateConstant(rotationList[rotIndex]);
                    break;


                case TurnPoints.TurnType.Instant:
                    RotateInstant(rotationList[rotIndex]);
                    break;


                case TurnPoints.TurnType.Lerp:
                    RotateLerp(rotationList[rotIndex]);
                    break;


                case TurnPoints.TurnType.Slerp:
                    RotateSlerp(rotationList[rotIndex]);
                    break;


                default:
                    Debug.Log("Error: Code ´´AimAtPlayer´´ does nothing with the TurnType: " + rotationList[rotIndex].turnType);
                    break;
            }
        }
    }


    private void RotateConstant(TurnPoints turnClass)
    {
        foreach (Transform rotPoint in turnClass.pointList)
        {
            //Relative aim point at player
            Vector2 targetDir = rotPoint.position - player.transform.position;

            //Target rotation is rotation it needs to have to aim directly at it
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.forward);
            //Makes sure that it rotates around the z axis
            targetRotation.x = 0;
            targetRotation.y = 0;

            Quaternion newRotation = targetRotation;

            //Current rotation
            Quaternion fromRotation = rotPoint.rotation;
            fromRotation.x = 0;
            fromRotation.y = 0;


            float targetRotZ = targetRotation.eulerAngles.z;
            float currentRotZ = fromRotation.eulerAngles.z;


            //If same side (left or right)
            //Increase rotation
            if (targetRotZ >= currentRotZ
                && ((targetRotZ >= 180 && currentRotZ >= 180)
                || (targetRotZ <= 180 && currentRotZ <= 180)))
            {
                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, 1, turnClass.turnSpeed));
            }

            //Decrease rotation
            else if (targetRotZ <= currentRotZ
                && ((targetRotZ >= 180 && currentRotZ >= 180)
                || (targetRotZ <= 180 && currentRotZ <= 180)))
            {
                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, -1, turnClass.turnSpeed));
            }


            //If from left side to right side
            //Increase rotation
            else if (targetRotZ <= currentRotZ + 180
                && targetRotZ >= 180 && currentRotZ <= 180)
            {
                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, 1, turnClass.turnSpeed));
            }

            //Decrease rotation
            else if (targetRotZ >= currentRotZ + 180
                && targetRotZ >= 180 && currentRotZ <= 180)
            {
                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngleSwitch(currentRotZ, targetRotation.eulerAngles.z, -1, turnClass.turnSpeed));
            }



            //If from right side to left side
            //Increase rotation
            else if (targetRotZ <= currentRotZ - 180
                && targetRotZ <= 180 && currentRotZ >= 180)
            {
                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngleSwitch(currentRotZ, targetRotation.eulerAngles.z, 1, turnClass.turnSpeed));
            }

            //Decrease rotation
            else if (targetRotZ >= currentRotZ - 180
                && targetRotZ <= 180 && currentRotZ >= 180)
            {
                newRotation.eulerAngles = new Vector3(0, 0, GetConstantAngle(currentRotZ, targetRotation.eulerAngles.z, -1, turnClass.turnSpeed));
            }

            //Set new rotation
            rotPoint.transform.rotation = newRotation;
        }
    }

    //Instantly rotates toward target
    private void RotateInstant(TurnPoints turnClass)
    {
        foreach (Transform rotPoint in turnClass.pointList)
        {
            Vector2 targetDir = rotPoint.position - player.transform.position;
            Quaternion newRotation = Quaternion.LookRotation(targetDir, Vector3.forward);
            newRotation.x = 0;
            newRotation.y = 0;

            rotPoint.rotation = newRotation;
        }
    }

    private void RotateLerp(TurnPoints turnClass)
    {
        foreach (Transform rotPoint in turnClass.pointList)
        {
            //Relative aim point
            Vector2 targetDir = rotPoint.position - player.transform.position;

            //Target rotation is rotation it needs to have to aim directly at it
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.forward);
            targetRotation.x = 0;
            targetRotation.y = 0;

            Quaternion newRotation = targetRotation;

            //Current rotation
            Quaternion fromRotation = rotPoint.rotation;
            fromRotation.x = 0;
            fromRotation.y = 0;

            //Lerp rotation
            newRotation = Quaternion.Lerp(fromRotation, targetRotation, Time.deltaTime * turnClass.turnSpeed * Mathf.PI / 180);

            rotPoint.transform.rotation = newRotation;
        }
    }

    private void RotateSlerp(TurnPoints turnClass)
    {
        foreach (Transform rotPoint in turnClass.pointList)
        {
            //Relative aim point
            Vector2 targetDir = rotPoint.position - player.transform.position;

            //Target rotation is rotation it needs to have to aim directly at it
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.forward);
            targetRotation.x = 0;
            targetRotation.y = 0;

            Quaternion newRotation = targetRotation;

            //Current rotation
            Quaternion fromRotation = rotPoint.rotation;
            fromRotation.x = 0;
            fromRotation.y = 0;

            //Slerp rotation
            newRotation = Quaternion.Slerp(fromRotation, targetRotation, Time.deltaTime * turnClass.turnSpeed * Mathf.PI / 180);

            rotPoint.transform.rotation = newRotation;
        }
    }


    private float GetConstantAngle(float currentRotZ, float targetRotation, int horizontal, float turningSpeed)
    {
        //the target rotation for constant angle rotation
        float targetRotZ = currentRotZ + horizontal * turningSpeed * Time.deltaTime;

        //if the turn took it past target, aim directly at target
        if ((targetRotZ > targetRotation
            && horizontal > 0)
            ||
            (targetRotZ < targetRotation
            && horizontal < 0))
        {
            targetRotZ = targetRotation;
        }

        //Return rotation
        return targetRotZ;
    }

    private float GetConstantAngleSwitch(float currentRotZ, float targetRotation, int horizontal, float turningSpeed)
    {
        //the target rotation for constant angle rotation
        float targetRotZ = currentRotZ + horizontal * turningSpeed * Time.deltaTime;

        //If rotation is or over 360 and turning right, subtract 360
        if (targetRotZ >= 360 && horizontal > 0)
        {
            targetRotZ -= 360;
        }

        //else if rotation is or under 0 and turning left, add 360
        else if (targetRotZ <= 0 && horizontal < 0)
        {
            targetRotZ += 360;
        }

        //if the turn took it past target, aim directly at target
        if ((targetRotZ > targetRotation
            && horizontal > 0
            && targetRotZ < targetRotation)
            ||
            (targetRotZ < targetRotation
            && horizontal < 0
            && targetRotZ > targetRotation))
        {
            targetRotZ = targetRotation;
        }

        //return rotation
        return targetRotZ;
    }
}


//Class contaning the list of the transforms, the way it turns and how quick
[System.Serializable]
public class TurnPoints
{
    [Tooltip("List of the points that turn the same way")]
    public List<Transform> pointList = new List<Transform>();

    public enum TurnType
    {
        Constant = 0,
        Instant,
        Lerp,
        Slerp
    }

    [Tooltip("The way it turns towards the player")]
    public TurnType turnType;


    [Tooltip("How fast it turns")]
    public float turnSpeed;
}