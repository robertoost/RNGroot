using System;
using System.Collections.Generic;

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
        
        
        
        Dictionary<(Node, Node), float> nutritionValues;

        public void CalculateNutrition(Tree tree, Dictionary<Bud, float> budValues) {
            // TODO: Implement.
            // TODO: Is this how I want this method to be called?
            foreach (Node node in tree.nodes)
            {
                // We've found a terminal node. Do a nutrition pass.
                if (node.terminal == true)
                {
                    Node currentNode = node;
                    // TODO: What values do we fill in here?
                    // TODO: Alter tree so basi- and acro-petal passes can be made more easily.
                    while (currentNode != tree.baseNode)
                    {
                        float nodeBudE = 0;
                        foreach (Bud childBud in node.childBuds)
                        {
                            float budE;
                            nodeBudE += budValues.TryGetValue(childBud, out budE) ? budE : 0;
                        }

                        Node parentNode = currentNode.parentNode;
                        (Node, Node) key = (parentNode, currentNode);

                        bool segmentAlreadySet = nutritionValues.TryAdd(key, nodeBudE);
                        if (segmentAlreadySet)
                        {
                            nutritionValues[key] += nodeBudE;
                        }
                    }
                }
            }
        }
    }
}