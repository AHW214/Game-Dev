namespace Game
{
    public enum State
    {
        Idle,
        Walking,
        Running
    }

    public static class StateNames
    {
        public const string IdleAnim = "Idle";
        public const string WalkAnim = "Walk";
        public const string RunAnim = "Run";

        static public string GetStateName(State state)
        {
            switch (state)
            {
                case State.Idle:
                    return IdleAnim;
                case State.Walking:
                    return WalkAnim;
                case State.Running:
                    return RunAnim;
            }

            return null;
        }
    }
}
