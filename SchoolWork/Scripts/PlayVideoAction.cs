using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Actions/PlayVideo")]
public class PlayVideoAction : StateAction {

    [Tooltip("The prefab of the video")]
    [SerializeField]
    private GameObject videoCanvasPrefab;

    private bool triggered = false;

    public override void Act(StateController controller)
    {
        PlayVideo(controller);
    }

    private void PlayVideo(StateController controller)
    {
        if (triggered == false)
        {
            triggered = true;

            //Instanciates thecanvas with the video
            Instantiate(videoCanvasPrefab);

            AudioSource[] audioSourceObject = GameObject.FindGameObjectWithTag("AudioSource").GetComponentsInChildren<AudioSource>();

            //Stops all the audio from the AudioSource object
            foreach (AudioSource au in audioSourceObject)
            {
                au.Stop();
            }

            //Inactivates the player to stop it from making sound
            controller.playerGameObject.SetActive(false);
        }
    }

    public override void Reset(StateController controller)
    {
        triggered = false;
    }
}
