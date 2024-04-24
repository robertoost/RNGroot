using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{


    public class RandomTree : GrowthAlgorithm
    {

        public override void Grow()
        {
            foreach (Bud bud in tree.buds)
            {
                // Create new node attached to bud's parent node.
                //
                tree.AddNode(bud, branchLength, branchRadius);
            }
            tree.buds.Clear();
            
            //Debug.Log("Adding nodes");
            //Debug.Log(tree.nodes.Count);

            foreach (Node node in tree.nodes)
            {
                if(node.childBuds.Count == 0 && node.terminal == true)
                {
                    // Generate a random direction based on the node direction, and rotate by it.
                    //
                    Vector3 randomDir = new Vector3(Random.value, Random.value, Random.value);
                    randomDir = Vector3.Normalize(randomDir);
                    float angle = Random.Range(-40, 40);

                    Vector3 nodeDirection = node.parentNode != null ? node.Direction() : Vector3.up;
                    Vector3 rotationAxis = Vector3.Cross(randomDir, nodeDirection);

                    // Rotate by random axis and angle along node direction
                    Vector3 newDir = Quaternion.AngleAxis(angle, rotationAxis) * nodeDirection;
                    Vector3 newDirB = Quaternion.AngleAxis(-angle, rotationAxis) * nodeDirection;

                    // Randomly chooses to add a second lateral bud or not.

                    tree.AddBud(node, newDir);
                   
                    if (Random.value > 0.3)
                    {
                        tree.AddBud(node, newDirB);
                    }

                    node.terminal = false;
                }
            }

        }
    }
}