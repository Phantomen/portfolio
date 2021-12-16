using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHeadCharacterCon : MonoBehaviour
{
    CharacterController cc;
    [SerializeField]
    Transform head;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localHeadSpace = head.position - transform.position;
        cc.center = new Vector3(localHeadSpace.x, 0 , localHeadSpace.z);
    }
}
