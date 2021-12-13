﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPickup : MonoBehaviour {

    [Tooltip("Time it takes to pick up item")]
    public float pickupTimeInSeconds = 0.5f;
    [Tooltip("Pointsystem that keeps track of life and pickup meter")]
    public PointSystem pointSystem;
    [Tooltip("Tag to identify collision with player")]
    public string playerTag = "Player";
    [Tooltip("Amount that item fills meter with")]
    public int fillMeterWith = 1;
    public GameObject audioSourceObject;
    private GameObject player;

    private Timer timer;
    private bool timerOn = false;
    private PlayerAnimationController playerAnim;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        //sets the amount of time it takes to pick up an item
        timer = new Timer(pickupTimeInSeconds, 0);

        player = GameObject.FindGameObjectWithTag(playerTag);
        audioSource = audioSourceObject.GetComponent<AudioSource>();
        playerAnim = player.GetComponent<PlayerAnimationController>();

    }

    void FixedUpdate()
    {


        if (timerOn)
        {

            timer.Time += Time.deltaTime;

            //If enough time has passed while interacting with player, set as true
            if (timer.Expired == true)
            {
                //Fill the meter in the pointsystem with the amount specified in fillMeterWith, then destroy object this script is on.
                audioSource.Play();
                if (gameObject.tag == "Spraybottle")
                {
                playerAnim.PickedUpSpray();
                }

                else if (gameObject.tag == "Trashbag")
                {
                    playerAnim.PickedUpBag();
                }
                pointSystem.FillMeter(fillMeterWith);
                Destroy (gameObject);
            }

        }
    }
	

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == playerTag)
        {
            timerOn = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //If ending interaction with player before timer expires, reset and stop timer so that objects don't get picked up while left alone, or when entering the hitbox of a new object
        if (other.gameObject.tag == playerTag)
        {
            timer.Time = 0;
            timerOn = false;
        }
    }
}
