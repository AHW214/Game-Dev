using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFSM
{
    public class Collisions
    {
        private readonly IDictionary<int, RaycastHit2D?>[] components = new IDictionary<int, RaycastHit2D?>[2];

        public Collisions()
        {
            for (int i = 0; i < 2; i++)
            {
                components[i] = new Dictionary<int, RaycastHit2D?>
                    {
                        {  1, null },
                        { -1, null }
                    };
            }
        }

        public void Reset()
        {
            for (int i = 0; i < 2; i++)
            {
                Reset(i);
            }
        }

        public void Reset(int index)
        {
            components[index][1] = null;
            components[index][-1] = null;
        }

        public IDictionary<int, RaycastHit2D?> X
        {
            get { return components[0]; }
        }

        public IDictionary<int, RaycastHit2D?> Y
        {
            get { return components[1]; }
        }

        public IDictionary<int, RaycastHit2D?> this[int index]
        {
            get { return components[index]; }
        }

        public bool Grounded
        {
            get { return components[1][-1] != null; }
        }
    }
}