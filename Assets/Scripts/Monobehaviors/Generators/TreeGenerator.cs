using UnityEngine;
namespace RNGroot
{
    public class TreeGenerator : MonoBehaviour
    {
        // TODO: What I want is to be able to pick and choose what parts of the model you want to use.
        // That way, I can create prefabs!
        //
        public TreeModelAlpha treeModel;

        public int growthSteps = 10;

        public Tree tree;

        public float branchLength = 1f;
        public float branchRadius = 0.05f;


        // Start is called before the first frame update
        void Start()
        {
            tree = new Tree(transform.position, branchRadius);
            IEnvelope envelope = new UnitSphereEnvelope();
            IEnvironmentalInput environmentalInput = new SpaceColonization(tree, envelope);
            IBranchingRules branchingRules = new RandomTreeBranchingRules();
            treeModel = new TreeModelAlpha(tree, environmentalInput, branchingRules, branchLength, branchRadius);


            for (int i = 0; i < growthSteps; i++)
            {
                treeModel.Grow();
            }
            treeModel.tree.changeEvent.Invoke();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                treeModel.Grow();
            }
            treeModel.tree.changeEvent.Invoke();
        }
    }
}
