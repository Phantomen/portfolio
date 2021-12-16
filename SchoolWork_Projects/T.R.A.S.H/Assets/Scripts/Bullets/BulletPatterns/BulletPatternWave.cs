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

    private Vector3 oldWavePos = new Vector3(0, 0, 0);


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
        Vector3 wavePos = (transform.right * horizontal * Mathf.Sin(tempCurrentTime * (2f / timePerWave) * Mathf.PI) * distanceFromCenter);

        Vector3 newPos = wavePos - oldWavePos + transform.position;
        //Debug.Log(newPos);


        transform.position = newPos;

        oldWavePos = wavePos;
    }

    private void zigZagMotion()
    {
        zigZagTimer.Time += Time.deltaTime;

        if (zigZagTimer.Expired == true)
        {
            horizontal = -horizontal;
            zigZagTimer.Time -= timePerWave / 2;
        }

        Vector3 zigZagPos = transform.right * horizontal * Mathf.PingPong(tempCurrentTime * distanceFromCenter * (4f / timePerWave), distanceFromCenter);

        Vector3 newPos = zigZagPos - oldWavePos + transform.position;
        //Debug.Log(newPos);


        transform.position = newPos;

        oldWavePos = zigZagPos;
    }
}
