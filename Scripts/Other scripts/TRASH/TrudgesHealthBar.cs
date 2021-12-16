using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrudgesHealthBar : MonoBehaviour {


    public GameObject healthBarObject;
    public List<Sprite> healthBar = new List<Sprite>();

    private int damageTaken = 0;
    private SpriteRenderer healthBarSpriterenderer;
    private PointSystem pointSystem;

    [Tooltip("Sound to play when boss takes damage")]
    [SerializeField]
    private AudioClip damageSound;

    [Tooltip("Sound to play when boss changes phase (reset healthbar)")]
    [SerializeField]
    private AudioClip phaseChangeSound;

    AudioSource audioSource;

    private Animator bossAnimator;

    // Use this for initialization
    void Start ()
    {
        healthBarSpriterenderer = healthBarObject.GetComponent<SpriteRenderer>();
        healthBarSpriterenderer.sprite = healthBar[0];
        pointSystem = GameObject.FindGameObjectWithTag("PointSystem").GetComponent<PointSystem>();

        bossAnimator = gameObject.transform.parent.gameObject.GetComponentInChildren<Animator>();

        audioSource = GameObject.FindGameObjectWithTag("AudioSource").transform.Find("BossSpecial").GetComponent<AudioSource>();
    }


    //When the boss takes damage
    public void UpdateBar()
    {
        damageTaken ++;
            
        //Take damage
        healthBarSpriterenderer.sprite = healthBar[damageTaken];

        //Play damage sound if curren't health is not 0
        if (damageTaken < healthBar.Count - 1)
        {
            audioSource.clip = damageSound;
            audioSource.Play();
        }
    }

    //Resets the healthbar
    public void ResetBar()
    {
        damageTaken = 0;

        healthBarSpriterenderer.sprite = healthBar[damageTaken];

        //Play PhaseChangeSound
        audioSource.clip = phaseChangeSound;
        audioSource.Play();

        //Plays the "slam" animation once 
        bossAnimator.SetTrigger("Slam");
    }


    //Checks if the boss has 0 health
    public bool CheckFullDamage()
    {
        bool fullDamage = false;

        if (damageTaken >= healthBar.Count - 1)
        {
            fullDamage = true;
        }

        return fullDamage;
    }
}
