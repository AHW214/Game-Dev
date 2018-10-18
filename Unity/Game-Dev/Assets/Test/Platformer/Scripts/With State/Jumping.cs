using UnityEngine;

namespace FSM
{
    public class Jumping : State
    {
        public Jumping(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            Debug.Log("jumping");

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

                        if (i == 1 && dir == -1)
                        {
                            player.SetState(new Idle(player));
                        }
                    }
                }
            }

            player.transform.Translate(player.displacement);
        }

        public override void OnStateEnter()
        {
            player.velocity.y = player.jumpVelocityRange[1];

            player.transform.Translate(player.displacement); //refactor
        }
    }
}
