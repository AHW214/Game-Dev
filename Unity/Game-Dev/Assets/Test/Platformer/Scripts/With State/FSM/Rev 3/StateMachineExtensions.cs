using PlatformerFSM;

namespace FSMRev3
{
    public static class StateMachineExtensions
    {
        public static void Initialize(this StateMachine<Player> stateMachine, Player player)
        {
            Idle idle = new Idle(player);
            Walking walking = new Walking(player);            
            Grounded grounded = new Grounded(player);

            Falling falling = new Falling(player);
            Jumping jumping = new Jumping(player);
            WallSliding wallSliding = new WallSliding(player);
            WallJumping wallJumping = new WallJumping(player);
            Airborne airborne = new Airborne(player);

            AscendingPlatform ascendingPlatform = new AscendingPlatform(player);
            DescendingPlatform descendingPlatform = new DescendingPlatform(player);


            stateMachine.AddState(idle, grounded);
            stateMachine.AddState(walking, grounded);

            stateMachine.AddState(falling, airborne);
            stateMachine.AddState(jumping, airborne);
            stateMachine.AddState(wallSliding, airborne);
            stateMachine.AddState(wallJumping, airborne);

            stateMachine.AddState(ascendingPlatform);
            stateMachine.AddState(descendingPlatform);

            stateMachine.SetState("Idle");
            stateMachine.Start();
        }
    }
}
