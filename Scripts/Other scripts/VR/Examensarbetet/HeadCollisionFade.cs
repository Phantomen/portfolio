using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HeadCollisionFade : MonoBehaviour
{
    public float fadeDuration = 0.5f;

    public Color fadeColor = Color.black;

    Collider col;

    public SteamVR_Fade fade;

    List<Collider> collidingObjects = new List<Collider>();



    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FadeOut()
    {

        //set and start fade to
        SteamVR_Fade.Start(fadeColor, fadeDuration);
    }

    void ClearFade()
    {
        //set and start fade to
        SteamVR_Fade.Start(Color.clear, fadeDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player") == false)
        {
            if (collidingObjects.Count == 0)
            {
                FadeOut();
            }

            collidingObjects.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player") == false)
        {
            collidingObjects.Remove(other);

            if (collidingObjects.Count == 0)
            {
                ClearFade();
            }
        }
    }
}
