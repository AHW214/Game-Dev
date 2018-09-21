using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Player : MovingObject
    {
        private Animator ar;

        private new void Start()
        {
            base.Start();
            ar = GetComponent<Animator>();
        }

        private void Update()
        {
            ReadInput();
        }

        private void ReadInput()
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (input.magnitude >= 0.5F)
            {
                heading = new Direction(input);

                Vector2 pos = 0.1F * Vector2.up + (Vector2)transform.position;
                Debug.DrawRay(pos, input.normalized, Color.red, Time.deltaTime);
                Debug.DrawRay(pos, heading.v2, Color.blue, Time.deltaTime);

                Move();
            }
        }
    }
}

