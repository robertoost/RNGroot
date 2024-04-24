using UnityEngine;

namespace RNGroot
{

    public class CopyTreeGen : MonoBehaviour
    {
        public TreeGenModelAlpha treeGenModel;
        public Tree tree;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                tree = new Tree(treeGenModel.tree);
            }
        }
    }
}
