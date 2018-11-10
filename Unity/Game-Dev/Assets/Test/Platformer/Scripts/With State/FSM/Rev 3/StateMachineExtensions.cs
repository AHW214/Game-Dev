using PlatformerFSM;

namespace FSMRev3
{
    public static class StateMachineExtensions
    {
        public static void Initialize(this StateMachine<Player> stateMachine, Player player)
        {
            stateMachine.AddState(new Grounded(player), new Standing(player), new Crouching(player));
            stateMachine.AddState(new Crouching(player), new IdleCrouching(player), new Sliding(player));
            stateMachine.AddState(new Standing(player), new IdleStanding(player), new Moving(player));
            stateMachine.AddState(new Moving(player), new Walking(player), new Running(player));

            stateMachine.AddState(new Airborne(player), new Falling(player), new Rising(player));

            stateMachine.AddState(new Dashing(player));

            stateMachine.AddState(new WallSliding(player));

            stateMachine.AddState(new AscendingPlatform(player));
            stateMachine.AddState(new DescendingPlatform(player));

            stateMachine.LogTransitions = player.LogState;
            stateMachine.SetState("IdleStanding");
            stateMachine.Start();
        }
    }
}
