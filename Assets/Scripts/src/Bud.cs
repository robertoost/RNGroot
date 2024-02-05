using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public class Bud
    {
        // Could just be a struct...
        //
        public Vector3 position;
        public Vector3 direction;
        public Branch parent;

        // Global constant occupancy radius
        // Global constant perception volume angle and distance

        public Bud(Vector3 pos, Vector3 dir, Branch parent)
        {
            this.parent = parent;
            position = pos;
            direction = dir;
        }
    }
}