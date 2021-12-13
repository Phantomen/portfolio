using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShooterPattern : MonoBehaviour {

    //So every class that inherit ShooterPattern has a list of the indexes that it will use
    [SerializeField]
    protected List<AttackActionBulletSpawnList> bulletSpawnList = new List<AttackActionBulletSpawnList>();


    public abstract void Shoot(GameObject shooterGameObject);


    public abstract void Reset(GameObject shooterGameObject);


    public abstract void ClampValues();
}
