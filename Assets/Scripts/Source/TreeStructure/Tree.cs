using RNGroot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RNGroot
{
    public class Tree
    {
    
        public List<Bud> buds = new List<Bud>();
        public List<Node> nodes = new List<Node>();
        public Node baseNode;

        //public List<Vector3> leaves;
        public UnityEvent changeEvent = new UnityEvent();
        public UnityEvent<Node> cutEvent = new UnityEvent<Node>();
        public Tree(Vector3 startPos, float startRadius)
        {
            // TODO: Should we start with a node instead if bud fate is introduced?
            baseNode = new Node(startPos, Vector3.up, startRadius, null);
            buds = new List<Bud>();
            nodes = new List<Node> { baseNode };
            Node.tree = this;

            this.AddBud(baseNode, Vector3.up);
        }
        public Tree(Tree copyTree)
        {
            // Copies the tree and everything in it recursively.
            //
            baseNode = new Node(copyTree.baseNode, null);
            RecursiveCopy(baseNode, ref nodes, ref buds);
        }

        public void RecursiveCopy(Node node, ref List<Node> nodes, ref List<Bud> buds)
        {
            nodes.Add(node);
            buds.AddRange(node.childBuds);

            foreach (Node childNode in node.childNodes)
            {
                nodes.Add(childNode);
            }
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
                bud.parent.terminal = false;
            }

            this.buds.Remove(bud);

            return newNode;
        }

        public Bud AddBud(Node node, Vector3 dir, Vector3 position)
        {
            Bud newBud = new Bud(position, dir, node);
            node.childBuds.Add(newBud);
            buds.Add(newBud);
            return newBud;
        }

        public Bud AddBud(Node node, Vector3 dir)
        {
            return AddBud(node, dir, node.position);
        }


    }

}