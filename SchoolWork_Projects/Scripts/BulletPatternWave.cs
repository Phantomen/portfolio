using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternWave : MonoBehaviour {

    public float distanceFromCenter = 1;

    public bool startRight = true;
    private int horizontal;

    //Slows down at the ends and speeds up towards center
    //use sin
    public bool smoothWaveTurning = true;

    //Seconds
    public float timePerWave = 2;


    private float tempCurrentTime = 0f;

    private Vector3 oldWaveOffset = new Vector3(0, 0, 0);


    Timer zigZagTimer = new Timer();



    //private float 

    // Use this for initialization
    void Start()
    {
        if (startRight == true)
        {
            horizontal = 1;
        }
        else
        {
            horizontal = -1;
        }

        zigZagTimer.Set(timePerWave / 2, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tempCurrentTime += Time.deltaTime;

        //Vector3 pos = transform.position;


        if (smoothWaveTurning == true)
        {
            WaveMotion();
        }

        else
        {
            zigZagMotion();
        }
    }

    private void WaveMotion()
    {
        //The new offset of the wave
        Vector3 waveOffset = (transform.right * horizontal * Mathf.Sin(tempCurrentTime * (2f / timePerWave) * Mathf.PI) * distanceFromCenter);

        //To keep the object to from moving more to the side than it should
        //Remove the old waveOffset from the current position
        Vector3 newPos = waveOffset + (transform.position - oldWaveOffset);

        //Sets new position
        transform.position = newPos;

        //saves the waveOffset for the next update
        oldWaveOffset = waveOffset;
    }

    private void zigZagMotion()
    {
        zigZagTimer.Time += Time.deltaTime;

        //If the timer has expired
        if (zigZagTimer.Expired == true)
        {
            //the object moves to the other side
            horizontal = -horizontal;
            //Halves the time
            zigZagTimer.Time -= timePerWave / 2;
        }

        //The new offset
        Vector3 zigZagOffset = transform.right * horizontal * Mathf.PingPong(tempCurrentTime * distanceFromCenter * (4f / timePerWave), distanceFromCenter);

        //To keep the object to from moving more to the side than it should
        //Remove the old waveOffset from the current position
        Vector3 newPos = zigZagOffset + (transform.position - oldWaveOffset);

        //Sets new position
        transform.position = newPos;

        //saves the Offset for the next update
        oldWaveOffset = zigZagOffset;
    }
}
