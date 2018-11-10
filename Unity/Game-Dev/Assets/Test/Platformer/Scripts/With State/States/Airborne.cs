using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Airborne : State<Player>
    {
        private Vector2? wallNormal;

        public Airborne(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if (entity.input.x != 0 && (wallNormal = entity.controller.collisions[0][entity.facing]?.normal) != null
                && Mathf.Approximately(Vector2.Angle(wallNormal.Value, Vector2.up), 90))
            {
                entity.StateMachine.SetState("WallSliding");
            }
        }

        public override void OnEnter()
        {
            entity.movementSpeed = entity.normalSpeed;
            entity.accelerationTime = entity.accelerationTimeAirborne;

            entity.StateMachine.SetState(entity.velocity.y > 0 ? "Rising" : "Falling");
        }

        public override void OnExit()
        {

        }
    }
}
