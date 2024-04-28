using System.Collections.Generic;
using System.Linq;

namespace RNGroot
{
    /// <summary>
    /// Borchert & Honda model of resource allocation.
    /// </summary>
    public class ResourceAllocation
    {
        // Do a basipetal pass.
        // Read environmental values for each bud.
        // For each internode we store a value based on that env value.
        // Then we do an acropetal pass to deliver nutrients.
        
        
        
        Dictionary<Bud, float> budNutritionValues = new Dictionary<Bud, float>();
        
        // lambda
        //
        public float mainBias = 0.7f;
        public float proportionality = 2f;

        public void CalculateNutrition(Tree tree)
        {
            budNutritionValues = new Dictionary<Bud, float>();

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
            // Qm = main lightval, Ql = lateral lightval
            //
            float mainLight = 0f;
            float lateralLight = 0f;
            foreach (Node childNode in node.childNodes)
            {
                if (childNode.mainAxis)
                {
                    mainLight = childNode.nodeE;
                } else
                {
                    float partLateralLight = 0f;
                    partLateralLight = childNode.nodeE;
                    lateralLight += partLateralLight;
                }
            }
            
            // Nutrients to be dispersed to the main branch.
            //
            float lambdaQm = mainBias * mainLight;
            float lambdaQl = (1 - mainBias) * lateralLight;
            float mainNutrients = nutrition * (lambdaQm / (lambdaQm + lambdaQl));

            // Whether to disperse nutrients away from the main branch/bud, for when it is lost.
            // TODO: Just assign another node to become the main branch?
            //
            bool mainBudLost = node.childBuds.Any(childBud => childBud.mainAxis && childBud.E == 0);
            bool mainBranchLost = node.childNodes.Any(childNode => childNode.mainAxis && (childNode.cut || childNode.nodeE == 0));

            // Get the amount of lateral branches that have not been cut.
            //
            foreach (Node childNode in node.childNodes)
            {
                if (childNode.cut)
                    continue;

                // If there's no light values up this segment, then there are no buds to feed.
                //
                if (childNode.nodeE == 0)
                {
                    // Going up the dead-end branch!
                    //
                    continue;
                }

                float chosenNutrients = 0;

                // Use lambda to determine where the nutrition goes.
                //
                if (childNode.mainAxis)
                {
                    chosenNutrients = mainNutrients;
                } else
                {
                    float nodeLambdaQl = (1 - mainBias) * childNode.nodeE;
                    chosenNutrients = nutrition * (nodeLambdaQl / (lambdaQm + nodeLambdaQl));
                }

                DistributeNutrition(childNode, chosenNutrients);
            }

            // Childbuds do some stuff.
            //
            foreach(Bud childBud in node.childBuds)
            {
                if (childBud.E == 0)
                    continue;
                float budLambdaQl = (1 - mainBias) * childBud.E;
                float lateralBudNutrients = nutrition * (budLambdaQl / (lambdaQm + budLambdaQl));

                childBud.nutrients = lateralBudNutrients;
            }
        }
    }
}