using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallProperties))]
public class ForceObject : MonoBehaviour {

    private Vector3 currentVelocity = new Vector3();

    private BallProperties properties;


    // Use this for initialization
    void Awake()
    {
        properties = GetComponent<BallProperties>();
    }


    public float GetRadius { get { return properties.ballRadius; } }
    public float GetMass { get { return properties.ballMass; } }


    public void ApplyForce(Vector3 forceToApply)
    {
        currentVelocity += (forceToApply / properties.ballMass) * Time.fixedDeltaTime;
    }

    public void SetVelocity(Vector3 velToSet)
    {
        currentVelocity = velToSet;
    }

    public Vector3 GetVelocity()
    {
        return currentVelocity;
    }

    public Vector3 GetForce()
    {
        return currentVelocity * properties.ballMass;
    }

    public void UpdateObjectposition()
    {
        transform.position += currentVelocity * Time.fixedDeltaTime;
    }

}
