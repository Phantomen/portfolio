using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectsTransformList : MonoBehaviour {

    public List<TransformList> transformList = new List<TransformList>();
}

[System.Serializable]
public class TransformList
{
    public List<Transform> bulletSpawnList = new List<Transform>();
}
