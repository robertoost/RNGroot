using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RNGroot
{
    public class Node
    {
        public Vector3 position;
        public bool terminal = true;

        // Nodees have access to further shoots
        //
        public Node parentNode;
        public List<Node> childNodes = new List<Node>();
        public List<Bud> childBuds = new List<Bud>();

        // TODO: Positions for leaves?
        //
        public List<Vector3> leaves;

        // TODO: Do nodes have references to their buds?

        public float diameter;

        public Node(Vector3 pos, float rad, Node parent)
        {
            position = pos;
            diameter = rad;
            parentNode = parent;
        }

        public bool IsTerminal()
        {
            return childNodes.Count == 0;
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