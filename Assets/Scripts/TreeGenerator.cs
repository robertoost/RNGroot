using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public class TreeGenerator : MonoBehaviour
    {

        public enum GrowthAlgorithmType
        {
            RandomTree = 0,
            SpaceColonization = 1,
            //ShadowPropagation = 2
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
            tree = new Tree(new Vector3(0, 0, 0), Vector3.up);
            switch (type)
            {
                case GrowthAlgorithmType.RandomTree:
                    growthAlgo = new RandomTree(tree);
                    break;
                case GrowthAlgorithmType.SpaceColonization:
                    throw new System.NotImplementedException("I didn't make this yet.");
            }

            for (int i = 0; i < steps; i++) growthAlgo.Grow();
        }

        // Update is called once per frame

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Grow!");
                growthAlgo.Grow();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (type == GrowthAlgorithmType.SpaceColonization)
            {
                if (((RandomTree)growthAlgo) == null)
                {
                    return;
                }

                Gizmos.color = Color.blue;
                foreach (Vector3 marker in ((RandomTree)growthAlgo).markers)
                {
                    Gizmos.DrawSphere(marker, 0.1f);
                }

                Gizmos.color = Color.red;
                foreach (Vector3 marker in ((RandomTree)growthAlgo).occupied_markers)
                {
                    Gizmos.DrawSphere(marker, 0.1f);
                }
            }
        }
    }
}