using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShooterPattern : MonoBehaviour {

    //public abstract void Start();

    public abstract void Shoot(GameObject shooterGameObject);

    //public abstract void Shoot(List<GameObject> shooterList);

    //private abstract void ShootPattern();

    //public abstract void Reset();

    public abstract void Reset(GameObject shooterGameObject);

    public abstract void Reset(GameObject shooterGameObject, List<Transform> bulletSpawnList);
}
