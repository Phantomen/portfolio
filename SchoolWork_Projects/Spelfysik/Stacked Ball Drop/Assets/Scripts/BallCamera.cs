using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCamera : MonoBehaviour {

    //public BallManager ballMan;

    private GameObject ballToFollow;

    Camera cam;

	// Use this for initialization
	void Awake ()
    {
        //ballToFollow = ballMan.GetCameraBalls()[1].gameObject;

        cam = GetComponent<Camera>();

        //cam.orthographicSize = ballMan.GetCameraBalls()[0].GetRadius + ballMan.GetCameraBalls()[1].GetRadius;
    }

    public void UpdateCameraTarget(GameObject target, float targetRadius, float firstBallRadius)
    {
        ballToFollow = target;
        cam.orthographicSize = targetRadius + firstBallRadius;
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        transform.position += new Vector3(0, ballToFollow.transform.position.y - transform.position.y, 0);
	}
}
