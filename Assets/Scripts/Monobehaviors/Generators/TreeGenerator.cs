using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RNGroot
{
    public class TreeGenerator : MonoBehaviour
    {

        public enum E_Envelope
        {
            Sphere,
            Hemisphere,
            ClassicTree
        }

        // TODO: What I want is to be able to pick and choose what parts of the model you want to use.
        // That way, I can create prefabs!
        //
        public TreeModelAlpha treeModel;
        public SpaceColonization environmentalInput;

        [Range(1, 100)]
        public int growthSteps = 10;

        public bool cutTree;
        [Range(1, 20)]
        public int cutEveryXSteps = 5;
        private int lowerCutDepthBound = 2;
        [Range(2, 20)]
        public int upperCutDepthBound = 20;
        [Range(5, 40)]
        public int stopCuttingAfterYear = 30;

        public Tree tree;
        [Range(-0.8f, 0.8f)]
        public float GRAVITROPISM = 0;
        [Range(0.0f, 0.8f)]
        public float PREFERRED_DIR = 0;
        public float branchLength = 0.45f;
        public float branchRadius = 0.05f;

        [Range(1000, 20000)]
        public int n_markers = 1000;
        public float occupancy_radius = 0.6f;
        public float perception_angle = 90;
        public float perception_distance = 1;
        public E_Envelope envelope;
        public float _showDiameterGreaterThan;

        private void Awake()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            tree = new Tree(transform.position, branchRadius);

            IEnvelope envelope = SelectEnvelope();
            SpaceColonization environmentalInput = new SpaceColonization(tree, envelope, n_markers, occupancy_radius, perception_angle, perception_distance);
            IBranchingRules branchingRules = new RandomTreeBranchingRules();
            ResourceAllocation borchertHonda = new ResourceAllocation();
            treeModel = new TreeModelAlpha(tree, environmentalInput, branchingRules, borchertHonda, branchLength, branchRadius, GRAVITROPISM, PREFERRED_DIR);
        }

        // Start is called before the first frame update
        void Start()
        {

            //for (int i = 0; i < growthSteps; i++)
            //{
            //    treeModel.Grow();
            //}
            GrowCut();
            tree.changeEvent.Invoke();
        }

        private void GrowCut()
        {
            for (int i = 0; i < growthSteps; i++)
            {
                treeModel.Grow();
                if (i % cutEveryXSteps == 0 && cutTree && i <= stopCuttingAfterYear)
                {
                    CutMainBranch();
                }
            }
        }

        private void CutMainBranch()
        {
            // Starting from the base of the tree, go up x amount of depth.
            // If no such depth is available, cancel the cut.
            //
            List<Node> candidateNodes = new List<Node>();
            SelectBranchAtDepth(tree.baseNode, 0, Random.Range(lowerCutDepthBound, upperCutDepthBound), ref candidateNodes);

            if (candidateNodes.Count == 0)
                return;
            
            List<Node> orderedNodes = candidateNodes.OrderByDescending(node => node.diameter).ToList();
            treeModel.Cut(orderedNodes[0]);
        }

        private void SelectBranchAtDepth(Node node, int depth, int maxDepth, ref List<Node> candidateNodes) {
            if (depth == maxDepth)
            {
                if (node.cut == false)
                    candidateNodes.Add(node);
                
                return;
            }

            foreach (Node childNode in node.childNodes)
            {
                if (childNode.cut || childNode.terminal || childNode.childNodes.Count == 0)
                    continue;

                SelectBranchAtDepth(childNode, depth + 1, maxDepth, ref candidateNodes);
            }
        }

        IEnvelope SelectEnvelope()
        {
            switch(envelope)
            {
                default:
                    return new UnitSphereEnvelope();
                case E_Envelope.Sphere:
                    return new UnitSphereEnvelope();
                case E_Envelope.Hemisphere:
                    return new UnitHemisphereEnvelope();
                case E_Envelope.ClassicTree:
                    return new SphereTreeEnvelope();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                treeModel.Grow();
                tree.changeEvent.Invoke();
            }
        }
    }
}
