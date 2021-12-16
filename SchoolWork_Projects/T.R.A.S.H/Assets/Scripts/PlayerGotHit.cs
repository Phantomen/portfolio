using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGotHit : MonoBehaviour {

    public PointSystem pointSystem;
    public int damageTakenByHits = 1;
    public float invincibleTime = 1f;
    public bool invincible = false;


    private int boopCount;
    private Timer timer;
    private Timer deathTimer;
    private bool timerOn = false;
    private bool gotHit = false;
   // private Animator animator;
    private AudioSource sound;
    private PlayerAnimationController playerAnim;




    // Use this for initialization
    void Start()
    {
        sound = GetComponent<AudioSource>();
        timer = new Timer(invincibleTime, 0);
        deathTimer = new Timer(0.25f, 0);
        //animator = gameObject.GetComponent<Animator>();
        playerAnim = gameObject.GetComponent<PlayerAnimationController>();

    }

    void FixedUpdate()
    {
        if (timerOn)
        {
            timer.Time += Time.deltaTime;
            if (timer.Expired == true)
            {
                timer.Time = 0;
                timerOn = false;
                invincible = false;
            }

        }

        //if(gotHit)
        //{


            //deathTimer.Time += Time.deltaTime;
            //if (deathTimer.Expired == true)
            //{
            //    animator.SetBool("GetHit", false);
            //    deathTimer.Time = 0;
            //    gotHit = false;
            //}
        //}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "bullet" && invincible == false)
        {

            sound.Play();
            boopCount++;
            pointSystem.ChangeLife(damageTakenByHits);
            timerOn = true;
            invincible = true;
            //gotHit = true;
            //animator.SetBool("GetHit", true);
            playerAnim.GotHit();
            Destroy(other.gameObject, 0.1f);

        }
    }
}
