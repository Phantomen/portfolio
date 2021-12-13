using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallProperties : MonoBehaviour {

    public float ballMass = 1;
    public float ballRadius = 0.5f;

    public void SetRadius(float newRadius)
    {
        ballRadius = newRadius;
        gameObject.transform.localScale = Vector3.one * ballRadius * 2;
    }

    public void SetMass(float newMass)
    {
        ballMass = newMass;
    }

    public void InstanciateBall(float radius, float mass)
    {
        SetRadius(radius);
        SetMass(mass);
    }
}
