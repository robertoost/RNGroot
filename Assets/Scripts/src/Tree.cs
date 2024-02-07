using RNGroot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public class Tree
    {
    
        public List<Bud> buds;
        public List<Branch> branches;
        //public List<Vector3> leaves;

        public Tree(Vector3 startPos, Vector3 startDir)
        {
            // TODO: Should we start with a branch instead if bud fate is introduced?
            Bud treeBase = new Bud(startPos, startDir, null);
            buds = new List<Bud> { treeBase };
            branches = new List<Branch>();
            //leaves = new List<Vector3>();
        }

        /// <summary>
        /// Adds a new branch to the tree branch list. Attaches the branch to the bud's parent branch.
        /// </summary>
        /// <param name="bud">The bud this branch will originate from.</param>
        /// <returns>The newly instantiated Branch.</returns>
        public Branch AddBranch(Bud bud)
        {
            // Create new branch at bud position in bud direction.
            Branch newBranch = new Branch(bud.position, bud.direction, 1, 0.2f, bud.parent);
            branches.Add(newBranch);

            // If this is the starting branch
            //
            if (bud.parent != null)
            {
                bud.parent.childBranch.Add(newBranch);
                bud.parent.UpdateDiameter(2);
            }

            return newBranch;
        }
    }

}