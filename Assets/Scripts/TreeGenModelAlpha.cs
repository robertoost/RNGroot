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
            environmentalInput = new SpaceColonization(tree, new SphereTreeEnvelope());
            branchingRules = new RandomTreeBranchingRules();
            base.Start();
        }

        private void OnDrawGizmosSelected()
        {

            // For now, show space colonization markers here.
            //
            if (environmentalInput == null) return;

            SpaceColonization spaceCol = ((SpaceColonization)environmentalInput);
            Gizmos.color = Color.green;
            foreach (int marker_id in spaceCol.unoccupied_marker_ids)
            {
                Gizmos.DrawSphere(spaceCol.markers[marker_id], 0.07f);
            }
            Gizmos.color = Color.red;
            foreach (int marker_id in spaceCol.marker_occupation.Keys)
            {
                Gizmos.DrawSphere(spaceCol.markers[marker_id], 0.05f);
            }
        }

        public override void Grow()
        {
            // (Re)calculate environmental influence.
            //
            if (addedNodes.Count > 0)
            {
                environmentalInput.AddNodes(addedNodes);
                addedNodes.Clear();
            }

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

            // TODO: L-System for bud placement
            // Place buds.
            branchingRules.PlaceBuds(tree, addedNodes);

            treeMetrics = TreeMetricHelper.CalculateMetrics(tree);
            // TODO: Grow / leave dormant buds lower
        }
    }
}
