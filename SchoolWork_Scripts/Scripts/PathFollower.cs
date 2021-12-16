using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;

[RequireComponent(typeof(BGCcMath))]
public class PathFollower : MonoBehaviour {

    public float startingSpeed = 1;
    private float currentSpeed;
    private float speedBeforeStopped;

    //What to do when it goes over the first or last point in the curve
    public enum OverflowType
    {
        Stop = 0,
        Cycle = 1,
        PingPong = 2
    }

    //Destroy gameObject with self when the path is done
    public enum DestructionType
    {
        destroyPathAndObject = 0,
        destroyOnlyPath
    }

    public OverflowType overflowType;


    public bool destroyOnEndPoints = false;
    public DestructionType destructionType;


    //when the cycle starts again, do every action in the same order again
    public bool resetActionAfterCycle = true;

    //Do the first action at the last point
    public bool sameActionsBack = false;

    //Do the same action at the same point
    public bool resetActionsBack = false;
    //If both are false there are new specific actions for the same points on the way back



    //If true, after a complete pingpong (back on posiiton one)
    //Do the same actions again (reset index)
    public bool resetPingPongAfterCompleted = true;


    public int numberOfTurns = 0;
    private int currentTurn = 0;


    public List<PointAction> pointActionList = new List<PointAction>();
    private int pointIndex = 0;
    private int pointListIndex = 0;


    //vector3 (point 1, point 2, number of times)
    public bool repeatingCircle;
    public List<Circling> repeatCircle = new List<Circling>();

    //Shall it bounce between points?
    public bool bouncingBetweenPoints = false;
    //What points
    public Vector2 bounceBetweenPoints;


    private Timer stoppedTimer = new Timer();

    //The prefab it spawns
    public GameObject objectPrefab;
    //If DestroyPathOnly, it will destroy the object after seconds
    public float destroyObjectTimeAfterPathDestroyed = 20;


    //Reference to object it moves
    private GameObject objectToMove;



    //Components of the BGCurve
    private BGCurve curve;
    private BGCcMath math;
    private BGCcCursor cursor;
    private BGCcCursorChangeLinear cursorLinear;

    //private const float PointMoveTime = 2f;

    private float distance;

    //private float started;


    private bool startNextActionNextTurn = false;



    private bool movingBackwards = false;



