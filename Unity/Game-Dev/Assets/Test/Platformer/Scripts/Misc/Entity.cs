using UnityEngine;
using StateTestNew;

public class Entity : MonoBehaviour
{
    public readonly StateMachine stateMachine = new StateMachine();

    private void Start()
    {
        stateMachine.AddState(new A(this), new AP(this));
        stateMachine.AddState(new B(this), new BP(this));

        stateMachine.SetState("A");
        stateMachine.Start();
    }

    private void Update()
    {
        stateMachine.Tick();
    }
}
