using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Standing : State<Player>
    {
        public Standing(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {           
            if (entity.input.y < 0)
            {
                entity.StateMachine.SetState("Crouching");
            }

            else if (Input.GetKeyDown(KeyCode.E))
            {
                entity.StateMachine.SetState("Dashing");
            }
        }

        public override void OnEnter()
        {
            entity.StateMachine.SetState(entity.input.x != 0 ? "Moving" : "IdleStanding");
        }

        public override void OnExit()
        {

        }
    }
}