using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Player : MovingObject
    {     
        private Animator ar;
        private Coroutine inst = null;

        private new void Start()
        {
            base.Start();
            ar = GetComponent<Animator>();
            ar.Play("Idle");
        }

        private void Update()
        {
            ReadInput();
        }

        private void ReadInput()
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (input.magnitude >= 0.1F)
            {
                heading = new Direction(input);

                Vector2 pos = 0.1F * Vector2.up + (Vector2)transform.position;
                Debug.DrawRay(pos, input.normalized, Color.red, Time.deltaTime);
                Debug.DrawRay(pos, heading.v2, Color.blue, Time.deltaTime);

                ar.SetFloat("FaceX", heading.v2.x);
                ar.SetFloat("FaceY", heading.v2.y);

                if (input.magnitude >= 0.7F)
                {
                    if (Input.GetButton("B"))
                    {
                        ar.Play("Roll");
                        movementSpeed = 1.5F;
                    }

                    else
                    {
                        ar.Play("Walk");
                        movementSpeed = 1.0F;
                    }
                    
                    Move();
                }

                else if (!ar.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    ar.Play("Idle");
                }
            }

            else if (!ar.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                ar.Play("Idle");
            }
        }

        private IEnumerator OneTimeAnim(string stateName)
        {
            ar.Play(stateName);

            while (!ar.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                yield return null;
            }

            float length = ar.GetCurrentAnimatorStateInfo(0).length;

            for (float t = 0.0F; t < length; t += Time.deltaTime)
            {
                yield return null;
            }

            inst = null;
        }
    }
}