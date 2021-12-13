using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Actions/ActivateWinCanvas")]
public class ActivateWinCanvas : StateAction {

    public float deathTime = 1f;

    private float currentTime = 0;

    private bool triggered = false;

    [Tooltip("Sound that plays when trudges is defeated")]
    [SerializeField]
    private AudioClip deathClip;

    public override void Act(StateController controller)
    {
        ActivateCanvas(controller);
    }

    private void ActivateCanvas(StateController controller)
    {
        currentTime += Time.deltaTime;

        if (currentTime >= deathTime)
        {
            GameObject winCanvas = GameObject.Find("Canvas").transform.Find("YouWin").gameObject;

            if (winCanvas.activeSelf == false && winCanvas != null)
            {
                AudioSource[] audioSourceObject = GameObject.FindGameObjectWithTag("AudioSource").GetComponentsInChildren<AudioSource>();

                //Stops all the audio from the AudioSource object
                foreach (AudioSource au in audioSourceObject)
                {
                    au.Stop();
                }

                //Inactivates the player to stop it from making sound
                controller.playerGameObject.SetActive(false);

                winCanvas.SetActive(true);
            }
        }

        if (triggered == false)
        {
            triggered = true;

            AudioSource[] audioSourceObject = GameObject.FindGameObjectWithTag("AudioSource").GetComponentsInChildren<AudioSource>();

            //Stops all the audio from the AudioSource object
            foreach (AudioSource au in audioSourceObject)
            {
                au.Stop();
            }

            Animator bossAnimator = controller.GetComponentInChildren<Animator>();

            bossAnimator.SetTrigger("Death");

            AudioSource audioSourceBoss = GameObject.FindGameObjectWithTag("AudioSource").transform.Find("BossSpecial").GetComponent<AudioSource>();

            audioSourceBoss.loop = false;
            audioSourceBoss.clip = deathClip;
            audioSourceBoss.Play();
        }
    }


    public override void Reset(StateController controller)
    {
        triggered = false;
        currentTime = 0;
    }
}
