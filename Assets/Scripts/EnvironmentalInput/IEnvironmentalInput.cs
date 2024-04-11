using RNGroot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnvironmentalInput
{
    /// <summary>
    /// Calculates environmental input E for any given bud.
    /// </summary>
    /// <returns>float E</returns>
    public abstract float CalculateE(Bud bud);

    public abstract void AddNodes(List<Node> addedNodes);
    public abstract void RemoveNodes(List<Node> removedNodes);
}
