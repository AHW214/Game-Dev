﻿using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Falling : State<Player>
    {
        private Collider2D platform;

        public Falling(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if ((platform = entity.controller.collisions[1][-1]?.collider) != null)
            {
                if (entity.input.y < 0 && platform.CompareTag("One Way Platform"))
                {
                    entity.StateMachine.SetState("OneWayPlatform");
                }

                else
                {
                    entity.StateMachine.SetState("Grounded");
                }              
            }
            
            else if (entity.velocity.y < -15)
            {
                entity.animator.Play("long fall");
            }
        }

        public override void OnEnter()
        {
            entity.animator.Play("falling");
        }

        public override void OnExit()
        {

        }
    }
}
