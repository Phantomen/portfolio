using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {


    public List<GameObject> waves = new List<GameObject>();
    public List<float> waveTimes = new List<float>();


    private float time;
    private Timer timer;

    // Use this for initialization
    void Start ()
    {
        //timer = new Timer(5, 0);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // timer.Time += Time.deltaTime;
        time += Time.deltaTime;

        if (time >= waveTimes[0])
        {
            Debug.Log("Ding");
            waves[0].SetActive(true);
        }

        if (time >= waveTimes[1])
        {
            waves[0].SetActive(false);
            waves[1].SetActive(true);
        }

        if (time >= waveTimes[2])
        {
            waves[1].SetActive(false);
            waves[2].SetActive(true);
        }

        if (time >= 20f)
        {
            waves[2].SetActive(false);
            time = 0f;
        }
    }
}
