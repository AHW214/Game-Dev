using System;
using UnityEngine;

namespace FSM
{
    public abstract class State
    {
        public Player player;

        public abstract Type TSuperstate { get; }
        public abstract string animName { get; }

        public abstract void Tick();

        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }

        public void HandleCollisions()
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

        public State(Player player)
        {
            this.player = player;
        }
    }
}
