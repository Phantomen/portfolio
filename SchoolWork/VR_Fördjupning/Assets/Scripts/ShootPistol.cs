using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class ShootPistol : MonoBehaviour
    {
        public SteamVR_Action_Boolean shootAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Gun", "Shoot");

        public Transform barrelend;

        public GameObject bulletPrefab;
        public int bpm = 30;
        public int bulletSpeed = 10;
        private float timePassed = 0f;
        private float timeToShoot;

        private Interactable interactable;
        private SteamVR_Input_Sources hand;

        private bool shooting;

        // Start is called before the first frame update
        void Start()
        {
            interactable = GetComponent<Interactable>();
            timeToShoot = 60f / bpm;
        }

        // Update is called once per frame
        void Update()
        {
            timePassed += Time.deltaTime;

            if (interactable.attachedToHand)
            {
                hand = interactable.attachedToHand.handType;
                shooting = shootAction[hand].stateDown;

                if (shooting && timePassed >= timeToShoot)
                {
                    timePassed = 0;
                    GameObject bullet = Instantiate(bulletPrefab, barrelend.transform.position, barrelend.transform.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = barrelend.transform.forward * bulletSpeed;
                    Destroy(bullet, 5f); 
                }
            }
            else
            {

            }
        }
    }
}

