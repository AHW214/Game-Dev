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

        public IDictionary<int, RaycastHit2D?> X => components[0];
        public IDictionary<int, RaycastHit2D?> Y => components[1];

        public IDictionary<int, RaycastHit2D?> this[int index] => components[index];

        public bool Right => X[1]  != null;
        public bool Left  => X[-1] != null;

        public bool Above => Y[1]  != null;
        public bool Below => Y[-1] != null;

        public bool Horizontal => Right || Left;
        public bool Vertical   => Above || Below;

        public bool Contains(Collider2D collider)
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (int dir in components[i].Keys)
                {
                    if (components[i][dir]?.collider == collider)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}