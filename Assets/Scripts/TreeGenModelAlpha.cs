using RNGroot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RNGroot
{
    public class TreeGenModelAlpha : GrowthAlgorithm
    {
        public IEnvironmentalInput environmentalInput;
        public IBranchingRules branchingRules;
        public TreeMetrics treeMetrics;
        private List<Node> addedNodes = new List<Node>();

        protected override void Start()
        {
            environmentalInput = new SpaceColonization(tree, new UnitSphereEnvelope());
            branchingRules = new RandomTreeBranchingRules();
            base.Start();
        }

        public override void Grow()
        {
            Dictionary<Bud, float> EValues = new Dictionary<Bud, float>();

            // Calculate E value.
            //
            foreach(Bud bud in tree.buds)
            {
                float E = environmentalInput.CalculateE(bud);
                EValues.Add(bud, E);
            }

            // Decide internal growth values.
            
            // TODO: Bud fate here.
            //
            // Grow nodes.
            //
            for (int i = tree.buds.Count - 1; i > -1; i--)
            {
                Bud bud = tree.buds[i];

                // TODO: Bud fate here
                //
                if (EValues[bud] > 0)
                {
                    Node newNode = tree.AddNode(bud, branchLength, branchRadius);
                    addedNodes.Add(newNode);
                }
            }

            // Place buds.
            //
            branchingRules.PlaceBuds(tree, addedNodes);

            treeMetrics = TreeMetricHelper.CalculateMetrics(tree);

            // (Re)calculate environmental influence.
            //
            if (addedNodes.Count > 0)
            {
                environmentalInput.AddNodes(addedNodes);
                addedNodes.Clear();
            }
        }
        
        public void Cut(Node node)
        {
            List<Node> cutNodes = new List<Node>();
            Cut(node, ref cutNodes);
            node.cut = true;
            environmentalInput.RemoveNodes(cutNodes);
            // TODO: Review terminal bool logic to see if we need a cut bool instead.
        }

        private void Cut(Node node, ref List<Node> cutNodes)
        {
            cutNodes.Add(node);

            foreach (Node child in node.childNodes)
            {
                Cut(child, ref cutNodes);
                tree.nodes.Remove(child);
            }

            node.childNodes.Clear();

            foreach (Bud childBud in node.childBuds)
            {
                tree.buds.Remove(childBud);
            }
            node.childBuds.Clear();
        }
    }
}
