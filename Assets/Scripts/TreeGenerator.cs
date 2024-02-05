using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public class TreeGenerator : MonoBehaviour
    {

        public enum GrowthAlgorithmType
        {
            SpaceColonization = 0,
            //ShadowPropagation = 1
        }

        private GrowthAlgorithm growthAlgo;

        public GrowthAlgorithmType type;
        public int steps;

        // Create a new tree.
        //
        public Tree tree;
        
        // Start is called before the first frame update
        void Awake()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            tree = new Tree(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            switch (type)
            {
                case GrowthAlgorithmType.SpaceColonization:
                    growthAlgo = new SpaceColonization(tree);
                    break;
            }

            for (int i = 0; i < steps; i++) growthAlgo.Grow();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}