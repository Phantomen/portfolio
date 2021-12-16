using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Decision/BossHealthEmpty")]
public class BossHealthEmptyDecision : StateDecision {

    public override bool Decide(StateController controller)
    {
        return HealthIsEmpty(controller);
    }

    //If the boss has no health left, return true
    private bool HealthIsEmpty(StateController controller)
    {
        bool isEmpty = false;

        TrudgesHealthBar hpBar = controller.GetComponentInChildren<TrudgesHealthBar>();

        if (hpBar != null && hpBar.CheckFullDamage() == true)
        {
            isEmpty = true;
        }

        return isEmpty;
    }
}
