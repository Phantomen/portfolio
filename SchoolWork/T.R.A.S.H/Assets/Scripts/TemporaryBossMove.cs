using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryBossMove : MonoBehaviour {

    public float min = 2f;
    public float max = 3f;
    public float movementSpeed;
    // Use this for initialization
    void Start()
    {

        //min = transform.position.x;
        //max = transform.position.x + 3;

    }

    // Update is called once per frame
    void FixedUpdate()
    {


        transform.position = new Vector3(Mathf.PingPong(Time.time * movementSpeed, max - min) + min,transform.position.y , transform.position.z);

    }

}
