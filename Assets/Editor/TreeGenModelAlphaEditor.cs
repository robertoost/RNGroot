using UnityEngine;
using UnityEditor;
using RNGroot;
using System.Collections.Generic;


[CustomEditor(typeof(TreeGenModelAlpha))]
public class TreeGenModelAlphaEditor : Editor
{
    private List<int> ids = new List<int>();
    private Dictionary<int, Node> controlIDNodes = new Dictionary<int, Node>();
    bool showMarkers = false;
    int selectedControlID = -1;

    public override void OnInspectorGUI()
    {
        showMarkers = GUILayout.Toggle(showMarkers, " Show Markers");
        
        DrawDefaultInspector();

        TreeGenModelAlpha myScript = (TreeGenModelAlpha)target;
        if (GUILayout.Button("Grow"))
        {
            myScript.Grow();
            myScript.tree.changeEvent.Invoke();
        }

        if (selectedControlID != -1)
        {
            if (GUILayout.Button("Cut!"))
            {
                controlIDNodes[selectedControlID].Cut();
                myScript.tree.changeEvent.Invoke();
                selectedControlID = -1;
            }
        }

        EditorGUILayout.LabelField("DBH: " + myScript.treeMetrics.DBH.ToString());
    }


    public void OnSceneGUI()
    {
        // TODO: Get this working whenever.
        if (!EditorApplication.isPlaying)
            return;
        Draw();
    }

    void DrawTree(Node node)
    {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        controlIDNodes[controlID] = node;

        Handles.color = controlID == selectedControlID ? Color.yellow : Color.red;

        bool clicked = Handles.Button(node.position, Quaternion.identity, 0.1f, 0.1f, Handles.SphereHandleCap);

        selectedControlID = clicked ? controlID : selectedControlID;

        foreach (Node child in node.childNodes)
        {
            Gizmos.color = Color.blue;
            Handles.DrawLine(node.position, child.position);
            DrawTree(child);
        }
    }

    void DrawMarkers(SpaceColonization  spaceCol)
    {
        Handles.color = Color.green;
        foreach (int marker_id in spaceCol.unoccupied_marker_ids)
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            Vector3 marker = spaceCol.markers[marker_id];
            Handles.SphereHandleCap(controlID, marker, Quaternion.identity, 0.03f, EventType.Repaint);
        }

        Gizmos.color = Color.grey;
        foreach (int marker_id in spaceCol.marker_occupation.Keys)
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            Vector3 marker = spaceCol.markers[marker_id];
            Handles.SphereHandleCap(controlID, marker, Quaternion.identity, 0.03f, EventType.Repaint);
        }
    }

    void Draw()
    {
        TreeGenModelAlpha treeGenModel = (TreeGenModelAlpha)target;
        DrawTree(treeGenModel.tree.baseNode);


        if (showMarkers && treeGenModel.environmentalInput is SpaceColonization)
        {
            DrawMarkers((SpaceColonization)treeGenModel.environmentalInput);
        }
        
    }
}

