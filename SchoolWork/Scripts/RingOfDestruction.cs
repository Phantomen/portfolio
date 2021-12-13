using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfDestruction : MonoBehaviour {

    private ParticleSystem parSys;

    CircleCollider2D col2D;


    // Use this for initialization
    void Start () {
        //Gets the particle system and the collider
        parSys = GetComponent<ParticleSystem>();
        col2D = GetComponent<CircleCollider2D>();

        //Disables the collider
        col2D.enabled = false;
    }

    private void Update()
    {
        //If the particle system is playing
        //Enable the collider
        if (parSys.isPlaying == true)
        {
            col2D.enabled = true;
        }

        else
        {
            col2D.enabled = false;
        }

        //Sets the colliders radius
        col2D.radius = (parSys.time / parSys.startLifetime) * (parSys.startSize / 2);
    }

    //If the object collides with objects that the layer allow
    //Destroy them
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
