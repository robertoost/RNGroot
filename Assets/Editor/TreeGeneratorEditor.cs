using UnityEngine;
using UnityEditor;
using RNGroot;
using System.Collections.Generic;


[CustomEditor(typeof(TreeGenerator))]
public class TreeGeneratorEditor : Editor
{
    private List<int> ids = new List<int>();
    private Dictionary<int, Node> controlIDNodes = new Dictionary<int, Node>();
    bool showMarkers = false;
    int selectedControlID = -1;

    bool initialized = false;
    bool spaceCol = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TreeGenerator myScript = (TreeGenerator)target;
        TreeModelAlpha treeModel = myScript.treeModel;

        if (GUILayout.Button("Grow"))
        {
            treeModel.Grow();
            treeModel.tree.changeEvent.Invoke();
        }

        if (selectedControlID != -1)
        {
            if (GUILayout.Button("Cut!"))
            {
                treeModel.Cut(controlIDNodes[selectedControlID]);
                treeModel.tree.changeEvent.Invoke();
                selectedControlID = -1;
            }
        }

        EditorGUILayout.LabelField("Age: " + treeModel.treeMetrics.age.ToString());
        EditorGUILayout.LabelField("DBH: " + treeModel.treeMetrics.DBH.ToString());

        if (!EditorApplication.isPlaying)
            return;

        if (!initialized)
        {
            spaceCol = treeModel.environmentalInput is SpaceColonization;
        }
        if (spaceCol)
        {
            showMarkers = GUILayout.Toggle(showMarkers, " Show Markers");
        }
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
        
        Vector3 offset = ((MonoBehaviour)target).transform.position;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        controlIDNodes[controlID] = node;

        Handles.color = controlID == selectedControlID ? Color.yellow : Color.red;

        bool clicked = Handles.Button(node.position + offset, Quaternion.identity, 0.1f, 0.1f, Handles.SphereHandleCap);

        selectedControlID = clicked ? controlID : selectedControlID;

        foreach (Node child in node.childNodes)
        {
            Gizmos.color = Color.blue;
            Handles.DrawLine(node.position + offset, child.position + offset);
            DrawTree(child);
        }
    }

    void DrawMarkers(SpaceColonization spaceCol)
    {
        Handles.color = Color.yellow;
        foreach (int marker_id in spaceCol.unoccupied_marker_ids)
        {
           int controlID = GUIUtility.GetControlID(FocusType.Passive);
           Vector3 marker = spaceCol.markers[marker_id] + ((MonoBehaviour)target).transform.position;
           Handles.SphereHandleCap(controlID, marker, Quaternion.identity, 0.03f, EventType.Repaint);
        }

        Handles.color = Color.blue;
        foreach (int marker_id in spaceCol.marker_occupation.Keys)
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            Vector3 marker = spaceCol.markers[marker_id] + ((MonoBehaviour)target).transform.position;
            Handles.SphereHandleCap(controlID, marker, Quaternion.identity, 0.03f, EventType.Repaint);
        }
    }

    void Draw()
    {
        TreeGenerator treeGenerator = (TreeGenerator)target;
        TreeModelAlpha treeModel = treeGenerator.treeModel;
        DrawTree(treeGenerator.tree.baseNode);

        // If we're dealing with SpaceColonization: draw markers.
        //
        if (showMarkers && spaceCol)
        {
            DrawMarkers((SpaceColonization)treeModel.environmentalInput);
        }
        
    }
}

