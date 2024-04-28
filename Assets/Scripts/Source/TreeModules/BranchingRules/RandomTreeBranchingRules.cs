
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public class RandomTreeBranchingRules : IBranchingRules
    {
        public void PlaceBuds(Tree tree, List<Node> nodes)
        {
            // Place buds and lateral buds simultaneously.
            // We can worry about bud fate later.
            //
            foreach (Node node in nodes)
            {
                if (node.childBuds.Count == 0 && node.terminal == true)
                {
                    tree.AddBud(node, node.position, true);

                    // If the branch above is a branching point, or already has buds, continue.
                    //
                    //if (node.parentNode.childNodes.Count > 1 || node.parentNode.childBuds.Count > 0 || node.parentNode == tree.baseNode) continue;

                    // Generate a random direction based on the node direction, and rotate by it.
                    //
                    Vector3 randomDir = new Vector3(Random.value, Random.value, Random.value);
                    randomDir = Vector3.Normalize(randomDir);
                    float angle = Random.Range(20, 35);

                    Vector3 nodeDirection = node.parentNode != null ? node.Direction() : Vector3.up;
                    Vector3 rotationAxis = Vector3.Cross(randomDir, nodeDirection);

                    // Rotate by random axis and angle along node direction
                    Vector3 newDir = Quaternion.AngleAxis(angle, rotationAxis) * nodeDirection;
                    Vector3 newDirB = Quaternion.AngleAxis(-angle, rotationAxis) * nodeDirection;

                    // Randomly chooses to add a second lateral bud or not.
                    tree.AddBud(node, newDir, node.position + 0.001f * rotationAxis.normalized, false);
                    tree.AddBud(node, newDirB, node.position - 0.001f * rotationAxis.normalized, false);
                }
            }
        }
    }
}