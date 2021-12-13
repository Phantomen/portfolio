using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FreeMovement : MonoBehaviour
{
    public SteamVR_Action_Vector2 input;
    public float speed = 1;
    private CharacterController characterController;

    private Player player;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponentInParent<CharacterController>();
        player = Player.instance;
    }

    // Update is called once per frame
    void Update()
    {

        Quaternion leftHandRotation = player.leftHand.transform.rotation;

        //This is used to make it so that even if the controller is pointed up, you still move at the speed that is dedicated by the analog stick
        if (leftHandRotation.eulerAngles.x >= 0 && leftHandRotation.eulerAngles.x < 90
            || leftHandRotation.eulerAngles.x > 270)
        {
            leftHandRotation = Quaternion.Euler(0, leftHandRotation.eulerAngles.y, leftHandRotation.eulerAngles.z);
        }
        else if (leftHandRotation.eulerAngles.x > 90 && leftHandRotation.eulerAngles.x < 270)
        {
            leftHandRotation = Quaternion.Euler(180, leftHandRotation.eulerAngles.y, leftHandRotation.eulerAngles.z);
        }

        if (leftHandRotation.eulerAngles.z >= 0 && leftHandRotation.eulerAngles.z < 90
            || leftHandRotation.eulerAngles.z > 270)
        {
            leftHandRotation = Quaternion.Euler(leftHandRotation.eulerAngles.x, leftHandRotation.eulerAngles.y, 0);
        }
        else if (leftHandRotation.eulerAngles.z >= 90 && leftHandRotation.eulerAngles.z < 270)
        {
            leftHandRotation = Quaternion.Euler(leftHandRotation.eulerAngles.x, leftHandRotation.eulerAngles.y, 180);
        }

        GameObject leftHand = new GameObject();
        leftHand.transform.rotation = leftHandRotation;


        Vector3 direction = leftHand.transform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));

        characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up));

        Destroy(leftHand);
    }
}
