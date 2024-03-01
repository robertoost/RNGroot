using UnityEngine;
using System.Collections;
using UnityEditor;
using RNGroot;
using NUnit.Framework;
using System.Collections.Generic;


[CustomEditor(typeof(SpaceColonization))]
public class GrowthAlgorithmEditor : Editor
{
    private List<int> ids = new List<int>();

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
        SpaceColonization spaceCol = (SpaceColonization)target;

        // TODO: Get this working whenever.
        if (!EditorApplication.isPlaying)
            return;

        //switch(Event.current.type)
        //{
        //    case EventType.Layout:
        //        LayoutGUI(spaceCol);
        //        break;
        //    case EventType.Repaint:
        //        RepaintGUI(spaceCol);
        //        break;
        //}
    }
    //void LayoutGUI(SpaceColonization spaceCol)
    //{
    //    ids.Clear();
    //    for (int control_index = 0; control_index < spaceCol.tree.nodes.Count; control_index++) {
    //        Node node = spaceCol.tree.nodes[control_index];

    //        int controlID = GUIUtility.GetControlID(FocusType.Passive);
    //        ids.Add(controlID);
    //        CreateHandleCap(controlID, node.position, EventType.Layout);
    //    }
    //}
    //void RepaintGUI(SpaceColonization spaceCol)
    //{
    //    int firstID = -1;
    //    for (int control_index = 0; control_index < spaceCol.tree.nodes.Count; control_index++)
    //    {
    //        Node node = spaceCol.tree.nodes[control_index];

    //        int controlID = GUIUtility.GetControlID(FocusType.Passive);
    //        if (control_index == 0)
    //            firstID = controlID;


    //        Handles.color = HandleUtility.nearestControl == controlID ? Color.yellow : Color.red;
    //        CreateHandleCap(controlID, node.position, EventType.Repaint);
    //    }
    //    Debug.Log($"First control: {ids[0]} and {firstID}");
    //    Debug.Log($"Hot control: {GUIUtility.hotControl} and NearestControl: {HandleUtility.nearestControl}");
    //}

    void CreateHandleCap(int id, Vector3 pos, EventType eventType)
    {
        Handles.SphereHandleCap(id, pos, Quaternion.identity, 0.05f, eventType);
    }
}

