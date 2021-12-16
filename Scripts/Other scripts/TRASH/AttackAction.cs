using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/AttackAction")]
public class AttackAction : StateAction {

    //The shooterpattern
    public ShooterPattern shotPattern;

    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        //Shot the pattern
        shotPattern.Shoot(controller.gameObject);
    }

    public override void Reset(StateController controller)
    {
        ResetShooting(controller);
    }

    private void ResetShooting(StateController controller)
    {
        //Resets the pattern
        shotPattern.Reset(controller.gameObject);
    }
}

[System.Serializable]
public class AttackActionBulletSpawnList
{
    public int phaseIndex = 0;
    public int[] spawnPointIndex;
}