using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RNGroot
{
    [RequireComponent(typeof(GrowthAlgorithm))]
    public class BasicTreeRenderer : MonoBehaviour
    {
        public GameObject shootPrefab;
        public GameObject leafPrefab;
        public GameObject budPrefab;
        
        public List<GameObject> renderedObjects;

        private GrowthAlgorithm treeGenerator;

        // Start is called before the first frame update
        void Start()
        {
            treeGenerator = GetComponent<GrowthAlgorithm>();
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
            foreach (Node childNode in tree.baseNode.childNodes)
                RenderNode(childNode);

            foreach (Bud bud in tree.buds)
                RenderBud(bud);
        }

        public void RenderBud(Bud bud)
        {
            renderedObjects.Add(Instantiate(budPrefab, bud.position, Quaternion.FromToRotation(Vector3.up, bud.direction), transform));
        }

        public void RenderNode(Node node)
        {
            GameObject newNode = Instantiate(shootPrefab, node.parentNode.position, Quaternion.FromToRotation(Vector3.up, node.Direction()), transform);
            renderedObjects.Add(newNode);

            newNode.transform.localScale = new Vector3(node.diameter, Mathf.Abs((node.position - node.parentNode.position).magnitude), node.diameter);

            foreach (Node childNode in node.childNodes)
            {
                RenderNode(childNode);
            }
        }
    }
}