using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public class Bud
    {
        // Could just be a struct...
        //
        Vector3 position;
        Vector3 direction;

        // Global constant occupancy radius
        // Global constant perception volume angle and distance

        public Bud(Vector3 pos, Vector3 dir)
        {
            position = pos;
            direction = dir;
        }
    }
}