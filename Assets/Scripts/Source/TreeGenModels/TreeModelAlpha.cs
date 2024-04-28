using System;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    [Serializable]
    public class TreeModelAlpha
    {
        // TODO: Separate global constants.
        //
        public float GRAVITROPISM = 0;
        public float PREFERRED_DIR = 0;
        public float branchLength = 1f;
        public float branchRadius = 0.05f;
        public Tree tree { get; set; }

        // TODO: Get spacecol variables back in the editor?
        public IEnvironmentalInput environmentalInput;
        public IBranchingRules branchingRules;
        public TreeMetrics treeMetrics = new TreeMetrics();

        private List<Node> addedNodes = new List<Node>();

        public TreeModelAlpha()
        {
            // TODO: Pass a starting position?
            tree = new Tree(new Vector3(0, 0, 0), branchLength, branchRadius);
            environmentalInput = new SpaceColonization(tree, new UnitSphereEnvelope());
            branchingRules = new RandomTreeBranchingRules();
        }

        public TreeModelAlpha(TreeModelAlpha copyModel)
        {
            tree = new Tree(copyModel.tree);
            // TODO: Copy environmental input.
            // environmentalInput = environmentalInput.Copy();
        }

        public void Grow()
        {
            // Calculate E value.
            //
            Dictionary<Bud, (float, Vector3)> E_values = environmentalInput.CalculateBudInformation();

            // TODO: Decide internal growth values.

            // Grow nodes.
            //
            for (int i = tree.buds.Count - 1; i > -1; i--)
            {
                Bud bud = tree.buds[i];

                (float E, Vector3 E_dir) = E_values[bud];

                // TODO: Bud fate here
                //
                if (E > 0)
                {
                    // TODO: Tropisms here.
                    Vector3 budDirection = (bud.direction * PREFERRED_DIR + E_dir + Vector3.down * GRAVITROPISM).normalized;
                    bud.direction = budDirection;

                    Node newNode = tree.AddNode(bud, branchLength, branchRadius);
                    addedNodes.Add(newNode);
                }
            }

            // Place buds.
            //
            branchingRules.PlaceBuds(tree, addedNodes);

            treeMetrics = TreeMetricHelper.CalculateMetrics(tree, treeMetrics);

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
