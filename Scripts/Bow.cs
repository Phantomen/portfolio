using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class Bow : MonoBehaviour
    {
        public SteamVR_Action_Boolean pullAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Default", "InteractUI");


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
            //If bow is being held, set the hand holding it and set the other hand to the hand that holds the arrows
            if (interactable.attachedToHand)
            {
                bowHand = interactable.attachedToHand;
                arrowHand = bowHand.otherHand;

                //if arrowHand is trying to pull and the correct distance away from the notch
                if (pullAction[arrowHand.handType].stateDown
                    && Vector3.Distance(arrowHand.transform.position, notch.transform.position) <= distToPullNotchWithHand)
                {
                    //If arrow hand is empty or holding an arrow
                    if (arrowHand.currentAttachedObject == null
                        || arrowHand.currentAttachedObject.GetComponent<MyArrow>() != null)
                    {
                        AttachNotch();

                        if(arrowHand.currentAttachedObject.GetComponent<MyArrow>() != null)
                        {
                            GameObject arrow = arrowHand.currentAttachedObject;

                            arrow.transform.position = notch.transform.position;
                            arrow.transform.rotation = notch.transform.rotation;

                        }
                    }
                }

                //If the arrowhand was pulling and released the notch
                else if (pullAction[arrowHand.handType].stateUp && currentlyPulled)
                {
                    //If arrowhand was holding an arrow, release it and shoot it forward based on the distance the notch was pulled
                    if (arrowHand.currentAttachedObject != null && arrowHand.currentAttachedObject.GetComponent<MyArrow>() != null && currentDistPulled >= minDistPullToShoot)
                    {
                        GameObject arrow = arrowHand.currentAttachedObject;

                        arrowHand.DetachObject(arrow);

                        arrow.GetComponent<Rigidbody>().velocity = (currentDistPulled/notchPullDistanceMax) * shootSpeed * transform.forward;

                        arrow.transform.position = notch.transform.position;
                        arrow.transform.rotation = notch.transform.rotation;

                        arrow.GetComponent<MyArrow>().Shot();

                        //Make it so that the arrow can't collide with the bow for 0.5 seconds
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
        }

        void ReleaseNotch()
        {
            currentlyPulled = false;

            //release the hand from the notch location
            notch.GetComponent<Interactable>().attachedToHand = null;

            //Set the notch back to the position where it is not pulled
            notch.transform.position = notchOffset.transform.position;
            currentDistPulled = 0;
        }

        //Pull the notch based on how far back the bow was pulled
        void PullNotch()
        {
            float handNotchDist = -notchOffset.transform.InverseTransformDirection(arrowHand.transform.position - notchOffset.transform.position).z;
            currentDistPulled = Mathf.Clamp(handNotchDist, 0f, notchPullDistanceMax);
            notch.transform.localPosition = new Vector3(0, 0, -currentDistPulled);
        }
    }
}
