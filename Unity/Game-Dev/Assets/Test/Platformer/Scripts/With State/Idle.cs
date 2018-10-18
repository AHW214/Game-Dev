using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class Idle : State
    {
        public Idle(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (KeyValuePair<int, RaycastHit2D> pair in player.controller.hits[i])
                {
                    if (pair.Value)
                    {
                        player.displacement[i] = pair.Key * (pair.Value.distance - Controller2D.skinWidth);

                        if (pair.Value.distance <= Controller2D.skinWidth)
                        {
                            player.velocity[i] = 0;
                        }
                    }
                }
            }
        }
    }
}