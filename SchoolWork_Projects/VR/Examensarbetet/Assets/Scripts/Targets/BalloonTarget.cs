using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonTarget : ShotTarget
{
    GameController gameController;

    // Start is called before the first frame update
    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (gameObject.activeInHierarchy)
        {
            gameController.AddTarget(gameObject);
        }
    }

    public override void TargetShot()
    {
        gameController.targetShot();
        gameObject.SetActive(false);
    }
}
