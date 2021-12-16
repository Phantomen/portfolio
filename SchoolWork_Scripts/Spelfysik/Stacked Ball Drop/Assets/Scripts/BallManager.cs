using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallManager : MonoBehaviour {

    public GameObject ballPrefab;

    public GameObject validText;

    public GameObject simulationCreateCanvas;
    public GameObject simulationCanvas;

    public MainCameraFollow mainCam;
    public BallCamera ballCam;

    [HideInInspector]
    public float sizeMultiplier = 1.2f;
    public InputField sizeMultUIField;

    [HideInInspector]
    public float weightMultiplier = 2f;
    public InputField weightMultUIField;

    [HideInInspector]
    public float ballDiameter, ballWeight = 1f;
    public InputField ballDiameterUIField, ballWeightUIField;

    [HideInInspector]
    public float dropHeight = 1f;
    public InputField dropHeightUIField;

    [HideInInspector]
    public int numberOfBalls = 1;
    public InputField ballNumberUIField;

    public Slider ballToFollowSlider;

    public float rollVelocity = 0.1f;

    private List<ForceObject> balls = new List<ForceObject>();

    private bool firstBallBounced = false;

    [HideInInspector]
    public WorldProperties worldProp;

    public float ballPosOffset = 0.001f;

    private bool simulating = false;

    public Text heightUIText;
    public Text velocityUIText;
    public Text maxHeightUIText;
    private float currentMaxHeight = 0;

    public ForceObject[] GetCameraBalls()
    {
        ForceObject[] ballsCameraList = new ForceObject[3];
        ballsCameraList[0] = balls[0];
        ballsCameraList[1] = balls[balls.Count - 1];

        return ballsCameraList;
    }


	// Use this for initialization
	void Start ()
    {
        worldProp = GetComponent<WorldProperties>();

        UpdateCreationBalls();

        simulationCreateCanvas.SetActive(true);
        simulationCanvas.SetActive(false);
    }

    void LateUpdate()
    {
        heightUIText.text = balls[(int)ballToFollowSlider.value - 1].transform.position.y.ToString();
        velocityUIText.text = balls[(int)ballToFollowSlider.value - 1].GetVelocity().y.ToString();
    }

    private void CreateNewBalls()
    {
        float currentWeightMult = 1;
        float currentSizeMult = 1;
        for (int i = 0; i < numberOfBalls - 1; i++)
        {
            currentWeightMult *= weightMultiplier;
            currentSizeMult *= sizeMultiplier;
        }

        float currentDropHeight = dropHeight;

        for (int i = 0; i < numberOfBalls; i++)
        {
            GameObject ball = GameObject.Instantiate(ballPrefab, new Vector3(0, currentDropHeight + (ballDiameter * currentSizeMult) / 2, 0), new Quaternion(), transform);
            ball.GetComponent<BallProperties>().InstanciateBall((ballDiameter * currentSizeMult) / 2, ballWeight * currentWeightMult);

            currentDropHeight += ballDiameter * currentSizeMult + ballPosOffset;
            currentWeightMult /= weightMultiplier;
            currentSizeMult /= sizeMultiplier;

            ball.name = "Ball " + (i + 1);

            balls.Add(ball.GetComponent<ForceObject>());

            Renderer render = ball.GetComponent<Renderer>();

            render.material = new Material(render.material);

            render.material.color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
                );
        }

        currentMaxHeight = balls[numberOfBalls - 1].transform.position.y - balls[numberOfBalls - 1].GetRadius;
        maxHeightUIText.text = currentMaxHeight.ToString();

        UpdateCameras(numberOfBalls - 1);
    }

    private void UpdateCameras(int ballTarget)
    {
        ForceObject[] ballsCameraList = new ForceObject[2];
        ballsCameraList[0] = balls[0];
        ballsCameraList[1] = balls[ballTarget];

        mainCam.UpdateCameraTarget(ballsCameraList);

        ballCam.UpdateCameraTarget(balls[ballTarget].gameObject, balls[ballTarget].GetRadius, balls[0].GetRadius);
    }

	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (simulating)
        {
            if (balls.Count > 0)
            {
                //AddGravityForce();
                firstBallBounced = false;
                BallBounce();
                AddGravityForce();
                AirDrag();

                MoveBalls();
            }

            if (currentMaxHeight < balls[numberOfBalls - 1].transform.position.y - balls[numberOfBalls - 1].GetRadius)
            {
                currentMaxHeight = balls[numberOfBalls - 1].transform.position.y - balls[numberOfBalls - 1].GetRadius;

                maxHeightUIText.text = currentMaxHeight.ToString();
            }
        }
	}

    private void AddGravityForce()
    {
        for(int i = 0; i< balls.Count; i++)
        {
            float gravitalForce = -worldProp.worldGravity * balls[i].GetMass;
            balls[i].ApplyForce(new Vector3(0, gravitalForce, 0));
        }
    }


    private void AirDrag()
    {
        for(int i = 0; i < balls.Count; i++)
        {
            float vel = balls[i].GetVelocity().y;
            float radius = balls[i].GetRadius;
            //0.47 is drag coefficient of a sphere      PI * radius^2 is the cross section area of a sphere (cirlce)
            float dragForce = (worldProp.airDensity / 2) * vel * vel * 0.47f * (Mathf.PI * radius * radius) * Time.fixedDeltaTime;    
            if(vel > 0)
            {
                dragForce = -dragForce;
            }

            balls[i].ApplyForce(new Vector3(0, dragForce, 0));
        }
    }


    private void BallBounce()
    {
        List<int> indexListCollision = new List<int>();

        if(balls[0].transform.position.y < balls[0].GetRadius)
        {
            firstBallBounced = true;
            balls[0].transform.position += new Vector3(0, balls[0].GetRadius - balls[0].transform.position.y, 0);
            FirstBallHitGround();
        }

        for (int i = 0; i < balls.Count - 1; i++)
        {
            //if ball 1 is higher than ball 2 minus the radius of ball 1 & 2 
            if (balls[i].transform.position.y > balls[i + 1].transform.position.y - balls[i + 1].GetRadius - balls[i].GetRadius)
            {
                indexListCollision.Add(i + 1);

                float yDist = balls[i].transform.position.y + balls[i].GetRadius + balls[i + 1].GetRadius + ballPosOffset;
                balls[i + 1].transform.position = new Vector3(0, yDist, 0);

                ResolveCollisionTwoBalls(i, i + 1);
            }
        }

        List<int> indexListCollisionTemp = new List<int>();
        for (int i = indexListCollision.Count - 1; i >= 0; i--)
        {
            //If moving down and the ball underneath is either moving down but faster or moving down while the ball underneath is going up
            if(balls[indexListCollision[i]].GetVelocity().y < 0
                && balls[indexListCollision[i]].GetVelocity().y < balls[indexListCollision[i] - 1].GetVelocity().y)
            {
                indexListCollisionTemp.Add(indexListCollision[i]);

                ResolveCollisionTwoBalls(indexListCollision[i], indexListCollision[i] - 1);
            }
        }

        if (balls[0].GetVelocity().y < 0 && firstBallBounced)
        {
            FirstBallHitGround();
        }

        //call new function to check temp indexlist
        if (indexListCollisionTemp.Count > 0)
        {

            indexListCollisionTemp.Reverse();
            BallCollision(indexListCollisionTemp);
        }
    }

    private void ResolveCollisionTwoBalls(int a, int b)
    {
        float j = -(1 + worldProp.elasticityCoefficient) * (balls[a].GetVelocity().y - balls[b].GetVelocity().y);
        j = j / (1 / balls[a].GetMass + 1 / balls[b].GetMass);
        j = j * worldProp.energyKept;

        float newVelA = balls[a].GetVelocity().y + (j / balls[a].GetMass);

        float newVelB = balls[b].GetVelocity().y - (j / balls[b].GetMass);

        balls[a].SetVelocity(new Vector3(0, newVelA, 0));
        balls[b].SetVelocity(new Vector3(0, newVelB, 0));

        if (newVelA > 0 && newVelA <= rollVelocity * a * 1.2f
            && (worldProp.elasticityCoefficient * worldProp.energyKept * worldProp.groundBounce * (1 - worldProp.airDensity) != 1))
        {
            balls[a].SetVelocity(new Vector3(0, worldProp.worldGravity * Time.fixedDeltaTime, 0));
        }

        if (newVelB > 0 && newVelB <= rollVelocity * b * 1.2f
            && (worldProp.elasticityCoefficient * worldProp.energyKept * worldProp.groundBounce * (1 - worldProp.airDensity) != 1))
        {
            balls[b].SetVelocity(new Vector3(0, worldProp.worldGravity * Time.fixedDeltaTime, 0));
        }
    }


    private void BallCollision(List<int> indexListCollision)
    {
        List<int> indexListCollisionUpTemp = new List<int>();

        for(int i = 0; i < indexListCollision.Count; i++)
        {
            ////If balls are going in opposite directions or going in the same but one is going faster thatn the other towards the slower ball
            //if ((balls[indexListCollision[i] - 1].GetVelocity().y * balls[indexListCollision[i]].GetVelocity().y <= 0 && (balls[indexListCollision[i] - 1].GetVelocity().y > 0 || balls[indexListCollision[i]].GetVelocity().y < 0))
            //    ||
            //    (balls[indexListCollision[i] - 1].GetVelocity().y * balls[indexListCollision[i]].GetVelocity().y > 0
            //    && (balls[indexListCollision[i]].GetVelocity().y < 0 && balls[indexListCollision[i]].GetVelocity().y < balls[indexListCollision[i] - 1].GetVelocity().y || balls[indexListCollision[i]].GetVelocity().y > 0 && balls[indexListCollision[i] - 1].GetVelocity().y > balls[indexListCollision[i]].GetVelocity().y)))

            //Ball under is going up and is going up faster than the ball above
            if (balls[indexListCollision[i] - 1].GetVelocity().y > 0
                && balls[indexListCollision[i] - 1].GetVelocity().y > balls[indexListCollision[i]].GetVelocity().y)
            {
                ResolveCollisionTwoBalls(indexListCollision[i], indexListCollision[i] - 1);
                indexListCollisionUpTemp.Add(indexListCollision[i]);
            }
        }

        List<int> indexListCollisionDownTemp = new List<int>();
        for (int i = indexListCollisionUpTemp.Count - 1; i >= 0; i--)
        {
            if (balls[indexListCollisionUpTemp[i]].GetVelocity().y < 0
                && balls[indexListCollisionUpTemp[i]].GetVelocity().y < balls[indexListCollisionUpTemp[i] - 1].GetVelocity().y)
            {
                ResolveCollisionTwoBalls(indexListCollisionUpTemp[i], indexListCollisionUpTemp[i] - 1);
                indexListCollisionDownTemp.Add(indexListCollisionUpTemp[i]);
            }
        }

        if (balls[0].GetVelocity().y < 0 && firstBallBounced)
        {
            FirstBallHitGround();
        }

        //Do it again
        if(indexListCollisionDownTemp.Count > 0)
        {
            indexListCollisionDownTemp.Reverse();
            BallCollision(indexListCollisionDownTemp);
        }
    }


    private void FirstBallHitGround()
    {
        balls[0].SetVelocity(-balls[0].GetVelocity() * worldProp.groundBounce * worldProp.energyKept);
    }

    private void MoveBalls()
    {
        for(int i = 0; i < balls.Count; i++)
        {
            balls[i].UpdateObjectposition();
        }
    }

    public void UpdateCreationBalls()
    {
        if (SetBallUnits() && SetBallWeight())
        {
            validText.SetActive(false);

            for (int i = 0; i < balls.Count; i++)
            {
                Destroy(balls[i].gameObject);
            }

            balls.Clear();

            CreateNewBalls();
        }

        else
        {
            validText.SetActive(true);
        }
    }

    private bool SetBallUnits()
    {
        if (!int.TryParse(ballNumberUIField.text, out numberOfBalls) || numberOfBalls < 0)
        {
            return false;
        }

        if (!float.TryParse(dropHeightUIField.text, out dropHeight) || dropHeight < 0)
        {
            return false;
        }

        if (!float.TryParse(ballDiameterUIField.text, out ballDiameter) || ballDiameter < 0)
        {
            return false;
        }

        if (!float.TryParse(sizeMultUIField.text, out sizeMultiplier) || sizeMultiplier < 0)
        {
            return false;
        }

        return true;
    }

    private bool SetBallWeight()
    {
        if (!float.TryParse(weightMultUIField.text, out weightMultiplier) || weightMultiplier < 0)
        {
            return false;
        }

        if (!float.TryParse(ballWeightUIField.text, out ballWeight) || ballWeight < 0)
        {
            return false;
        }




        return true;
    }

    private bool SetWorldPhysics()
    {
        if (!float.TryParse(worldProp.GravityUIField.text, out worldProp.worldGravity) || worldProp.worldGravity < 0)
        {
            return false;
        }

        if (!float.TryParse(worldProp.airDensityUIField.text, out worldProp.airDensity) || worldProp.airDensity < 0)
        {
            return false;
        }

        if (!float.TryParse(worldProp.elacityUISlider.value.ToString(), out worldProp.elasticityCoefficient))
        {
            return false;
        }

        if (!float.TryParse(worldProp.energyKeptUISlider.value.ToString(), out worldProp.energyKept))
        {
            return false;
        }

        if (!float.TryParse(worldProp.groundBounceUISlider.value.ToString(), out worldProp.groundBounce))
        {
            return false;
        }

        return true;
    }

    public void ChangeBallToFollow()
    {
        UpdateCameras((int)ballToFollowSlider.value - 1);
    }

    public void StartSimulation()
    {
        if(SetBallUnits() && SetBallWeight() && SetWorldPhysics())
        {
            simulating = true;
            simulationCanvas.SetActive(true);
            simulationCreateCanvas.SetActive(false);

            ballToFollowSlider.maxValue = numberOfBalls;
            ballToFollowSlider.value = numberOfBalls;

            validText.SetActive(false);
        }

        else
        {
            validText.SetActive(true);
        }
    }

    public void StopSimulation()
    {
        simulating = false;
        UpdateCreationBalls();
    }

    //e = elasticitets koefficienten
    //VA = old velocity
    //Va = new velocity
    //Va = ((1-e)VA*ma + (1+e)VB*mb) / 2
    //Vb = Va + e(VA*ma - VB*ma)
}
