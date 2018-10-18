﻿using UnityEngine;

namespace FSM
{
    public class Idle : State
    {
        public Idle(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            Debug.Log("idle");

            for (int i = 0; i < 2; i++)
            {
                foreach (int dir in player.controller.hits[i].Keys)
                {
                    RaycastHit2D? hit = player.controller.hits[i][dir];

                    if (hit != null)
                    {
                        player.displacement[i] = dir * (hit.Value.distance - Controller2D.skinWidth);

                        if (hit.Value.distance <= Controller2D.skinWidth)
                        {
                            player.velocity[i] = 0;
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.SetState(new Jumping(player));
            }

            player.transform.Translate(player.displacement);
        }
    }
}