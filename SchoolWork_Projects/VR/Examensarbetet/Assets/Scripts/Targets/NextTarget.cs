using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTarget : ShotTarget
{
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public override void TargetShot()
    {
        gameController.NextLevel();
    }
}
