using RNGroot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree
{
    public List<Bud> buds;
    public List<Shoot> shoots;
    public List<Vector3> leaves;

    public GameObject shootPrefab;
    public GameObject leafPrefab;
    public GameObject budPrefab;

    public Bud treeBase;

    public Tree(Vector3 startPos, Vector3 startDir)
    {
        treeBase = new Bud(startPos, startDir);
        buds = new List<Bud>();
    }
}
