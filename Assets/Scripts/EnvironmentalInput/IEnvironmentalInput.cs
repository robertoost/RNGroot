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
    public abstract Dictionary<Bud, (float, Vector3)> CalculateBudInformation();

    public abstract void AddNodes(List<Node> addedNodes);
    public abstract void RemoveNodes(List<Node> removedNodes);
}
