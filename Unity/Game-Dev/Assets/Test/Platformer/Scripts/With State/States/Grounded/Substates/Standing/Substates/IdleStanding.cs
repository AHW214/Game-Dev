using FSMRev3;

namespace PlatformerFSM
{
    public class IdleStanding : State<Player>
    {
        public IdleStanding(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (entity.input.x != 0 && !entity.controller.collisions.Horizontal)
            {
                entity.StateMachine.SetState("Moving");
            }          
        }

        public override void OnEnter()
        {
            entity.animator.Play("idle");
        }

        public override void OnExit()
        {

        }
    }
}