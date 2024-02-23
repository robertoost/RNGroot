using UnityEngine;
using System.Collections;
using UnityEditor;
using RNGroot;


[CustomEditor(typeof(SpaceColonization))]
public class GrowthAlgorithmEditor : Editor
{

    static int _currentHandleId = 0;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpaceColonization myScript = (SpaceColonization)target;
        if (GUILayout.Button("Grow"))
        {
            myScript.Grow();
            myScript.growthEvent.Invoke(myScript.tree);
        }
    }
    public void OnSceneGUI()
    {
        _currentHandleId = (EditorGUIUtility.hotControl != 0) ? EditorGUIUtility.hotControl : _currentHandleId;

        SpaceColonization spaceCol = (SpaceColonization)target;
        for (int control_id = 0; control_id < spaceCol.tree.nodes.Count; control_id++)
        {
            Node node = spaceCol.tree.nodes[control_id];
            // TODO: Allow selection of handles, and make an editor window addition which allows me to alter what parts get cut.
            //Vector3 handlePos = Handles.PositionHandle(node.position, Quaternion.identity);
            //if (handlePos == node.position)
            //    continue;
            //node.position = handlePos;
            //Debug.Log("Moved :)");
            Handles.color = _currentHandleId == control_id + 1 ? Color.yellow : Color.red;
            Handles.SphereHandleCap(control_id + 1, node.position, Quaternion.identity, 0.05f, EventType.Repaint);

            // Draw small sphere caps for each node.
            // Allow us to select these caps.

        }
    }


}

