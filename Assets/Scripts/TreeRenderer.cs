using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    [RequireComponent(typeof(TreeGenerator))]
    public class TreeRenderer : MonoBehaviour
    {
        public GameObject shootPrefab;
        public GameObject leafPrefab;
        public GameObject budPrefab;

        public List<GameObject> renderedObjects;

        private TreeGenerator treeGenerator;
        // Start is called before the first frame update
        void Start()
        {
            treeGenerator = GetComponent<TreeGenerator>();
            RenderTree();
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Grow!");
                RenderTree();
            }
        }

        public void RenderTree()
        {
            foreach (GameObject obj in renderedObjects)
            {
                Destroy(obj);
            }
            Tree tree = treeGenerator.tree;
            foreach (Branch branch in tree.branches)
            {
                GameObject newBranch = Instantiate(shootPrefab, branch.position, Quaternion.FromToRotation(Vector3.up, branch.direction), transform);
                renderedObjects.Add(newBranch);

                newBranch.transform.localScale = new Vector3(branch.diameter, branch.length, branch.diameter);
            }
            foreach (Bud bud in tree.buds)
            {
                renderedObjects.Add(Instantiate(budPrefab, bud.position, Quaternion.FromToRotation(Vector3.up, bud.direction), transform));
            }
        }
    }
}