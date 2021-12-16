using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TwoHanded : MonoBehaviour
{
    public Interactable mainHand;
    private Hand handMain;
    public GameObject mainHandOffset;
    private Quaternion mainHandOrgRot;
    private bool mainGrabbed = false;

    public Interactable offHand;
    public GameObject offHandOffset;
    //private Transform offHandOrgTrans;

    public bool mainToOffhand = true;

    public enum TwoHandRotationType { None, First, Second};
    public TwoHandRotationType twohandRotationType = TwoHandRotationType.First;

    //private bool grabbedMain = false, grabbedOff = false;

    // Start is called before the first frame update
    void Start()
    {
        //mainHandOrgRot = mainHandOffset.transform.rotation;
        //mainHandOrgRot = mainHand.attachedToHand.transform.rotation;
        //offHandOrgTrans = offHand.attachedToHand.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(mainHandOrgRot.eulerAngles);

        if (mainHand.attachedToHand)
        {
            handMain = mainHand.attachedToHand;
        }

        if (mainHand.attachedToHand && offHand.attachedToHand)
        {
            if(!mainGrabbed)
            {
                mainGrabbed = true;
                mainHandOrgRot = handMain.objectAttachmentPoint.transform.localRotation;
                //Debug.Log(mainHand.attachedToHand.transform.rotation);
            }

            //mainHandOffset.transform.rotation = Quaternion.LookRotation(offHand.attachedToHand.transform.position - mainHandOffset.transform.position);

            handMain.objectAttachmentPoint.transform.rotation = GetTwoHandRot();

            //mainHandOffset.transform.rotation = Quaternion.LookRotation(offHand.attachedToHand.transform.position - mainHandOffset.transform.position);
            
            /*
            if(mainToOffhand)
                handMain.objectAttachmentPoint.transform.rotation = Quaternion.LookRotation(offHand.attachedToHand.transform.position - handMain.transform.position);   //mainHandOffset.transform.position);
            else
                handMain.objectAttachmentPoint.transform.rotation = Quaternion.LookRotation(handMain.transform.position - offHand.attachedToHand.transform.position);   //mainHandOffset.transform.position);

            handMain.objectAttachmentPoint.transform.rotation *= Quaternion.Euler(25, 0, 90);
            */
            
            //Debug.Log(Quaternion.LookRotation(offHand.attachedToHand.transform.position - mainHandOffset.transform.position));
        }

        else if (mainGrabbed)
        {
            //mainHandOffset.transform.rotation = mainHandOrgRot.transform.rotation;
            handMain.objectAttachmentPoint.transform.localRotation = mainHandOrgRot;
            mainHandOrgRot = new Quaternion();
            mainGrabbed = false;
            if (!mainHand.attachedToHand)
                handMain = null;
            Debug.Log("reset");
        }
    }

    private Quaternion GetTwoHandRot()
    {
        Transform firstOffset, secondOffset;
        if (mainToOffhand)
        {
            firstOffset = handMain.otherHand.transform; //offHand.attachedToHand.transform;
            secondOffset = handMain.transform;
        }

        else
        {
            firstOffset = handMain.transform;
            secondOffset = handMain.otherHand.transform; //offHand.attachedToHand.transform;
        }

        Quaternion targetRot;

        if (twohandRotationType == TwoHandRotationType.None)
        {
            //Transform average = handMain.transform;
            //average.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, handMain.transform.rotation.eulerAngles.z), Quaternion.Euler(0, 0, handMain.otherHand.transform.rotation.eulerAngles.z), 0);
            targetRot = Quaternion.LookRotation(firstOffset.position - secondOffset.position);
            //Quaternion handRot = Quaternion.Euler(0, 0, firstOffset.rotation.eulerAngles.z + ((firstOffset.rotation.eulerAngles.z - secondOffset.rotation.eulerAngles.z) / 2));
            //targetRot *= handRot * Quaternion.Euler(0, 0, 90);
            //Debug.Log(handRot.eulerAngles.z);
        }
        else if (twohandRotationType == TwoHandRotationType.First)
        {
            //targetRot = Quaternion.LookRotation(firstOffset.position - secondOffset.position, handMain.transform.up);
            targetRot = Quaternion.LookRotation(firstOffset.position - secondOffset.position, handMain.transform.TransformDirection(Vector3.forward));
            targetRot *= Quaternion.Euler(0, 0, 90);
        }
        else
        {
            //targetRot = Quaternion.LookRotation(firstOffset.position - secondOffset.position, handMain.otherHand.transform.up);
            targetRot = Quaternion.LookRotation(firstOffset.position - secondOffset.position, handMain.otherHand.transform.TransformDirection(Vector3.forward));
            targetRot *= Quaternion.Euler(0, 0, 90);
        }

        /*
        if (twohandRotationType == TwoHandRotationType.None)
        {
            targetRot = Quaternion.LookRotation(mainHandOffset.transform.position - offHandOffset.transform.position);
        }
        else if (twohandRotationType == TwoHandRotationType.First)
        {
            targetRot = Quaternion.LookRotation(mainHandOffset.transform.position - offHandOffset.transform.position, mainHandOffset.transform.up);
        }
        else
        {
            targetRot = Quaternion.LookRotation(mainHandOffset.transform.position - offHandOffset.transform.position, offHandOffset.transform.up);
        }
        */

        return targetRot;
    }

    public void GrabbedMain()
    {

    }
}
