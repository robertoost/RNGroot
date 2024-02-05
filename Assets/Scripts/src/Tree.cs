using RNGroot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree
{
    public List<Bud> buds;
    public List<Branch> branches;
    //public List<Vector3> leaves;

    public Tree(Vector3 startPos, Vector3 startDir)
    {
        Bud treeBase = new Bud(startPos, startDir, null);
        buds = new List<Bud> { treeBase };
        branches = new List<Branch>();
        //leaves = new List<Vector3>();
    }
}
