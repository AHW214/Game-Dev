﻿using UnityEngine;

namespace FSM
{
    public class Grounded : Superstate
    {
        public Grounded(Player player) : base(player)
        {

        }

        public Grounded(State state) : base(state)
        {

        }

        protected override State DetermineSubstate()
        {
            return new Idle(player);
        }

        public override void Tick()
        {
            if (!player.controller.collisions.Grounded)
            {
                player.SetState(new Falling(player));
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                player.SetState(new Jumping(player));
            }

            else
            {
                base.Tick();
            }           
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            Debug.Log("Enter: Grounded");
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            Debug.Log("Exited: Grounded");
        }
    }
}