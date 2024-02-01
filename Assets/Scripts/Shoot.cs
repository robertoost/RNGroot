using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public class Shoot
    {
        public Vector3 position;
        public Vector3 direction;

        // Shoots have access to further shoots
        //
        public List<Shoot> childShoot;
        public List<Bud> childBuds;

        // TODO: Positions for leaves?
        //
        public List<Vector3> leaves;

        // TODO: Do shoots have references to their buds?

        // TODO: Length might be variable or based on a constant.
        //
        public float length;

        // TODO: Radius dependent on pipe model.
        //
        public float radius;

        public Shoot(Vector3 pos, Vector3 dir, float len, float rad)
        {
            position = pos;
            direction = dir;
            length = len;
            radius = rad;
        }
    }
}