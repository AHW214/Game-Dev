using UnityEngine;

namespace FSM
{
    public class StateTest : MonoBehaviour
    {
        public readonly StateMachine stateMachine = new StateMachine();

        private void Start()
        {
            stateMachine.SetState(new CoreA(this));
        }

        private void Update()
        {
            stateMachine.state.Tick();
        }
    }
}