    // Use this for initialization
    void Start () {
        curve = GetComponent<BGCurve>();
        math = GetComponent<BGCcMath>();
        cursor = GetComponent<BGCcCursor>();
        cursorLinear = GetComponent<BGCcCursorChangeLinear>();

        //startingspeed = objectPrefab.GetComponent<XXXXXX>().speed;

        currentSpeed = startingSpeed;
        speedBeforeStopped = startingSpeed;
        cursorLinear.Speed = startingSpeed;

        //distance = math.Math[pointListIndex].DistanceFromStartToOrigin;
        distance = math.Math.GetDistance(pointIndex);

        //Instantiate the object
        objectToMove = Instantiate(objectPrefab ,curve[0].PositionWorld, Quaternion.Euler(0, 0, 0));

        //Sets the object so that the path moves the object
        GetComponent<BGCcCursorObjectTranslate>().ObjectToManipulate = objectToMove.transform;


        //Sets the overflow to the right one in BGCCursorChangeLinear
        switch (overflowType)
        {
            case OverflowType.Stop:
                cursorLinear.OverflowControl = BGCcCursorChangeLinear.OverflowControlEnum.Stop;
                break;

            case OverflowType.Cycle:
                cursorLinear.OverflowControl = BGCcCursorChangeLinear.OverflowControlEnum.Cycle;
                break;

            case OverflowType.PingPong:
                cursorLinear.OverflowControl = BGCcCursorChangeLinear.OverflowControlEnum.PingPong;
                break;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        //If the object it's supposed to move is null, destroy yourself
        if (objectToMove == null)
        {
            Destroy(gameObject);
        }

        stoppedTimer.Time += Time.deltaTime;
        StopForTime();

        CheckPoint();

        ActOnActions();
    }


    //The script that checks if the object passed a point
    private void CheckPoint()
    {
        //Activates the next action
        if (startNextActionNextTurn == true)
        {
            startNextActionNextTurn = false;

            //"Starts" the action
            pointActionList[pointListIndex].StartAction(objectToMove);

            //If speed is set as the beginning speed, reset speed
            if (pointActionList[pointListIndex].startSpeed == true)
            {
                if (movingBackwards == false)
                {
                NewSpeed(startingSpeed);
                }

                else
                {
                    NewSpeed(-startingSpeed);
                }
            }

            //Else if the speed will change and the speed is higher than zero, change it
            else if (pointActionList[pointListIndex].changeSpeed == true
                && pointActionList[pointListIndex].newSpeed > 0)
            {
                if (movingBackwards == false)
                {
                    NewSpeed(pointActionList[pointListIndex].newSpeed);
                }

                else
                {
                    NewSpeed(-pointActionList[pointListIndex].newSpeed);
                }
            }

            NextPointAction();
        }


        if (movingBackwards == false)
        {
            //If current distance is higher or equal to next point distance and not moving backwards
            if (cursor.Distance >= distance)
            {
                pointActionList[pointListIndex].StartAction(objectToMove);

                //Sets new speed
                if (pointActionList[pointListIndex].startSpeed == true)
                {
                    if (movingBackwards == false)
                    {
                        NewSpeed(startingSpeed);
                    }

                    else
                    {
                        NewSpeed(-startingSpeed);
                    }
                }

                else if (pointActionList[pointListIndex].changeSpeed == true
                    && pointActionList[pointListIndex].newSpeed > 0)
                {
                    if (movingBackwards == false)
                    {
                        NewSpeed(pointActionList[pointListIndex].newSpeed);
                    }

                    else
                    {
                        NewSpeed(-pointActionList[pointListIndex].newSpeed);
                    }
                }

                NextPointAction();
            }

            //Else if next time it will move, it will cross a point
            else if (cursor.Distance + cursorLinear.Speed * Time.deltaTime >= distance)
            {
                startNextActionNextTurn = true;
            }
        }

        else if (movingBackwards == true)
        {
            //If current distance is lower or equal to next point distance and not moving backwards
            if (cursor.Distance <= distance)
            {
                pointActionList[pointListIndex].StartAction(objectToMove);

                //Sets new speed
                if (pointActionList[pointListIndex].startSpeed == true)
                {
                    if (movingBackwards == false)
                    {
                        NewSpeed(startingSpeed);
                    }

                    else
                    {
                        NewSpeed(-startingSpeed);
                    }
                }

                else if (pointActionList[pointListIndex].changeSpeed == true
                    && pointActionList[pointListIndex].newSpeed > 0)
                {
                    if (movingBackwards == false)
                    {
                        NewSpeed(pointActionList[pointListIndex].newSpeed);
                    }

                    else
                    {
                        NewSpeed(-pointActionList[pointListIndex].newSpeed);
                    }
                }

                NextPointAction();
            }

            //Else if next time it will move, it will cross a point
            else if (cursor.Distance + cursorLinear.Speed * Time.deltaTime <= distance)
            {
                startNextActionNextTurn = true;
            }
        }
    }



    private void NextPointAction()
    {
        if (movingBackwards == false)
        {
            //for each action in list, add point
            for (int i = 0; i < pointListIndex; i++)
            {
                pointActionList[i].currentPointAfter++;
            }

            //Plus the indexes
            pointListIndex++;
            pointIndex++;

            //If it moved past a point that had a stoptime
            //Move the object to it and stop
            if (pointActionList[pointListIndex - 1].stoppedTime > 0)
            {
                stoppedTimer.Duration = pointActionList[pointListIndex - 1].stoppedTime;
                stoppedTimer.Time = 0;

                speedBeforeStopped = currentSpeed;
                StopForTime();
            }
        }

        else if (movingBackwards == true)
        {
            //for each action in list, add point
            for (int i = 0; i < pointListIndex; i++)
            {
                pointActionList[i].currentPointAfter++;
            }

            //"Plus" the indexes
            if (sameActionsBack == true)
            {
                pointListIndex--;
            }

            else if (resetActionsBack == true)
            {
                pointListIndex++;
            }

            pointIndex--;

            //If it moved past a point that had a stoptime
            //Move the object to it and stop
            if (pointActionList[pointListIndex + 1].stoppedTime > 0)
            {
                Debug.Log("Stop!");
                stoppedTimer.Duration = pointActionList[pointListIndex - 1].stoppedTime;
                stoppedTimer.Time = 0;

                speedBeforeStopped = currentSpeed;
                StopForTime();
            }
        }


        //Bouncing back
        if (movingBackwards == false && bouncingBetweenPoints == true && pointIndex > bounceBetweenPoints.x)
        {
            movingBackwards = !movingBackwards;
            NewSpeed(-currentSpeed);

            pointIndex -= 2;

            if (resetActionsBack == true)
            {
                for (int i = 0; i < repeatCircle.Count; i++)
                {
                    repeatCircle[i].currentCircles = 0;
                }


                pointListIndex -= 2;

                for (int i = 0; i < pointActionList.Count; i++)
                {
                    pointActionList[i].ResetActionTrigger(objectToMove);
                }
            }

            else if (resetActionsBack == false)
            {
                for (int i = 0; i < repeatCircle.Count; i++)
                {
                    repeatCircle[i].currentCircles = 0;
                }


                pointListIndex = int.Parse(bounceBetweenPoints.y.ToString());

                for (int i = 0; i < pointActionList.Count; i++)
                {
                    pointActionList[i].ResetActionTrigger(objectToMove);
                }
            }

            float distanceOver = cursor.Distance - math.Math.GetDistance(pointIndex + 1);
            cursor.Distance += -distanceOver;

            distance = math.Math.GetDistance(pointIndex);
        }

        //Bouncing forward
        else if (movingBackwards == true && bouncingBetweenPoints == true && pointIndex < bounceBetweenPoints.y)
        {
            movingBackwards = !movingBackwards;
            NewSpeed(-currentSpeed);

            pointIndex += 2;

            if (resetActionsBack == true)
            {
                for (int i = 0; i < repeatCircle.Count; i++)
                {
                    repeatCircle[i].currentCircles = 0;
                }

                pointListIndex += 2;

                for (int i = 0; i < pointActionList.Count; i++)
                {
                    pointActionList[i].ResetActionTrigger(objectToMove);
                }
            }

            else if (resetActionsBack == false)
            {
                for (int i = 0; i < repeatCircle.Count; i++)
                {
                    repeatCircle[i].currentCircles = 0;
                }


                pointListIndex = int.Parse(bounceBetweenPoints.y.ToString());

                for (int i = 0; i < pointActionList.Count; i++)
                {
                    pointActionList[i].ResetActionTrigger(objectToMove);
                }
            }

            float distanceOver = cursor.Distance - math.Math.GetDistance(pointIndex - 1);
            cursor.Distance += distanceOver;

            distance = math.Math.GetDistance(pointIndex);
        }


        //Circling
        for (int c = 0; c < repeatCircle.Count; c++)
        {
            int circle = 0;

            if (repeatCircle[c].resetActionsInCircle == true) { }

            else if (repeatCircle[c].resetActionsInCircle == false)
            {
                circle = repeatCircle[c].currentCircles;
            }


            if (movingBackwards == false && repeatingCircle == true
                && (pointListIndex > repeatCircle[c].actionB && pointListIndex > repeatCircle[c].actionA)
                && (pointIndex > repeatCircle[c].pointB && pointIndex > repeatCircle[c].pointB)
                && repeatCircle[c].currentCircles < repeatCircle[c].numberOfCircles
                && repeatCircle[c].numberOfCircles > 0)
            {
                if (repeatCircle[c].resetActionsInCircle == true)
                {
                    pointListIndex = pointListIndex - (repeatCircle[c].actionB + 1 - repeatCircle[c].actionA);
                    for (int i = repeatCircle[c].actionA; i <= repeatCircle[c].actionB; i++)
                    {
                        pointActionList[i].ResetActionTrigger(objectToMove);
                    }
                }

                repeatCircle[c].currentCircles++;

                float distanceOver = cursor.Distance - math.Math.GetDistance(pointIndex - 1);

                pointIndex = repeatCircle[c].actionA;

                cursor.Distance = math.Math.GetDistance(pointIndex) + distanceOver;
            }
        }


        //Overflow
        if (pointIndex >= curve.PointsCount && movingBackwards == false)
        {
            OverFlow();
        }

        else if (pointIndex < 0 && movingBackwards == true)
        {
            OverFlow();
        }

        distance = math.Math.GetDistance(pointIndex);
    }

    private void OverFlow()
    {
        if (destroyOnEndPoints == true)
        {
            DestroyGameObject();
        }

        if (overflowType == OverflowType.Cycle)
        {
            //Count turn and reset point index
            currentTurn++;
            pointIndex = 0;
            //If current turn is higher than number of turns, destroy
            if (currentTurn > numberOfTurns)
            {
                DestroyGameObject();
            }

            else
            {
                //If the actions will be reset after completed cycle
                if (resetActionAfterCycle == true)
                {
                    pointListIndex = 0;
                    for (int i = 0; i < pointActionList.Count; i++)
                    {
                        pointActionList[i].ResetActionTrigger(objectToMove);
                    }

                    for (int i = 0; i < repeatCircle.Count; i++)
                    {
                        repeatCircle[i].currentCircles = 0;
                    }
                }
            }
        }

        //Pingpongs the object back
        else if (overflowType == OverflowType.PingPong)
        {
            if (pointIndex > 0)
            {
                pointIndex--;

                movingBackwards = true;


                if (sameActionsBack == true)
                {
                    pointListIndex = 0;
                    for (int i = 0; i < pointActionList.Count; i++)
                    {
                        pointActionList[i].ResetActionTrigger(objectToMove);
                    }
                }
            }

            //If the next point is the one before the first one
            //Make it move forward and the point index is 0
            else if (pointIndex < 0)
            {
                currentTurn++;
                movingBackwards = false;
                pointIndex = 0;

                if (currentTurn > numberOfTurns)
                {
                    DestroyGameObject();
                }

                else
                {
                    if (resetPingPongAfterCompleted == true)
                    {
                        pointListIndex = 0;
                        for (int i = 0; i < pointActionList.Count; i++)
                        {
                            pointActionList[i].ResetActionTrigger(objectToMove);
                        }
                    }
                }
            }
        }

        //If overflow is stop, destroy
        else if (overflowType == OverflowType.Stop)
        {
            pointIndex = 0;
            DestroyGameObject();
        }
    }

    private void ActOnActions()
    {
        //Does the actions that can and will do something
        for (int i = 0; i < pointActionList.Count; i++)
        {
            if (pointActionList[i].currentlyDoingSomething == true)
            {
                pointActionList[i].Action(objectToMove);
            }
        }
    }

    //Destroys both the path and maybe the object it will move
    private void DestroyGameObject()
    {
        if (destructionType == DestructionType.destroyPathAndObject)
        {
            Destroy(gameObject);
            Destroy(objectToMove);
        }

        else if (destructionType == DestructionType.destroyOnlyPath)
        {
            Destroy(gameObject);

            Destroy(objectToMove, destroyObjectTimeAfterPathDestroyed);
        }
    }


    private void StopForTime()
    {
        //If expired, start moving again
        if (stoppedTimer.Expired == true && currentSpeed == 0)
        {
            NewSpeed(speedBeforeStopped);
        }

        //else, stop moving
        else if (stoppedTimer.Expired == false)
        {
            NewSpeed(0);

            //if the position of the object and the point is off, move the object to the point
            if (cursor.Distance != math.Math.GetDistance(pointIndex - 1))
            {
                cursor.Distance = math.Math.GetDistance(pointIndex - 1);
            }
        }
    }

    //Sets new speed
    private void NewSpeed(float speed)
    {
        currentSpeed = speed;
        cursorLinear.Speed = currentSpeed;
    }
}


//The circle class
[System.Serializable]
public class PointAction
{
    public bool doSomething = false;
    [HideInInspector] public bool currentlyDoingSomething = false;
    [HideInInspector] public bool beenTriggered = false;

