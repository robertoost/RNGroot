using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    /// <summary>
    /// Borchert & Honda model of resource allocation.
    /// </summary>
    public class ResourceAllocation
    {           
        // lambda
        //
        public float mainBias = 0.5f;
        public float proportionality = 2f;

        public void CalculateNutrition(Tree tree)
        {
            if (tree.buds.Count == 0)
                return;

            // Reset all nodes
            foreach (Node node in tree.nodes)
            {
                node.nodeE = 0;
            }

            // Do acropetal pass.
            //
            foreach (Bud bud in tree.buds)
            {
                Node currentNode = bud.parent;

                // If a bud has no space/light, continue.
                //
                if (bud.E == 0)
                    continue;

                while (currentNode != null)
                {
                    // Parent node.
                    //
                    currentNode.nodeE += bud.E;
                    currentNode = currentNode.parentNode;
                }
            }

            // Get final base value.
            //
            if (tree.baseNode.nodeE == 0)
                return;

            // Distribute nutrition.
            //
            DistributeNutrition(tree.baseNode, proportionality * tree.baseNode.nodeE);
        }

        private void DistributeNutrition(Node node, float nutrition)
        {
            //// Determine whether we want to disperse nutrients away from the main branch/bud, for when it is lost.
            ////
            //bool mainBudViable = node.childBuds.Any(childBud => (childBud.mainAxis && childBud.E > 0));
            //bool mainBranchIntact = node.childNodes.Any(childNode => childNode.mainAxis && !childNode.cut && childNode.nodeE > 0);
            //bool lateralBudViable = node.childBuds.Any(childBud => !childBud.mainAxis && childBud.E > 0);
            //bool lateralBranchIntact = node.childNodes.Any(childNode => !childNode.mainAxis && !childNode.cut && childNode.nodeE > 0);
            //bool mainViable = mainBudViable || mainBranchIntact;
            //bool lateralViable = lateralBudViable || lateralBranchIntact;

            float mainLight = 0f;
            float lateralLight = 0f;

            foreach(Bud childBud in node.childBuds)
            {                
                if (childBud.mainAxis)
                {
                    mainLight = childBud.E;
                } else
                {
                    lateralLight += childBud.E;
                }
            }

            foreach (Node childNode in node.childNodes)
            {
                if (childNode.mainAxis)
                {
                    mainLight = childNode.nodeE;
                } else
                {
                    lateralLight += childNode.nodeE;
                }
            }           
            
            // Nutrients to be dispersed to the main branch.
            //
            float lambdaQm = mainBias * mainLight;
            float lambdaQl = (1 - mainBias) * lateralLight;

            foreach (Node childNode in node.childNodes)
            {
                if (childNode.cut || childNode.nodeE == 0)
                    continue;

                float chosenNutrients;

                // Use lambda to determine where the nutrition goes.
                //
                if (childNode.mainAxis )
                {
                    float nodeLambdaQm = mainBias * childNode.nodeE;
                    chosenNutrients = nutrition * (nodeLambdaQm / (nodeLambdaQm + lambdaQl));
                } else
                {
                    float nodeLambdaQl = (1 - mainBias) * childNode.nodeE;
                    chosenNutrients = nutrition * (nodeLambdaQl / (lambdaQm + nodeLambdaQl));
                }

                DistributeNutrition(childNode, chosenNutrients);
            }

            // Childbud nutrients.
            //
            foreach(Bud childBud in node.childBuds)
            {
                if (childBud.E == 0)
                    continue;

                float budNutrients;

                if (childBud.mainAxis)
                {
                    float budLambdaQm = mainBias * childBud.E;
                    budNutrients = nutrition * (budLambdaQm / (budLambdaQm + lambdaQl));
                }
                else
                {
                    float budLambdaQl = (1 - mainBias) * childBud.E;
                    budNutrients = nutrition * (budLambdaQl / (lambdaQm + budLambdaQl));
                }

                childBud.nutrients = budNutrients;
            }
        }
    }
}