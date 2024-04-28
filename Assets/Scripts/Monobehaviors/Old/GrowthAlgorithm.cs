using UnityEngine;
using UnityEngine.Events;

namespace RNGroot
{
    public abstract class GrowthAlgorithm : MonoBehaviour
    {
        /// <summary>
        /// Grows the tree structure by one timestep.
        /// </summary>
        public abstract void Grow();
        public int steps = 10;
        public float branchLength = 1f;
        public float branchRadius = 0.2f;
        public Tree tree { get; set; }

        void Awake()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            tree = new Tree(new Vector3(0, 0, 0), branchLength, branchRadius);
        }

        protected virtual void Start()
        {
            for (int i = 0; i < steps; i++)
            {
                Grow();
            }
            tree.changeEvent.Invoke();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Grow!");
                Grow();
                tree.changeEvent.Invoke();
            }
        }
    }
}