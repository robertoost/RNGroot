using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RNGroot
{
    public class Node
    {
        // TODO: Allow for multiple trees.
        public static Tree tree;
        public Vector3 position;
        public Vector3 direction;

        public bool terminal = true;
        public bool cut = false;
        public bool mainAxis = false;
        public float nodeE = 0;

        // Nodes have access to further shoots
        //
        public Node parentNode;
        public List<Node> childNodes = new List<Node>();
        public List<Bud> childBuds = new List<Bud>();

        //// TODO: Positions for leaves?
        ////
        //public List<Vector3> leaves;

        // TODO: Do nodes have references to their buds?

        public float diameter;

        public Node(Vector3 pos, Vector3 dir, float rad, Node parent, bool mainAxis)
        {
            position = pos;
            direction = dir;
            diameter = rad;
            parentNode = parent;
            this.mainAxis = mainAxis;
        }

        /// <summary>
        /// Copies the given node, assigning a new parent node.
        /// </summary>
        public Node(Node copyNode, Node parent)
        {
            position = copyNode.position;
            direction = copyNode.direction;
            diameter = copyNode.diameter;
            mainAxis = copyNode.mainAxis;
            parentNode = parent;

            terminal = copyNode.terminal;
            cut = copyNode.cut;

            foreach (Node childNode in copyNode.childNodes)
            {
                childNodes.Add(new Node(childNode, this));
            }
            foreach (Bud childBud in copyNode.childBuds)
            {
                childBuds.Add(new Bud(childBud, this));
            }
        }

        public bool IsTerminal()
        {
            return childNodes.Count == 0;
        }

        public float CalculateMass()
        {
            // TODO: Calculate mass of tree starting from this node.
            return -1f;
        }

        public Vector3 Direction()
        {
            Vector3 nodeDirection = Vector3.Normalize(position - parentNode.position);
            return nodeDirection;
        }

        public void UpdateDiameter(float n)
        {
            if (terminal == false)
            {
                float totalDiameter = 0;
                foreach(Node node in childNodes)
                {
                    totalDiameter += Mathf.Pow(node.diameter, n);
                }
                diameter = Mathf.Pow(totalDiameter, 1 / n);
            }

            if (parentNode != null)
            {
                parentNode.UpdateDiameter(n);
            }
        }
    }
}