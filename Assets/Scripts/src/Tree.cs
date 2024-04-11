using RNGroot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RNGroot
{
    public class Tree
    {
    
        public List<Bud> buds;
        public List<Node> nodes;
        public Node baseNode;

        public float startRadius;
        public float branchLength;
        //public List<Vector3> leaves;
        public UnityEvent changeEvent = new UnityEvent();
        public UnityEvent<Node> cutEvent = new UnityEvent<Node>();
        public Tree(Vector3 startPos, float startRadius, float branchLength)
        {
            // TODO: Should we start with a node instead if bud fate is introduced?
            baseNode = new Node(startPos, Vector3.up, startRadius, null);
            buds = new List<Bud>();
            nodes = new List<Node> { baseNode };
            Node.tree = this;

            this.branchLength = branchLength;
            this.startRadius = startRadius;
        }

        /// <summary>
        /// Adds a new node to the tree node list. Attaches the node to the bud's parent node.
        /// </summary>
        /// <param name="bud">The bud this node will originate from.</param>
        /// <returns>The newly instantiated Node.</returns>
        public Node AddNode(Bud bud, float distance, float radius)
        {
            // Create new node at bud position in bud direction.
            Node newNode = new Node(bud.position + (bud.direction * distance), bud.direction, radius, bud.parent);
            nodes.Add(newNode);

            // If this is the starting node
            //
            if (bud.parent != null)
            {
                bud.parent.childBuds.Remove(bud);
                bud.parent.childNodes.Add(newNode);
                bud.parent.UpdateDiameter(2);
            }

            this.buds.Remove(bud);

            return newNode;
        }

        public Bud AddBud(Node node, Vector3 dir)
        {
            Bud newBud = new Bud(node.position, dir, node);
            node.childBuds.Add(newBud);
            buds.Add(newBud);
            return newBud;
        }
    }

}