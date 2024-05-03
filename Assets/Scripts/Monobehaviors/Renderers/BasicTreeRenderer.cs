using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RNGroot
{
    public class BasicTreeRenderer : MonoBehaviour
    {
        public GameObject shootPrefab;
        public GameObject cutShootPrefab;
        public GameObject leafPrefab;
        public GameObject budPrefab;
        public bool showLeaves;
        private bool _showLeavesCheck;
        private bool _queueUpdate;
        
        public List<GameObject> renderedObjects;

        private TreeGenerator treeGenerator;

        private void OnValidate()
        {
            if (showLeaves != _showLeavesCheck)
            {
                _showLeavesCheck = showLeaves;
                _queueUpdate = true;
            }
        }

        private void Update()
        {
            if (_queueUpdate)
            {
                RenderTree();
                _queueUpdate = false;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            treeGenerator = GetComponent<TreeGenerator>();
            treeGenerator.tree.changeEvent.AddListener(new UnityAction(RenderTree));
            RenderTree();
        }

        public void RenderTree()
        {
            Tree tree = treeGenerator.tree;
            foreach (GameObject obj in renderedObjects)
            {
                Destroy(obj);
            }

            renderedObjects.Clear();
            foreach (Node childNode in tree.baseNode.childNodes)
                RenderNode(childNode);

            foreach (Bud bud in tree.buds)
                RenderBud(bud);
        }

        public void RenderBud(Bud bud)
        {
            renderedObjects.Add(Instantiate(budPrefab, bud.position + transform.position, Quaternion.FromToRotation(Vector3.up, bud.direction), transform));
        }

        public void RenderNode(Node node)
        {
            GameObject newNode = Instantiate(
                node.cut ? cutShootPrefab : shootPrefab, 
                node.parentNode.position + transform.position, Quaternion.FromToRotation(Vector3.up, node.Direction()), transform);
            renderedObjects.Add(newNode);

            newNode.transform.localScale = new Vector3(node.diameter, Mathf.Abs((node.position - node.parentNode.position).magnitude), node.diameter);

            foreach (Node childNode in node.childNodes)
            {
                RenderNode(childNode);
            }

            if (node.terminal && node.cut == false && node.childNodes.Count == 0 && showLeaves)
            {
                renderedObjects.Add(Instantiate(leafPrefab, node.position + transform.position, Quaternion.FromToRotation(Vector3.up, node.direction), transform));
            }
        }
    }
}