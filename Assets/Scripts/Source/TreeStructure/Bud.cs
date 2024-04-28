using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public class Bud
    {
        public Vector3 position;
        public Vector3 direction;
        public Node parent;
        public bool mainAxis;
        public float E = 0f;
        public float nutrients = 0f;
        public Vector3 EDirection;

        // Global constant occupancy radius
        // Global constant perception volume angle and distance

        public Bud(Vector3 position, Vector3 direction, Node parent, bool mainAxis)
        {
            this.position = position;
            this.direction = direction;
            this.parent = parent;
            this.mainAxis = mainAxis;
        }

        public Bud(Bud copyBud, Node parent)
        {
            this.position = copyBud.position;
            this.direction = copyBud.direction;
            this.parent = parent;
            this.mainAxis = copyBud.mainAxis;
        }
    }
}