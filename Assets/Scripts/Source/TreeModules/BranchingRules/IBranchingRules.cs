using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public interface IBranchingRules
    {
        public void PlaceBuds(Tree tree, List<Node> nodes, bool regrowth);
    }

}