    public bool changeSpeed = false;
    public bool startSpeed = false;
    public float newSpeed = 0.5f;


    //List of diffrent shooting patterns
    public List<ShooterPattern> shootPatterns = new List<ShooterPattern>();


    //The time that it will be stopped (movement)
    public float stoppedTime = 0;

    //Stop the action after x amount of points
    public int stopActionAfterPoints = 0;
    [HideInInspector] public int currentPointAfter = 0;

    public bool stopActionAfterTime = false;
    public float stopActionTime = 0;
    [HideInInspector] public Timer stopActionTimer;

    public void StartAction(GameObject objectToStart)
    {
        //If it will do something and it hasn't been triggered
        if (doSomething == true)
        {
            if (beenTriggered == false)
            {
                //Resets
                for (int i = 0; i < shootPatterns.Count; i++)
                {
                    if (shootPatterns[i] != null)
                    {
                        //Creates a new instance of the code and resets things in it
                        //A new instance this object has a own instance and doesn't share with others
                        shootPatterns[i] = ShooterPattern.Instantiate(shootPatterns[i].GetComponent<ShooterPattern>());
                        shootPatterns[i].Reset(objectToStart);
                    }
                }
            }

            //Sets so it will and can do the action
            currentlyDoingSomething = true;
            beenTriggered = true;
            currentPointAfter = 0;

            stopActionTimer = new Timer(stopActionTime, 0);
        }
    }

