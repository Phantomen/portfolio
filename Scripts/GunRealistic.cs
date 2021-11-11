using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class GunRealistic : MonoBehaviour
    {
        public bool isAutomatic = true;

        public bool usingChargingHandle = false;

        public GameObject chargingHandle;
        public GameObject chargingHandleOffset;

        public float notchPullDistanceMax = 0.2f;
        private float currentDistPulled = 0f;
        public float shootSpeed = 20f;

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

                if (((isAutomatic == true && shootAction[gunHandType].state) || (isAutomatic == false && shootAction[gunHandType].stateDown))
                    && timePassed >= timeToShoot)
                {
                    Shoot();
                }

                //If the charging hande is enabled and is being used
                if (usingChargingHandle)
                {
                    PullChargingHandle();
                    if (currentlyPulled)
                    {
                        PullingChargingHandle();
                    }
                }

                //If there is no magazine in the gun and the other gun is holding on, and that magazine is close enough, attatch that magazine to the gun
                if(magazine == null)
                {
                    if(gunHand.otherHand.currentAttachedObject != null && gunHand.otherHand.currentAttachedObject.GetComponent<Magazine>() != null)
                    {
                        Magazine mag = gunHand.otherHand.currentAttachedObject.GetComponent<Magazine>();
                        if (Vector3.Distance(mag.magazineOffset.transform.position, magazineLoadOffset.transform.position) <= magazineLoadDistance)
                        {
                            ReloadMagazine(mag);
                        }
                    }
                }
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
            //If a bullet is chambered in the gun, shoot
            if(bulletChambered == true)
            {
                timePassed = 0;
                EjectCasing(casingPrefab);
                GameObject bullet = Instantiate(bulletPrefab, barrelend.transform.position, barrelend.transform.rotation);
                bullet.GetComponent<Rigidbody>().velocity = barrelend.transform.forward * bulletSpeed;
                Destroy(bullet, 5f);

                //If there is a magazine attatched, try to get a new bullet chambered into the gun
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
            //If hand is holding the chargingHandle
            if (gunHand.otherHand.currentAttachedObject == chargingHandle)
            {
                currentlyPulled = true;
            }
        }

        void PullingChargingHandle()
        {
            float handChargeHandleDist = chargingHandleOffset.transform.InverseTransformDirection(gunHand.otherHand.transform.position - chargingHandleOffset.transform.position).z;
            handChargeHandleDist = Mathf.Abs(handChargeHandleDist);

            //If the charging handle has been pulled back enough to eject a bullet
            //eject the bullet if there is one and if there is a magazine attached, try to get a new bullet chambered and release the charging handle
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

            gunHand.otherHand.DetachObject(chargingHandle);

            chargingHandle.transform.position = chargingHandleOffset.transform.position;
            currentDistPulled = 0;
        }

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
