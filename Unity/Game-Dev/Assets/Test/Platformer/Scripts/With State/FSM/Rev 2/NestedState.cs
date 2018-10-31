using System;
using UnityEngine;
using PlatformerFSM;

namespace FSMRev2
{
    public abstract class _State
    {      
        public Player player;

        public virtual Type TSuperstate { get; } = null;

        public abstract void Tick();

        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
    }

    public abstract class NestedState : _State
    {
        public _State state;

        public void SetState(_State _state)
        {
            state?.OnStateExit();
            (state = _state)?.OnStateEnter();
        }

        public override void Tick()
        {
            state.Tick();
        }

        public override void OnStateEnter()
        {
            state.OnStateEnter();
        }

        public override void OnStateExit()
        {
            state.OnStateExit();
        }

        public NestedState(_State state)
        {
            player = state.player;

            SetState(state);
        }
    }

    public abstract class CoreState : _State
    {
        /*
        private void HandleCollisions()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (int dir in player.controller.collisions[i].Keys)
                {
                    RaycastHit2D? hit = player.controller.collisions[i][dir];

                    if (hit != null)
                    {
                        CollisionHandler(i, dir, hit.Value);
                    }
                }
            }

            player.transform.Translate(player.displacement);
        }

        protected virtual void CollisionHandler(int component, int dir, RaycastHit2D hit)
        {
            float disp = hit.distance - Controller2D.skinWidth;

            player.displacement[component] = dir * disp;

            if (disp < 1E-5)
            {
                player.velocity[component] = 0;
            }
        }
       

        public override void Tick()
        {
            HandleCollisions();
        }
        */

        public CoreState(Player _player)
        {
            player = _player;
        }
    }

    public interface IAnimated
    {
        string AnimName { get; }
    }

    public class CoreA : CoreState
    {
        public override Type TSuperstate => typeof(NestedA);

        public CoreA(Player player) : base (player)
        {

        }

        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                //player.stateMachine.SetState(new CoreB(player));
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entered: core state A");
        }
    }

    public class NestedA : NestedState
    {
        public NestedA(_State state) : base(state)
        {

        }

        public override void OnStateEnter()
        {
            Debug.Log("Entered: nested state A");
        }
    }

    public class CoreB : CoreState
    {
        public override Type TSuperstate => typeof(NestedB);

        public CoreB(Player player) : base(player)
        {
            
        }

        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //player.stateMachine.SetState(new CoreA(player));
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entered: core state B");
        }
    }

    public class NestedB : NestedState
    {
        public NestedB(_State state) : base(state)
        {

        }

        public override void OnStateEnter()
        {
            Debug.Log("Entered: nested state B");
        }
    }
}
