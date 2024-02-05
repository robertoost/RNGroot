using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public class Branch
    {
        public Vector3 position;
        public Vector3 direction;
        public bool terminal = true;

        // Branches have access to further shoots
        //
        public Branch parentBranch;
        public List<Branch> childBranch = new List<Branch>();
        public List<Bud> childBuds = new List<Bud>();

        // TODO: Positions for leaves?
        //
        public List<Vector3> leaves;

        // TODO: Do shoots have references to their buds?

        // TODO: Length might be variable or based on a constant.
        //
        public float length;

        // TODO: Radius dependent on pipe model.
        //
        public float diameter;

        public Branch(Vector3 pos, Vector3 dir, float len, float rad, Branch parent)
        {
            position = pos;
            direction = dir;
            length = len;
            diameter = rad;
            parentBranch = parent;
        }

        public bool IsTerminal()
        {
            return childBranch.Count == 0;
        }

        public void UpdateDiameter(float n)
        {
            if (IsTerminal())
                diameter = 1;
            else
            {
                float totalDiameter = 0;
                foreach(Branch branch in childBranch)
                {
                    totalDiameter += Mathf.Pow(branch.diameter, n);
                }
                diameter = Mathf.Pow(totalDiameter, 1 / n);
            }

            if (parentBranch != null)
            {
                parentBranch.UpdateDiameter(n);
            }
        }
    }
}