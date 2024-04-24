using UnityEngine;
namespace RNGroot
{
    public class TreeGenBeta : MonoBehaviour
    {
        // What I want is to be able to pick and choose what parts of the model you want to use.
        // That way, I can create prefabs!
        //
        public TreeGenBeta treeGenModel;

        // Start is called before the first frame update
        void Start()
        {
            // An independent tree generator
            // 
            // 1. Has its own inspector for manual operations.
            // 2. Grows independently.
            // 3. Is generated at the position the tree is placed at.
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}
