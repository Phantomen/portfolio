using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    public float pickupAnimationTime;

    private Timer deathTimer, pickupTimer;
    private bool gotHit = false, pickedUpTrash = false, pickedUpSpray = false;
    private Animator animator;

    // Use this for initialization
    void Start ()
    {
        deathTimer = new Timer(0.25f, 0);
        pickupTimer = new Timer(pickupAnimationTime, 0);
        animator = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (gotHit == true)
        {
            animator.SetBool("GetHit", true);

            deathTimer.Time += Time.deltaTime;

          if (deathTimer.Expired == true)
          {
                animator.SetBool("GetHit", false);
                deathTimer.Time = 0;
                gotHit = false;
          }
        }

        if (pickedUpTrash == true)
        {
            animator.SetBool("PickingUpBag", true);

            pickupTimer.Time += Time.deltaTime;

            if (pickupTimer.Expired == true)
            {
                animator.SetBool("PickingUpBag", false);
                pickupTimer.Time = 0;
                pickedUpTrash = false;
            }
        }

        if (pickedUpSpray == true)
        {
            animator.SetBool("PickingUpSpray", true);

            pickupTimer.Time += Time.deltaTime;

            if (pickupTimer.Expired == true)
            {
                animator.SetBool("PickingUpSpray", false);
                pickupTimer.Time = 0;
                pickedUpSpray = false;
            }
        }
    }

   public void GotHit()
    {
        gotHit = true;
    }



    public void PickedUpBag()
    {
        pickedUpTrash = true;
    }

    public void PickedUpSpray()
    {
        pickedUpSpray = true;
    }
}
