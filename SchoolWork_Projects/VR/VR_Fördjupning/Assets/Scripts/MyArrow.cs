using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class MyArrow : MonoBehaviour
    {
        private bool shot = false;
        private bool stuck = false;

        Rigidbody rigid;
        Interactable inter;

        // Start is called before the first frame update
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            inter = GetComponent<Interactable>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!stuck && shot)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rigid.velocity), 0.2f);
            }

            if (inter.attachedToHand)
            {
                Grabbed();
            }
    }

        public void Shot()
        {
            shot = true;
        }

        public void Grabbed()
        {
            stuck = false;
            shot = false;
            rigid.constraints = RigidbodyConstraints.None;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (shot)
            {
                Stick(collision);
            }
        }

        private void Stick(Collision collision)
        {
            shot = false;
            stuck = true;
            rigid.constraints = RigidbodyConstraints.FreezeAll;

            transform.parent = collision.gameObject.transform;
        }
    }
}