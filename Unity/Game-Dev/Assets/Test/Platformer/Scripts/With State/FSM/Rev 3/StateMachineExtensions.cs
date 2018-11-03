using PlatformerFSM;

namespace FSMRev3
{
    public static class StateMachineExtensions
    {
        public static void Initialize(this StateMachine<Player> stateMachine, Player player)
        {
            stateMachine.AddState(new Grounded(player), new Idle(player), new Crouching(player),
                new Walking(player), new Running(player), new Sliding(player));

            stateMachine.AddState(new Airborne(player), new Falling(player), new Rising(player));

            stateMachine.AddState(new WallSliding(player));

            stateMachine.AddState(new AscendingPlatform(player));
            stateMachine.AddState(new DescendingPlatform(player));

            stateMachine.SetState("Idle");
            stateMachine.Start();
        }
    }
}
