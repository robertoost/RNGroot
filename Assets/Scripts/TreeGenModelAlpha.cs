using RNGroot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RNGroot
{
    public class TreeGenModelAlpha : GrowthAlgorithm
    {
        public IEnvironmentalInput environmentalInput;
        private List<Node> addedNodes = new List<Node>();

        protected override void Start()
        {
            environmentalInput = new SpaceColonization(tree, new SphereTreeEnvelope());
            base.Start();
        }

        private void OnDrawGizmosSelected()
        {
            if (environmentalInput == null) return;

            SpaceColonization spaceCol = ((SpaceColonization)environmentalInput);
            Gizmos.color = Color.green;
            foreach (int marker_id in spaceCol.unoccupied_marker_ids)
            {
                Gizmos.DrawSphere(spaceCol.markers[marker_id], 0.07f);
            }
            //Gizmos.color = Color.red;
            //foreach(int marker_id in spaceCol.marker_occupation.Keys)
            //{
            //    Gizmos.DrawSphere(spaceCol.markers[marker_id], 0.05f);
            //}
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
            AddRandomBuds(addedNodes);

            // TODO: Grow / leave dormant buds lower
        }
        private void AddRandomBuds(List<Node> chosenNodes)
        {
            foreach (Node node in tree.nodes)
            {
                if (node.childBuds.Count == 0 && node.terminal == true)
                {

                    // Generate a random direction based on the node direction, and rotate by it.
                    //
                    Vector3 randomDir = new Vector3(Random.value, Random.value, Random.value);
                    randomDir = Vector3.Normalize(randomDir);
                    float angle = Random.Range(20, 35);
                    // Random positive or negative
                    int posNeg = Random.Range(0, 2) * 2 - 1;

                    Vector3 nodeDirection = node.parentNode != null ? node.Direction() : Vector3.up;
                    Vector3 rotationAxis = Vector3.Cross(randomDir, nodeDirection);

                    // Rotate by random axis and angle along node direction
                    Vector3 newDir = Quaternion.AngleAxis(angle, rotationAxis) * nodeDirection;
                    Vector3 newDirB = Quaternion.AngleAxis(-angle, rotationAxis) * nodeDirection;

                    // Randomly chooses to add a second lateral bud or not.

                    tree.AddBud(node, newDir);

                    if (Random.value > 0.3)
                    {
                        tree.AddBud(node, newDirB);
                    }

                    node.terminal = false;
                }
            }
        }
    }
}
