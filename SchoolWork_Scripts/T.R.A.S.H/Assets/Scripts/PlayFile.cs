using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayFile : MonoBehaviour {

    public MovieTexture movie;
    private AudioSource audio;
    private float timer;
    // Use this for initialization
    void Start()
    {
        GetComponent<RawImage>().texture = movie as MovieTexture;
        audio = GetComponent<AudioSource>();
        //audio.clip = movie.audioClip;
        movie.Play();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 6)
        {
            timer = 0;
            movie.Stop();
        }
    }
}
