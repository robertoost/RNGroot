//using RNGroot;
//using UnityEngine;

//public class CutRegrowthExperiment : MonoBehaviour
//{
//    public int n_Steps = 20;
//    public int n_RecoverySteps = 10;

//    public TreeModelAlpha tree1;
//    public TreeModelAlpha tree2;

//    private void Awake()
//    {
//        Random.InitState((int)System.DateTime.Now.Ticks);
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        tree1 = new TreeModelAlpha();

//        // TODO: Generate to N steps
//        GrowthSteps(n_Steps, tree1);

//        tree2 = new TreeModelAlpha(tree1);
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            Debug.Log("Grow!");
//            Random.State randomState = Random.state;

//            // Problem... the results are no longer deterministic. The random bud placement will be different.
//            // I bet even if we reset the random value in between these runs, it would still be different.
//            // Because the nodes are stored in a different order in the tree's lists now.
//            // Perhaps I could try anyway.
//            //
//            GrowthSteps(n_RecoverySteps, tree1);
//            Random.state = randomState;
//            GrowthSteps(n_RecoverySteps, tree2);
//        }
//    }

//    void GrowthSteps(int steps, TreeModelAlpha treeModel) {
//        for (int i = 0; i < steps; i++)
//        {
//            treeModel.Grow();
//        }
//        treeModel.tree.changeEvent.Invoke();
//    }
//}
