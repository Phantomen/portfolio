using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class Bow : MonoBehaviour
    {
        public SteamVR_Action_Boolean pullAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Default", "InteractUI");
        //public SteamVR_Action_Boolean pullAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Bow", "Pull");

        Interactable interactable;
        public GameObject notch;
        public GameObject notchOffset;
        private Vector3 notchOrgPos;

        public float distToPullNotchWithHand = 0.3f;
        public float notchPullDistanceMax = 1.5f;
        private float currentDistPulled = 0f;
        public float minDistPullToShoot = 0.2f;
        public float shootSpeed = 20f;
        public float pulledDistShootSpeedMult = 2f;

        private Hand bowHand;
        private Hand arrowHand;

        private bool currentlyPulled = false;

        // Start is called before the first frame update
        void Awake()
        {
            interactable = GetComponent<Interactable>();
            notchOrgPos = notch.transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if (interactable.attachedToHand)
            {
                bowHand = interactable.attachedToHand;
                arrowHand = bowHand.otherHand;

                if (pullAction[arrowHand.handType].stateDown
                    && Vector3.Distance(arrowHand.transform.position, notch.transform.position) <= distToPullNotchWithHand)     //if arrowHandisTrying to pull and the correct distance away
                {
                    if (arrowHand.currentAttachedObject == null
                        || arrowHand.currentAttachedObject.GetComponent<MyArrow>() != null)                                               //If hand is empty or holding an arrow
                    {
                        AttachNotch();

                        if(arrowHand.currentAttachedObject.GetComponent<MyArrow>() != null)
                        {
                            GameObject arrow = arrowHand.currentAttachedObject;

                            arrow.transform.position = notch.transform.position;
                            arrow.transform.rotation = notch.transform.rotation;
                            //arrowHand.DetachObject(arrow);
                            //arrow.transform.parent = notch.transform;
                        }
                    }
                }

                //pullAction[arrowHand.handType].onStateUp +=
                else if (pullAction[arrowHand.handType].stateUp && currentlyPulled)
                {
                    if (arrowHand.currentAttachedObject != null && arrowHand.currentAttachedObject.GetComponent<MyArrow>() != null && currentDistPulled >= minDistPullToShoot)
                    {
                        GameObject arrow = arrowHand.currentAttachedObject;

                        arrowHand.DetachObject(arrow);

                        arrow.GetComponent<Rigidbody>().velocity = (currentDistPulled/notchPullDistanceMax) * shootSpeed * transform.forward;

                        arrow.transform.position = notch.transform.position;
                        arrow.transform.rotation = notch.transform.rotation;

                        arrow.GetComponent<MyArrow>().Shot();

                        Physics.IgnoreCollision(arrow.GetComponent<Collider>(), gameObject.GetComponent<Collider>(), true);
                        EnableCollision(arrow.GetComponent<Collider>(), gameObject.GetComponent<Collider>(), 0.5f);
                    }

                    ReleaseNotch();
                }

                else if (currentlyPulled)
                {
                    PullNotch();

                    if (arrowHand.currentAttachedObject.GetComponent<MyArrow>() != null)
                    {
                        GameObject arrow = arrowHand.currentAttachedObject;

                        arrow.transform.position = notch.transform.position;
                        arrow.transform.rotation = notch.transform.rotation;
                        //arrowHand.DetachObject(arrow);
                        //arrow.transform.parent = notch.transform;
                    }
                }
            }

            else if (currentlyPulled)
            {
                ReleaseNotch();
            }
        }

        private IEnumerator EnableCollision(Collider col1, Collider col2, float delay)
        {
            yield return new WaitForSeconds(delay);
            Physics.IgnoreCollision(col1, col2, false);
        }


        void AttachNotch()
        {
            currentlyPulled = true;

            //Attach the hand to the notch location
            notch.GetComponent<Interactable>().attachedToHand = arrowHand;
            //Grabbable grabbable = notch.GetComponent<Grabbable>();
            //arrowHand.AttachObject(notch.gameObject, GrabTypes.Grip, grabbable.attachmentFlags, grabbable.attachmentOffset);
            //(gameObject, startingGrabType, attachmentFlags, attachmentOffset);

            Debug.Log("attach");
        }

        void ReleaseNotch()
        {
            currentlyPulled = false;

            //release the hand from the notch location
            notch.GetComponent<Interactable>().attachedToHand = null;

            notch.transform.position = notchOffset.transform.position;
            currentDistPulled = 0;

            Debug.Log("dettach");
        }

        void PullNotch()
        {
            float handNotchDist = -notchOffset.transform.InverseTransformDirection(arrowHand.transform.position - notchOffset.transform.position).z;
            //handNotchDist = Mathf.Abs(handNotchDist);

            //float handNotchDist = Mathf.Abs(Vector3.Distance(arrowHand.transform.position, notchOffset.transform.position));

            currentDistPulled = Mathf.Clamp(handNotchDist, 0f, notchPullDistanceMax);

            notch.transform.localPosition = new Vector3(0, 0, -currentDistPulled);

            //Debug.Log(handNotchDist);
        }
    }
}