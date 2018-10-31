using UnityEngine;
using FSMRev3;

public class Entity : MonoBehaviour
{
    public readonly StateMachine<Entity> stateMachine = new StateMachine<Entity>();

    private void Start()
    {

    }

    private void Update()
    {
        stateMachine.Tick();
    }
}
