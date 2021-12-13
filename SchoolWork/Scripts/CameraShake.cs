using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private bool shaking = false;

    [Tooltip("Time it will shake")]
    [SerializeField]
    private float shakeTime = 1;

    private Timer shakeTimer;

    [Tooltip("")]
    [SerializeField]
    private float shakeAmount = 0.2f;

    private Vector3 camLocalPos = new Vector3();

    // Use this for initialization
    void Start ()
    {
        shakeTimer = new Timer(shakeTime, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //If it's shaking and the timer has not expired, Shake
        if (shaking == true && shakeTimer.Expired == false)
        {
            shakeTimer.Time += Time.deltaTime;
            Shake();
        }

        //If the timer has expired, stop shaking
        else if (shakeTimer.Expired == true)
        {
            StopShaking();
        }
	}

    private void Shake()
    {
        //If the amount it shakes is bigger than 0
        if (shakeAmount > 0)
        {
            //Gets camera offset
            float shakeOffsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float shakeOffsetY = Random.value * shakeAmount * 2 - shakeAmount;

            //Adds the offset to current offset
            camLocalPos.x += shakeOffsetX;
            camLocalPos.y += shakeOffsetY;

            //Local position of the camera is equal to the offset
            transform.localPosition = camLocalPos;
        }
    }

    //Call from other classes to start the camera shake
    public void StartCameraShake()
    {
        ResetShake();
    }

    //Reset time and sets shaking to true
    private void ResetShake()
    {
        shakeTimer.Time = 0;
        shaking = true;
    }

    //Stops the shaking, reset the cameras localposition and and resets values
    public void StopShaking()
    {
        shakeTimer.Time = 0;
        shaking = false;

        camLocalPos = Vector3.zero;
        transform.localPosition = Vector3.zero;
    }
}
