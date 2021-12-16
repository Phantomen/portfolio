using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointSystem : MonoBehaviour {

    //public GameObject player;
    public GameObject gameOver, youWin, soap, meterUI, audioSource;

    private GameObject player;

    public Text life, meter;
    public int lifeAmount = 3, meterFilled = 1;
    public BackgroundMusic backgroundMusic;
    //True fills the meter, false depletes meter
    //public bool depleteOrFill = false;
    public List<Sprite> lifeSoap = new List<Sprite>();
    public List<Sprite> meterBar = new List<Sprite>();

    Image soapImage, meterImage;

    //For when depleteOrFill is true, starts off the value at 0 instead of the meterFilled value
    private int meterStartValue = 0, meterGoalvalue, maxHealth;
   // private AudioSource audio;

    // Use this for initialization
    void Start () {

        //audio = audioSource.GetComponent<AudioSource>();
        soapImage = soap.GetComponent<Image>();
        meterImage = meterUI.GetComponent<Image>();
        life.text = "Life: " + lifeAmount.ToString();
        maxHealth = lifeAmount;

        meterGoalvalue = meterFilled;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ChangeLife(int other)
    {

            lifeAmount -= other;
            life.text = "Life: " + lifeAmount.ToString();

        
            soapImage.sprite = lifeSoap[maxHealth - lifeAmount];
       


        if (lifeAmount <= 0)
        {
            gameOver.SetActive(true);
            player.SetActive(false);
            backgroundMusic.StopMusic();

        }
    }

    public void FillMeter(int other)
    {
        meterFilled -= other;



		//Kommenterad bort pga fler sprites i testning
        //meterImage.sprite = meterBar[meterGoalvalue - meterFilled];




        if (meterFilled == 1)
        {
            youWin.SetActive(true);
            player.SetActive(false);
            backgroundMusic.StopMusic();
        }
    }
}
