using UnityEngine;
namespace RNGroot
{
    public class TreeGenerator : MonoBehaviour
    {
        // TODO: What I want is to be able to pick and choose what parts of the model you want to use.
        // That way, I can create prefabs!
        //
        public TreeModelAlpha treeModel;
        public SpaceColonization environmentalInput;
        public int growthSteps = 10;

        public Tree tree;

        public float branchLength = 0.45f;
        public float branchRadius = 0.05f;

        private void Awake()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            tree = new Tree(transform.position, branchRadius);
        }

        // Start is called before the first frame update
        void Start()
        {
            IEnvelope envelope = new UnitSphereEnvelope();
            SpaceColonization environmentalInput = new SpaceColonization(tree, envelope);
            IBranchingRules branchingRules = new RandomTreeBranchingRules();
            ResourceAllocation borchertHonda = new ResourceAllocation();
            treeModel = new TreeModelAlpha(tree, environmentalInput, branchingRules, borchertHonda, branchLength, branchRadius);

            for (int i = 0; i < growthSteps; i++)
            {
                treeModel.Grow();
            }
            tree.changeEvent.Invoke();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                treeModel.Grow();
            }
            tree.changeEvent.Invoke();
        }
    }
}
