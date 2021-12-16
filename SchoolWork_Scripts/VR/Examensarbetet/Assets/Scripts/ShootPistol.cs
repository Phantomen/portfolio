using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class ShootPistol : MonoBehaviour
    {
        public SteamVR_Action_Boolean shootAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Shoot");

        public AudioSource gunSound;

        public Transform barrelend;

        SteamVR_Input_Sources gunHand;

        public float range = 100f;
        private float timePassed = 0f;
        public float timeToShoot = 0.25f;


        public float effectDisplayTime = 0.2f;
        Ray shootRay = new Ray();
        RaycastHit shootHit;
        int shootableMask;
        LineRenderer gunLine;

        private bool shooting;

        // Start is called before the first frame update
        void Start()
        {
            timePassed = timeToShoot;

            shootableMask = LayerMask.GetMask("Shootable");
            gunLine = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            timePassed += Time.deltaTime;

                shooting = shootAction.stateDown;

                if (shooting && timePassed >= timeToShoot)
                {
                    Shoot();
                }

            if (timePassed >= effectDisplayTime)
            {
                DisableEffects();
            }
        }

        void Shoot()
        {
            timePassed = 0;

            gunSound.Play();

            gunLine.enabled = true;
            gunLine.SetPosition(0, barrelend.position);

            shootRay.origin = barrelend.position;
            shootRay.direction = barrelend.forward;

            if(Physics.Raycast (shootRay, out shootHit, range))
            {
                ShotTarget target = shootHit.collider.GetComponent<ShotTarget>();
                if (target != null)
                {
                    target.TargetShot();
                }
                gunLine.SetPosition(1, shootHit.point);
            }
            else
            {
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            }
        }

        void DisableEffects()
        {
            gunLine.enabled = false;
        }
    }
}

