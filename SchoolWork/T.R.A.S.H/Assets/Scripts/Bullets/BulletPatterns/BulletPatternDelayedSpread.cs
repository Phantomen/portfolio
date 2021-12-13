using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternDelayedSpread : MonoBehaviour {

    [SerializeField] GameObject playerObject;
    public enum ExplosionCheck
    {
        Distance = 0,
        Time,
        Either,
        Both
    }

    [Tooltip("What type of check the bullet will use for triggering the explosion")]
    public ExplosionCheck currentCheck;
    [Tooltip("Amount of seconds before triggering the explosion")]
    public float timeToExplode = 1;
    [Tooltip("How many units the bullets will travel before triggering the explosion")]
    public float distanceToExplode = 1;
    private float distanceTraveled = 0;
    private Timer timer;

    // Use this for initialization
    void Start ()
    {
        //distanceTraveled += Time.deltaTime * gameObject.GetComponent<Rigidbody2D>().velocity;

        distanceToExplode = -distanceToExplode;
        timer = new Timer(timeToExplode, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        timer.Time += Time.deltaTime;
        distanceTraveled += Time.deltaTime * gameObject.GetComponent<MoverForward>().speed;

        if (currentCheck == ExplosionCheck.Time)
        {
            if (timer.Expired == true)
            {
                Explode();
            }
        }

        else if (currentCheck == ExplosionCheck.Distance)
        {
            if (distanceTraveled <= distanceToExplode)
            {
                Explode();
            }
        }

        else if (currentCheck == ExplosionCheck.Either)
        {
            if (timer.Expired == true || distanceTraveled <= distanceToExplode)
            {
                Explode();
            }
        }

        else if (currentCheck == ExplosionCheck.Both)
        {
            if (timer.Expired == true && distanceTraveled <= distanceToExplode)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        this.gameObject.GetComponent<ShooterPatternSpread>().enabled = true;
        Destroy(this.gameObject, 0.02f);
    }
}
