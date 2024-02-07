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

    public class RandomTree : GrowthAlgorithm
    {
        public List<Vector3> markers = new List<Vector3>();
        public List<Vector3> occupied_markers = new List<Vector3>();
        const int n_markers = 100;

        // TODO: How do I wanna initialize space col?
        //
        public RandomTree(Tree tree)
        {
            this.tree = tree;
            for (int i = 0; i < n_markers; i++)
            {
                markers.Add(new Vector3(0,7,0) + (Random.insideUnitSphere * 5));
            }
        }

        public Tree tree { get; set; }

        public void Grow()
        {
            foreach (Bud bud in tree.buds)
            {
                // Create new branch attached to bud's parent branch.
                //
                tree.AddBranch(bud);
            }
            tree.buds.Clear();
            foreach (Branch branch in tree.branches)
            {
                if(branch.terminal == true)
                {                    
                    // Generate a random direction based on the branch direction, and rotate by it.
                    //
                    Vector3 randomDir = new Vector3(Random.value, Random.value, Random.value);
                    randomDir = Vector3.Normalize(randomDir);
                    float angle = Random.Range(-40, 40);
                    Vector3 rotationAxis = Vector3.Cross(randomDir, branch.direction);

                    // Rotate by random axis and angle along branch direction
                    Vector3 newDir = Quaternion.AngleAxis(angle, rotationAxis) * branch.direction;
                    Vector3 newDirB = Quaternion.AngleAxis(-angle, rotationAxis) * branch.direction;

                    // Randomly chooses to add a second lateral bud or not.
                    tree.buds.Add(new Bud(branch.position + (branch.direction * branch.length), newDirB, branch));
                   
                    if (Random.value > 0.3)
                    {
                        tree.buds.Add(new Bud(branch.position + (branch.direction * branch.length), newDir, branch));
                    }


                    branch.terminal = false;
                }
            }

        }
    }
}