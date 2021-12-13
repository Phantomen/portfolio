using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class GunRealistic : MonoBehaviour
    {
        public bool isAutomatic = true;



        public bool usingChargingHandle = false;
        //public GameObject chargingHandle;

        public GameObject chargingHandle;
        public GameObject chargingHandleOffset;
        //private Vector3 chargingHandleOrgPos;

        //public float distToPullCharghingHandleWithHand = 0.3f;
        public float notchPullDistanceMax = 0.2f;
        private float currentDistPulled = 0f;
        //public float minDistPullToShoot = 0.2f;
        public float shootSpeed = 20f;
        //public float pulledDistShootSpeedMult = 2f;

        private bool currentlyPulled = false;


        private Hand gunHand;

        public bool bulletChambered = true;

        public GameObject magazineLoadOffset;
        public Magazine magazine;
        public bool spawnWithMagazine = true;
        bool magazineLoaded;
        public float magazineLoadDistance = 0.2f;
        public float magazineEjectionSpeed = 1f;


        public SteamVR_Action_Boolean shootAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Gun", "Shoot");
        public SteamVR_Action_Boolean ejectAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Gun", "Eject");
        //public SteamVR_Action_Boolean pullAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Default", "InteractUI");    //Use grab instead of trigger

        public Transform barrelend;

        public GameObject bulletPrefab;
        public int bpm = 30;
        public int bulletSpeed = 10;
        private float timePassed = 0f;
        private float timeToShoot;

        private Interactable interactable;
        private SteamVR_Input_Sources gunHandType;

        private bool shooting;


        public GameObject shellPrefab;
        public GameObject casingPrefab;
        public GameObject casingEjectionPort;
        public float casingEjectionSpeed = 5f;
        public float casingLifeTime = 1f;

        // Start is called before the first frame update
        void Awake()
        {
            interactable = GetComponent<Interactable>();

            magazineLoaded = spawnWithMagazine;
            if (magazineLoaded == false)
            {
                Destroy(magazine);
            }
            else
            {
                magazine.GetComponent<Collider>().enabled = false;
                magazine.GetComponent<Rigidbody>().isKinematic = true;
                //magazine.GetComponent<Interactable>().enabled = false;
            }

            timeToShoot = 60f / bpm;
        }

        // Update is called once per frame
        void Update()
        {
            timePassed += Time.deltaTime;

            if (interactable.attachedToHand)
            {
                gunHand = interactable.attachedToHand;
                gunHandType = gunHand.handType;
                //shooting = shootAction[hand].state;

                if (((isAutomatic == true && shootAction[gunHandType].state) || (isAutomatic == false && shootAction[gunHandType].stateDown))
                    && timePassed >= timeToShoot)
                {
                    Shoot();
                }

                if (usingChargingHandle)
                {
                    PullChargingHandle();

                    if (currentlyPulled)
                    {
                        PullingChargingHandle();
                    }
                }

                if(magazine == null)
                {
                    //Magazine mag = gunHand.otherHand.currentAttachedObject.GetComponent<Magazine>();
                    if(gunHand.otherHand.currentAttachedObject != null && gunHand.otherHand.currentAttachedObject.GetComponent<Magazine>() != null)
                    {
                        Magazine mag = gunHand.otherHand.currentAttachedObject.GetComponent<Magazine>();
                        if (Vector3.Distance(mag.magazineOffset.transform.position, magazineLoadOffset.transform.position) <= magazineLoadDistance)
                        {
                            ReloadMagazine(mag);
                        }
                    }
                }

                //Debug.Log(chargingHandleOffset.transform.InverseTransformDirection(gunHand.otherHand.transform.position - chargingHandleOffset.transform.position));
                //Vector3 char2ToChar1 = gunHand.otherHand.transform.position - chargingHandleOffset.transform.position;
                //Vector3 localRelativePosition = chargingHandleOffset.transform.InverseTransformDirection(char2ToChar1);
                //Debug.Log(localRelativePosition);
            }

            else if(currentlyPulled)
            {
                ReleaseChargingHandle();
            }

            if (ejectAction[gunHandType].stateDown == true)
            {
                EjectMagazine();
            }
        }


        void Shoot()
        {
            if(bulletChambered == true)
            {
                timePassed = 0;
                EjectCasing(casingPrefab);
                GameObject bullet = Instantiate(bulletPrefab, barrelend.transform.position, barrelend.transform.rotation);
                bullet.GetComponent<Rigidbody>().velocity = barrelend.transform.forward * bulletSpeed;
                Destroy(bullet, 5f);

                if (magazine != null)
                    bulletChambered = magazine.GetNextBullet();

                else
                    bulletChambered = false;
            }
        }

        void EjectMagazine()
        {
            magazine.GetComponent<Collider>().enabled = true;
            magazine.GetComponent<Rigidbody>().isKinematic = false;
            magazine.GetComponent<Rigidbody>().useGravity = true;
            magazine.GetComponent<Interactable>().enabled = true;
            magazine.gameObject.transform.parent = null;

            //magazine.gameObject.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + new Vector3(0, -magazineEjectionSpeed, 0);
            magazine.gameObject.GetComponent<Rigidbody>().velocity += magazineEjectionSpeed * -transform.up;
            magazine = null;
        }

        void ReloadMagazine(Magazine mag)
        {


            //detach from hand;
            mag.GetComponent<Interactable>().attachedToHand.DetachObject(mag.gameObject);
            mag.GetComponent<Interactable>().enabled = false;

            magazine = mag;
            mag.gameObject.GetComponent<Collider>().enabled = false;
            mag.GetComponent<Rigidbody>().isKinematic = true;
            mag.GetComponent<Rigidbody>().velocity = new Vector3();
            mag.GetComponent<Rigidbody>().useGravity = false;

            mag.gameObject.transform.parent = magazineLoadOffset.transform;
            mag.gameObject.transform.position = magazineLoadOffset.transform.position;
            mag.gameObject.transform.rotation = magazineLoadOffset.transform.rotation;
        }


        void PullChargingHandle()
        {
            if (gunHand.otherHand.currentAttachedObject == chargingHandle)    //If hand is holding chargingHandle
            {
                currentlyPulled = true;

                //Attach the hand to the notch location
                //chargingHandle.GetComponent<Interactable>().attachedToHand = gunHand.otherHand;
                //Grabbable grabbable = notch.GetComponent<Grabbable>();
                //arrowHand.AttachObject(notch.gameObject, GrabTypes.Grip, grabbable.attachmentFlags, grabbable.attachmentOffset);
                //(gameObject, startingGrabType, attachmentFlags, attachmentOffset);

                //Debug.Log("attach");
            }

            //if (pullAction[gunHand.otherHand.handType].stateDown
            //    && Vector3.Distance(gunHand.otherHand.transform.position, chargingHandle.transform.position) <= distToPullCharghingHandleWithHand)     //if arrowHandisTrying to pull and the correct distance away
            //{
            //    if (gunHand.otherHand.currentAttachedObject == null)    //If hand is empty
            //    {
            //        currentlyPulled = true;

            //        //Attach the hand to the notch location
            //        chargingHandle.GetComponent<Interactable>().attachedToHand = gunHand.otherHand;
            //        //Grabbable grabbable = notch.GetComponent<Grabbable>();
            //        //arrowHand.AttachObject(notch.gameObject, GrabTypes.Grip, grabbable.attachmentFlags, grabbable.attachmentOffset);
            //        //(gameObject, startingGrabType, attachmentFlags, attachmentOffset);

            //        Debug.Log("attach");
            //    }
            //}

            //if (pullAction[arrowHand.handType].stateDown
            //                    && Vector3.Distance(arrowHand.transform.position, notch.transform.position) <= distToPullNotchWithHand)     //if arrowHandisTrying to pull and the correct distance away
            //{
            //    if (arrowHand.currentAttachedObject == null
            //        || arrowHand.currentAttachedObject.GetComponent<MyArrow>() != null)                                               //If hand is empty or holding an arrow
            //    {
            //        currentlyPulled = true;

            //        //Attach the hand to the notch location
            //        notch.GetComponent<Interactable>().attachedToHand = arrowHand;
            //        //Grabbable grabbable = notch.GetComponent<Grabbable>();
            //        //arrowHand.AttachObject(notch.gameObject, GrabTypes.Grip, grabbable.attachmentFlags, grabbable.attachmentOffset);
            //        //(gameObject, startingGrabType, attachmentFlags, attachmentOffset);

            //        Debug.Log("attach");

            //        if (arrowHand.currentAttachedObject.GetComponent<MyArrow>() != null)
            //        {
            //            GameObject arrow = arrowHand.currentAttachedObject;

            //            arrow.transform.position = notch.transform.position;
            //            arrow.transform.rotation = notch.transform.rotation;
            //            //arrowHand.DetachObject(arrow);
            //            //arrow.transform.parent = notch.transform;
            //        }
            //    }
            //}


        }

        void PullingChargingHandle()
        {

            //float handChargeHandleDist = Mathf.Abs(Vector3.Distance(gunHand.otherHand.transform.position, chargingHandleOffset.transform.position));
            float handChargeHandleDist = chargingHandleOffset.transform.InverseTransformDirection(gunHand.otherHand.transform.position - chargingHandleOffset.transform.position).z;
            //Debug.Log(chargingHandleOffset.transform.InverseTransformPoint(gunHand.otherHand.transform.position));

            handChargeHandleDist = Mathf.Abs(handChargeHandleDist);

            if (handChargeHandleDist >= notchPullDistanceMax)
            {
                if (bulletChambered)
                {
                    EjectCasing(shellPrefab);
                }

                if (magazine != null)
                    bulletChambered = magazine.GetNextBullet();
                else
                    bulletChambered = false;

                ReleaseChargingHandle();

                return;
            }

            currentDistPulled = Mathf.Clamp(handChargeHandleDist, 0f, notchPullDistanceMax);

            chargingHandle.transform.localPosition = new Vector3(0, 0, -currentDistPulled * 4);
            
        }

        void ReleaseChargingHandle()
        {
            currentlyPulled = false;

            //release the hand from the notch location
            //chargingHandle.GetComponent<Interactable>().attachedToHand = null;
            gunHand.otherHand.DetachObject(chargingHandle);

            chargingHandle.transform.position = chargingHandleOffset.transform.position;
            currentDistPulled = 0;

            //Debug.Log("dettach");
        }

        //private void ontriggerenter(collider other)
        //{
        //    if(other.getcomponent<magazine>() != null && magazine == null && other.getcomponent<interactable>().attachedtohand == gunhand.otherhand)
        //    {
        //        reloadmagazine(other.gameobject.getcomponent<magazine>());
        //    }
        //}

        void EjectCasing(GameObject prefabCasing)
        {
            if(prefabCasing != null)
            {
                GameObject casing = Instantiate(prefabCasing, casingEjectionPort.transform.position, casingEjectionPort.transform.rotation);
                casing.GetComponent<Rigidbody>().velocity = casingEjectionPort.transform.forward * casingEjectionSpeed;
                Destroy(casing, casingLifeTime);
            }
        }
    }
}
