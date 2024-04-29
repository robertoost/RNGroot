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
        private float branchLength;
        private float branchRadius;
        public Tree tree { get; set; }

        // TODO: Get spacecol variables back in the editor?
        public IEnvironmentalInput environmentalInput;
        public IBranchingRules branchingRules;
        public ResourceAllocation borchertHonda;

        // TODO: Get something better in place for tree metrics...
        public TreeMetrics treeMetrics = new TreeMetrics();

        private List<Node> addedNodes = new List<Node>();


        public TreeModelAlpha(Tree tree, IEnvironmentalInput environmentalInput, IBranchingRules branchingRules, ResourceAllocation bh, float branchLength, float branchRadius)
        {
            this.tree = tree;
            this.environmentalInput = environmentalInput;
            this.branchingRules = branchingRules;
            borchertHonda = bh;

            // TODO: standard length and radius globals?
            //
            this.branchLength = branchLength;
            this.branchRadius = branchRadius;
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
            environmentalInput.CalculateBudInformation();

            // TODO: Decide internal growth values.
            //
            borchertHonda.CalculateNutrition(tree);

            // Grow nodes.
            //
            for (int i = tree.buds.Count - 1; i > -1; i--)
            {
                Bud bud = tree.buds[i];

                // TODO: Bud fate here
                //
                if (bud.E > 0)
                {
                    float nutrientBranchLength = bud.nutrients / 30;
                    nutrientBranchLength = nutrientBranchLength > branchLength ? branchLength : nutrientBranchLength;
                    //Debug.Log("Bud nutrition " + bud.nutrients);
                    // Growth direction is affected by tropisms.
                    //
                    Vector3 budDirection = (bud.direction * PREFERRED_DIR + bud.EDirection + Vector3.down * GRAVITROPISM).normalized;
                    bud.direction = budDirection;

                    Node newNode = tree.AddNode(bud, branchLength, branchRadius);
                    addedNodes.Add(newNode);
                } else
                {
                    bud.parent.childBuds.Remove(bud);
                    tree.buds.Remove(bud);
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
