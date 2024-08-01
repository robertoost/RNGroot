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

        private GameObject branchParent;
        private GameObject budParent;
        private GameObject leafParent;

        public bool showLeaves;
        private bool _showLeavesCheck;
        public bool showDormantBuds;
        private bool _showDormantBudsCheck;
        public bool showBuds;
        private bool _showBudsCheck;
        private bool _queueUpdate;
        
        public List<GameObject> renderedObjects;

        private TreeGenerator treeGenerator;

        private void OnValidate()
        {
            if (showLeaves != _showLeavesCheck || showDormantBuds != _showDormantBudsCheck || showBuds != _showBudsCheck)
            {
                _showLeavesCheck = showLeaves;
                _showDormantBudsCheck = showDormantBuds;
                _showBudsCheck = showBuds;
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
            branchParent = new GameObject("Branches");
            branchParent.transform.parent = this.transform;
            budParent = new GameObject("Buds");
            budParent.transform.parent = this.transform;
            leafParent = new GameObject("Leaves");
            leafParent.transform.parent = this.transform;

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
            if (bud.dormant && showDormantBuds == false || showBuds == false)
                return;
            Vector3 budPos = bud.position + transform.position;
            Quaternion budRotation = Quaternion.FromToRotation(Vector3.up, bud.direction);
            GameObject budObj = Instantiate(budPrefab, budPos, budRotation, budParent.transform);

            renderedObjects.Add(budObj);
        }

        public void RenderNode(Node node)
        {
            GameObject useBranchPrefab = node.cut ? cutShootPrefab : shootPrefab;
            Vector3 branchPos = node.parentNode.position + transform.position;
            Quaternion branchRotation = Quaternion.FromToRotation(Vector3.up, node.Direction());
            GameObject newNode = Instantiate(useBranchPrefab, branchPos, branchRotation, branchParent.transform);

            renderedObjects.Add(newNode);

            newNode.transform.localScale = new Vector3(node.diameter, Mathf.Abs((node.position - node.parentNode.position).magnitude), node.diameter);

            foreach (Node childNode in node.childNodes)
            {
                RenderNode(childNode);
            }

            if (node.terminal && node.cut == false && node.childNodes.Count == 0 && showLeaves)
            {
                Vector3 leafPosition = node.position + transform.position;
                Quaternion leafRotation = Quaternion.FromToRotation(Vector3.up, node.direction);
                GameObject newLeaf = Instantiate(leafPrefab, leafPosition, leafRotation, leafParent.transform);
                renderedObjects.Add(newLeaf);
            }
        }
    }
}