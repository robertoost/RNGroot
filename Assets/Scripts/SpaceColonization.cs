using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public interface GrowthAlgorithm
    {
        /// <summary>
        /// Grows the tree structure by one timestep.
        /// </summary>
        public void Grow();

        public Tree tree { get; set; }
    }

    public class SpaceColonization : GrowthAlgorithm
    {
        private List<Vector3> markers;
        private List<Vector3> occupied_markers;
        const int n_markers = 10;

        // TODO: How do I wanna initialize space col?
        //
        public SpaceColonization(Tree tree)
        {
            this.tree = tree;
            for (int i = 0; i < n_markers; i++)
            {
                markers.Add(new Vector3(0,2,0) + (Random.insideUnitSphere * 2));
            }
        }

        public Tree tree { get; set; }

        public void Grow()
        {
            foreach (Bud bud in tree.buds)
            {
                // Okay let's take a step back
            }
        }
    }
}