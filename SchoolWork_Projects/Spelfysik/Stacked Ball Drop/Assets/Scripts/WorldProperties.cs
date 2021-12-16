using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldProperties : MonoBehaviour {

    [HideInInspector]
    public float worldGravity = 9.82f;
    public InputField GravityUIField;

    [HideInInspector]
    public float airDensity = 1.2f;
    public InputField airDensityUIField;

    [HideInInspector, Range(0, 1)]
    public float elasticityCoefficient = 1;
    public Slider elacityUISlider;

    [HideInInspector, Range(0, 1)]
    public float groundBounce = 1;
    public Slider groundBounceUISlider;

    //[HideInInspector, Range(0, 1)]
    //public float energyLoss = 0f;

    [HideInInspector, Range(0, 1)]
    public float energyKept = 1;
    public Slider energyKeptUISlider;
}