    //Resets the action trigger
    public void ResetActionTrigger(GameObject objectToShoot)
    {
        currentlyDoingSomething = false;
        beenTriggered = false;
        currentPointAfter = 0;
        stopActionTimer = new Timer(stopActionTime, 0);

        for (int i = 0; i < shootPatterns.Count; i++)
        {
            if (shootPatterns[i] != null)
            {
                shootPatterns[i].Reset(objectToShoot);
            }
        }
    }

    //The action
    public void Action(GameObject objectToShoot)
    {
        //If it can and will move
        if (currentlyDoingSomething == true
            && doSomething == true
            && currentPointAfter <= stopActionAfterPoints)
        {
            //For everypattern this action has. shoot
            for (int i = 0; i < shootPatterns.Count; i++)
            {
                //Tells the pattern to try to shot, unless theres no pattern
                if (shootPatterns[i] != null)
                {
                    shootPatterns[i].Shoot(objectToShoot);
                }
            }
        }

        //Else, stop doing something
        else
        {
            currentlyDoingSomething = false;
        }
    }
}

[System.Serializable]
public class Circling
{
    public int actionA;
    public int actionB;

    public int pointA;
    public int pointB;

    //Do the same actions back
    public bool resetActionsInCircle = true;

    public int numberOfCircles = 1;
    [HideInInspector] public int currentCircles = 0;
}
