using FSMRev3;

namespace PlatformerFSM
{
    public class Sliding : State<Player>, ICoreState
    {
        public string AnimName => "sliding";
        public bool CollisionsEnabled => true;

        public Sliding(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (entity.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.StateMachine.SetState("Crouching");
            }
        }

        public override void OnEnter()
        {
            entity.LockFacing();

            entity.velocity.x = entity.facing * 20;
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {
            entity.LockFacing(false);
        }
    }
}