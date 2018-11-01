﻿using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class WallJumping : State<Player>, ICoreState
    {
        public string AnimName => "jumping";
        public bool CollisionsEnabled => true;

        public WallJumping(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (entity.controller.collisions[1][-1] != null)
            {
                entity.StateMachine.SetState("Idle");
            }

            else if (entity.input.x != 0 && entity.controller.collisions[0][entity.facing] != null)
            {
                entity.StateMachine.SetState("WallSliding");
            }

            else if (entity.velocity.y < 0)
            {
                entity.StateMachine.SetState("Falling");
            }
        }

        public override void OnEnter()
        {
            entity.velocity = new Vector2(-entity.facing * 10, entity.jumpVelocityRange[0]); //jank
            Debug.Log("Entered: WallJumping");
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {
            Debug.Log("Exited: WallJumping");
        }
    }
}
