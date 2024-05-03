using System.Collections.Generic;
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
        public int growthSteps = 10;

        public Tree tree;
        public float GRAVITROPISM = 0;
        public float PREFERRED_DIR = 0;
        public float branchLength = 0.45f;
        public float branchRadius = 0.05f;
        public int n_markers = 1000;
        public float occupancy_radius = 0.6f;
        public float perception_angle = 90;
        public float perception_distance = 1;
        public E_Envelope envelope;
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

            for (int i = 0; i < growthSteps; i++)
            {
                treeModel.Grow();
            }
            tree.changeEvent.Invoke();
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
