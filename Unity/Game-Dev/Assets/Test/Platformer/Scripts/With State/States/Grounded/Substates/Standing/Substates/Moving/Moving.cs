using FSMRev3;
using UnityEngine;

namespace PlatformerFSM
{
    public class Moving : State<Player>
    {
        public Moving(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (entity.input.x == 0 || entity.controller.collisions.Horizontal)
            {
                entity.StateMachine.SetState("IdleStanding");
            }
        }

        public override void OnEnter()
        {
            entity.StateMachine.SetState(Input.GetKey(KeyCode.LeftShift) ? "Running" : "Walking");
        }

        public override void OnExit()
        {

        }
    }

}