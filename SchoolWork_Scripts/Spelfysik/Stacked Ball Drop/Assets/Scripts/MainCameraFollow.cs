using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraFollow : MonoBehaviour {

    Camera cam;

    public BallManager ballMan;

    private ForceObject[] balls = new ForceObject[2];

    public float sizeMult = 1.2f;

    // Use this for initialization
    void Awake ()
    {
        cam = GetComponent<Camera>();
    }

    public void UpdateCameraTarget(ForceObject[] ballsToAdd)
    {
        balls = ballsToAdd;

        transform.position = new Vector3(0, balls[0].transform.position.y + (balls[1].transform.position.y - balls[0].transform.position.y) / 2, transform.position.z);

        cam.orthographicSize = transform.position.y * sizeMult / 4;
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        transform.position = new Vector3(0, balls[0].transform.position.y + (balls[1].transform.position.y - balls[0].transform.position.y) / 2, transform.position.z);

        cam.orthographicSize = (balls[1].transform.position.y - balls[0].transform.position.y) * sizeMult + (balls[1].GetRadius + balls[0].GetRadius);
    }
}
