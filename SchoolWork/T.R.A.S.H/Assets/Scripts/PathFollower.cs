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

    public enum OverflowType
    {
        Cycle = 0,
        PingPong,
        Stop
    }

    public enum DestructionType
    {
        destroyPathAndObject = 0,
        destroyOnlyPath
    }

    public OverflowType overflowType;

    public bool destroyOnEndPoints = false;
    public DestructionType destructionType;



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

    public bool bouncingBetweenPoints = false;
    public Vector2 bounceBetweenPoints;

    private Timer stoppedTimer = new Timer();



    public GameObject objectPrefab;
    public float destroyObjectTimeAfterPathDestroyed = 20;


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


        objectToMove = Instantiate(objectPrefab ,curve[0].PositionWorld, Quaternion.Euler(0, 0, 0));


        GetComponent<BGCcCursorObjectTranslate>().ObjectToManipulate = objectToMove.transform;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        stoppedTimer.Time += Time.deltaTime;
        StopForTime();

        //=========================================== Moving a current point (and adjacent controls) slowly to their future positions
        // changing curve's attributes, like point's positions or controls fires curve.Changed event (in curve's LateUpdate)
        // math object uses this event to recalculate it's cached data (and it's relatively expensive operation).
        // we also use this event to call UpdateLineRenderer to update UI.


        CheckPoint3();

        ActOnActions();
    }

    //private void CheckPoint1()
    //{
    //    if (pointListIndex < pointActionList.Count)
    //    {
    //        if (startNextActionNextTurn == true)
    //        {
    //            startNextActionNextTurn = false;

    //            Debug.Log("Hit this turn");

    //            pointActionList[pointListIndex].StartAction(objectToMove);

    //            NextPointAction();
    //            distance = math.Math.GetDistance(pointListIndex);
    //        }


    //        if (cursor.Distance >= distance && movingBackwards == false)
    //        {
    //            Debug.Log("Hit");

    //            pointActionList[pointListIndex].StartAction(objectToMove);

    //            //pointListIndex++;
    //            NextPointAction();
    //            distance = math.Math.GetDistance(pointListIndex);
    //            //Set delay right at the beginning of update so it does not move to the point before this one
    //            //Or set speed to 0

    //            //cursorLinear.Delay = 1;
    //            //cursor.Distance = distance;
    //            //cursorLinear.Speed = 0;
    //            //PointMoveTime = 0;
    //        }

    //        else if (cursor.Distance <= distance && movingBackwards == true)
    //        {
    //            Debug.Log("Hit Backwards");
    //        }


    //        if (cursor.Distance + cursorLinear.Speed * Time.deltaTime >= distance
    //            && movingBackwards == false)
    //        {
    //            startNextActionNextTurn = true;
    //        }

    //        if (cursor.Distance + cursorLinear.Speed * Time.deltaTime <= distance
    //            && movingBackwards == true)
    //        {
    //            startNextActionNextTurn = true;
    //        }
    //    }
    //}

    //private void CheckPoint2()
    //{
    //    if (pointListIndex < pointActionList.Count)
    //    {
    //        if (startNextActionNextTurn == true)
    //        {
    //            startNextActionNextTurn = false;

    //            Debug.Log("Hit this turn");

    //            pointActionList[pointListIndex - 1].StartAction(objectToMove);

    //            //NextPointAction();
    //            distance = math.Math.GetDistance(pointListIndex);
    //        }


    //        if (cursor.Distance >= distance && movingBackwards == false)
    //        {
    //            Debug.Log("Hit");

    //            pointActionList[pointListIndex].StartAction(objectToMove);

    //            //pointListIndex++;
    //            NextPointAction();
    //            distance = math.Math.GetDistance(pointListIndex);
    //            //Set delay right at the beginning of update so it does not move to the point before this one
    //            //Or set speed to 0

    //            //cursorLinear.Delay = 1;
    //            //cursor.Distance = distance;
    //            //cursorLinear.Speed = 0;
    //            //PointMoveTime = 0;
    //        }

    //        else if (cursor.Distance <= distance && movingBackwards == true)
    //        {
    //            Debug.Log("Hit Backwards");
    //        }


    //        if (cursor.Distance + cursorLinear.Speed * Time.deltaTime >= distance
    //            && movingBackwards == false)
    //        {
    //            NextPointAction();
    //            startNextActionNextTurn = true;
    //        }

    //        if (cursor.Distance + cursorLinear.Speed * Time.deltaTime <= distance
    //            && movingBackwards == true)
    //        {
    //            startNextActionNextTurn = true;
    //        }
    //    }
    //}


    private void CheckPoint3()
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

            //If the pointIndex is higher than the total number of points
            //Overflow
            //if (pointIndex >= curve.PointsCount && movingBackwards == false)
            //{
            //    Debug.Log("Overflow");
            //    OverFlow();
            //}

            //else if (pointIndex < 0 && movingBackwards == true)
            //{
            //    Debug.Log("Overflow");
            //    OverFlow();
            //}
            //Sets new distance
            //distance = math.Math.GetDistance(pointIndex);
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
                //distance = math.Math.GetDistance(pointIndex);
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
                //distance = math.Math.GetDistance(pointIndex);
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
            //STOP!!!
            if (pointActionList[pointListIndex - 1].stoppedTime > 0)
            {
                Debug.Log("Stop!");
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

            //Plus the indexes
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
            //STOP!!!
            if (pointActionList[pointListIndex - 1].stoppedTime > 0)
            {
                Debug.Log("Stop!");
                stoppedTimer.Duration = pointActionList[pointListIndex - 1].stoppedTime;
                stoppedTimer.Time = 0;

                speedBeforeStopped = currentSpeed;
                StopForTime();
            }
        }


        //Bouncing
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

            else if (resetActionsBack == true)
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

            else if (resetActionsBack == true)
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


            //if (movingBackwards == false && repeatingCircle == true
            //    && (pointListIndex - circle > repeatCircle[c].actionB && pointListIndex - circle > repeatCircle[c].actionA)
            //    && repeatCircle[c].currentCircles < repeatCircle[c].numberOfCircles
            //    && repeatCircle[c].numberOfCircles > 0)
            //{
            //    if (repeatCircle[c].resetActionsInCircle == true)
            //    {
            //        pointListIndex = pointListIndex - (repeatCircle[c].actionB + 1 - repeatCircle[c].actionA);
            //        for (int i = repeatCircle[c].actionA; i <= repeatCircle[c].actionB; i++)
            //        {
            //            pointActionList[i].ResetActionTrigger();
            //        }
            //    }

            //    repeatCircle[c].currentCircles++;

            //    float distanceOver = cursor.Distance - math.Math.GetDistance(pointIndex - 1);

            //    pointIndex = repeatCircle[c].actionA;

            //    cursor.Distance = math.Math.GetDistance(pointIndex) + distanceOver;
            //}



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




            //else if (movingBackwards == true && repeatingCircle == true
            //    && (pointListIndex + circle > repeatCircle[c].actionB && pointListIndex + circle > repeatCircle[c].actionA)
            //    && repeatCircle[c].currentCircles < repeatCircle[c].numberOfCircles
            //    && repeatCircle[c].numberOfCircles > 0)

            //else if (movingBackwards == true && repeatingCircle == true
            //    && (pointListIndex > repeatCircle[c].actionB && pointListIndex > repeatCircle[c].actionA)
            //    && (pointIndex < repeatCircle[c].pointB && pointIndex < repeatCircle[c].pointB)
            //    && repeatCircle[c].currentCircles < repeatCircle[c].numberOfCircles
            //    && repeatCircle[c].numberOfCircles > 0)
            //{
            //    Debug.Log("Back circle");

            //    if (repeatCircle[c].resetActionsInCircle == true)
            //    {
            //        pointListIndex = pointListIndex - (repeatCircle[c].actionB - repeatCircle[c].actionA);
            //        for (int i = repeatCircle[c].actionA; i <= repeatCircle[c].actionB; i++)
            //        {
            //            pointActionList[i].ResetActionTrigger();
            //        }
            //    }

            //    repeatCircle[c].currentCircles++;


            //    float distanceOver = cursor.Distance - math.Math.GetDistance(pointIndex);

            //    pointIndex = repeatCircle[c].pointB;

            //    cursor.Distance = math.Math.GetDistance(pointIndex) + distanceOver;
            //}
        }


        //Overflow
        if (pointIndex >= curve.PointsCount && movingBackwards == false)
        {
            Debug.Log("Overflow");
            OverFlow();
        }

        else if (pointIndex < 0 && movingBackwards == true)
        {
            Debug.Log("Overflow");
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
            // Count turn and reset point index
            currentTurn++;
            pointIndex = 0;
            //If current turn is higher than number of turns, destroy
            if (currentTurn > numberOfTurns)
            {
                //Set so it moves to the next state
                //objectToMove.getComponent<finishedPath>.nextAction();
                //Destroy(gameObject); //Destroys the path and not the gameobject that moved
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

        else if (overflowType == OverflowType.Stop)
        {
            //Bool pathFinished = true;

            //Set so it moves to the next state
            //objectToMove.getComponent<finishedPath>.nextAction();
            //Destroy(gameObject); //Destroys the path and not the gameobject that moved
            //DestroyGameObject();
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

    //Destroys both the path and object it moves
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
    
    //Timer

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
