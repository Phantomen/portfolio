using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPickup : MonoBehaviour {

    [Tooltip("Time it takes to pick up item")]
    public float pickupTimeInSeconds = 0.5f;
    [Tooltip("Pointsystem that keeps track of life and pickup meter")]
    PointSystem pointSystem;
    [Tooltip("Tag to identify collision with player")]
    public string playerTag = "Player";
    [Tooltip("Amount that item fills meter with")]
    public int fillMeterWith = 1;

    public AudioClip audioClip;
    public string audioSourceChildNamePickup = "Trash pickup";
    public bool activateHealthBar = false;

    private TrudgesHealthBar trudgesHealthBar;
    private GameObject player;
    private AudioSource audioSource;
    private PlayerAnimationController playerAnim;
    private Animator ownAnim;
    private Timer timer;
    private bool timerOn = false;

    // Use this for initialization
    void Start()
    {
        //sets the amount of time it takes to pick up an item
        timer = new Timer(pickupTimeInSeconds, 0);

        //Finds the player
        player = GameObject.FindGameObjectWithTag(playerTag);
        playerAnim = player.GetComponent<PlayerAnimationController>();

        ownAnim = GetComponent<Animator>();

        //Finds pointsystem
        pointSystem = GameObject.FindGameObjectWithTag("PointSystem").GetComponent<PointSystem>();

        //finds audioSource
        audioSource = GameObject.FindGameObjectWithTag("AudioSource").transform.Find(audioSourceChildNamePickup).GetComponent<AudioSource>();

        //Gets the bosses healthbar
        trudgesHealthBar = GameObject.FindGameObjectWithTag("Boss").GetComponentInChildren<TrudgesHealthBar>();
    }

    void FixedUpdate()
    {
        //If player is picking the trash up
        if (timerOn)
        {
            timer.Time += Time.deltaTime;

            //If enough time has passed while interacting with player, pick it up
            if (timer.Expired == true)
            {
                //plays the sound
                audioSource.clip = audioClip;
                audioSource.Play();

                //plays the animation
                if (gameObject.tag == "Spraybottle")
                {
                    playerAnim.PickedUpSpray();
                }

                else if (gameObject.tag == "Trashbag")
                {
                    playerAnim.PickedUpBag();
                }

                //If it should damage a boss, and there actually is a boss, damage it
                if (activateHealthBar && trudgesHealthBar != null)
                {
                    trudgesHealthBar.UpdateBar();
                }

                //Fills the powerup meter
                pointSystem.FillMeter(fillMeterWith);

                //Destroy pickup
                Destroy(gameObject);
            }

        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //Player is picking it up
        if (other.gameObject.tag == playerTag)
        {
            timerOn = true;
            ownAnim.SetBool("Jumping", true);
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //If ending interaction with player before timer expires, reset and stop timer so that objects don't get picked up while left alone, or when entering the hitbox of a new object
        if (other.gameObject.tag == playerTag)
        {
            timer.Time = 0;
            timerOn = false;            
            ownAnim.SetBool("Jumping", false);
            
        }
    }
}
