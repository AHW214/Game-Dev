using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Airborne : State<Player>
    {
        public Airborne(Player player) : base(player)
        {

        }

        public override void Tick()
        {
      
        }

        public override void OnEnter()
        {
            Debug.Log("Enter: Airborne");
        }

        public override void OnExit()
        {
            Debug.Log("Exited: Airborne");
        }
    }
}
