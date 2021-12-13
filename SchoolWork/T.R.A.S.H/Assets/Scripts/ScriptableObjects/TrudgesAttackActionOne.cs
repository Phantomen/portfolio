using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/TrudgesAttackOne")]
public class TrudgesAttackActionOne : StateAction {

    public ShooterPattern shotPattern;

    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        shotPattern.Shoot(controller.gameObject);
    }

    public override void Reset(StateController controller)
    {
        ResetShooting(controller);
    }

    private void ResetShooting(StateController controller)
    {
        //shotPattern.Reset();
        shotPattern.Reset(controller.gameObject);
    }
}
