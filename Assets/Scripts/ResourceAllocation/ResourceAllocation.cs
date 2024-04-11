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
        }
    }
}