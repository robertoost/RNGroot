using RNGroot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{    
    public interface IEnvironmentalInput
    {
        /// <summary>
        /// Calculates environmental input E for any given bud.
        /// </summary>
        /// <returns>float E</returns>
        public abstract void CalculateBudInformation();

        public abstract IEnvironmentalInput Copy(Tree copiedTree);
        public abstract void AddNodes(List<Node> addedNodes);
        public abstract void RemoveNodes(List<Node> removedNodes);
    }
}
