using RNGroot;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GizmoTreeRenderer : MonoBehaviour
{

    private GrowthAlgorithm treeGenerator;

    // Start is called before the first frame update
    void Start()
    {
        treeGenerator = GetComponent<GrowthAlgorithm>(); 
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.yellow;
        DrawTreeGizmos(treeGenerator.tree.baseNode);
    }

    private void DrawTreeGizmos(Node node)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(node.position, 0.1f);

        foreach (Node child in node.childNodes)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(node.position, child.position);
            DrawTreeGizmos(child);
        }


        foreach (Bud childBud in node.childBuds)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(new Ray(childBud.position, childBud.direction));
        }
    }


}
