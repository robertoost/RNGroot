
using UnityEngine;

namespace RNGroot
{
    public struct TreeMetrics
    {
        public float DBH;
        // TODO: Add other metrics
    }

    public static class TreeMetricHelper
    {

        public static TreeMetrics CalculateMetrics(Tree tree)
        {
            TreeMetrics metrics = new TreeMetrics();
            metrics.DBH = CalculateDBH(tree);
            
            // TODO: Other metrics

            return metrics;
        }

        public static float CalculateDBH(Tree tree)
        {
            // Starting from the tree's base
            // Find nodes below and above 1.35 in length (meters)
            // Return tree diameter at this point.

            // Problem: There are some edge cases, such as:
            // 1. The tree forking below breast height
            // 2. The tree forking right above breast height
            // 3. Branch or deformity at breast height
            // 
            // Solutions: Hard code a tree that does not fork until above breast height.
            // 1: Determine what constitutes a "main branch" and what's a "lateral". Difference in angle cannot be greater than...
            //    A tree fork has two roughly equal diameter branches.
            //    
            // 2 & 3: Implement circumference calculation with a failsafe if the fork just happened.

            // For now: Always choose the widest branching node.
            // 
            float length = 0f;
            float DBH = 0f;
            Node currentNode = tree.baseNode;
            int iterations = 0;

            const int MAX_ITERATIONS = 100;
            const float BREAST_HEIGHT = 1.35f;

            while (currentNode.childNodes.Count > 0 && length < BREAST_HEIGHT && iterations < MAX_ITERATIONS)
            {

                Node nextNode = null;
                float widestDiameter = 0f;

                // Choose widest child node to find our measurement point on.
                //
                foreach (Node childNode in currentNode.childNodes)
                {
                    if (widestDiameter < childNode.diameter)
                    {
                        widestDiameter = childNode.diameter;
                        nextNode = childNode;
                    }
                }

                // If the trunk is angled, this way we still get the DBH at 1.35 meters.
                //
                length += Vector3.Distance(currentNode.position, nextNode.position);

                // TODO: Right now we choose the diameter of the lower node.
                // If the next node isn't a fork, this should be an interpolation.
                //
                DBH = length >= 1.35f ? currentNode.diameter : DBH;

                currentNode = nextNode;
                iterations++;
            }

            return DBH;
        }
    }
}