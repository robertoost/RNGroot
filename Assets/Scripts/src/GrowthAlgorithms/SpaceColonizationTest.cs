using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RNGroot
{
    [ExecuteInEditMode]
    public class SpaceColonizationTest : GrowthAlgorithm
    {
        // All markers and a matching list of ids. The list of markers will stay the same, while ids will be removed.
        [HideInInspector]
        public List<Vector3> markers = new List<Vector3>();
        [HideInInspector]
        public List<int> marker_ids;

        // All markers associated with a given node.
        private Dictionary<Node, List<int>> node_markers = new Dictionary<Node, List<int>>();
        private Dictionary<int, Bud> marker_buds;

        // All markers associated with all closest buds in their perception cone.
        private Dictionary<Bud, List<int>> bud_markers;

        // Two hashsets for occupied markers, one for just occupied ones.
        private HashSet<int> just_occupied_markers;
        [HideInInspector]
        public HashSet<int> occupied_markers = new HashSet<int>();

        // Dict for Bud E values
        private Dictionary<Bud, float> bud_env_input = new Dictionary<Bud, float>();

        public int n_markers = 100;

        /// <summary>
        /// The radius a
        /// </summary>
        public float occupancy_radius = 2;
        public float perception_angle = 90;
        public float perception_distance = 4;

        // Start is called before the first frame update
        protected override void Start()
        {
            tree.AddBud(tree.baseNode, Vector3.up);
            for (int i = 0; i < n_markers; i++)
            {
                markers.Add(new Vector3(0, 6, 0) + (Random.insideUnitSphere * 5));
            }

            markers.Add(new Vector3(0, 1, 0));
            markers.Add(new Vector3(0, 2, 0));
            marker_ids = Enumerable.Range(0, markers.Count).ToList();

            just_occupied_markers = new HashSet<int>();

            // Occupy any markers that were already near the existing tree node.
            // Problem: If there's more than one, the occupied markers may not pick the closest marker.
            foreach (Node node in tree.nodes)
            {
                OccupyMarkers(node);
            }

            tree.cutEvent.AddListener(FreeSpace);
            base.Start();
        }


        // each tree node has a set of markers associated with it.

        public override void Grow()
        {
            // TODO: If there's no viable markers left, we should abort early...

            marker_buds = new Dictionary<int, Bud>();
            bud_markers = new Dictionary<Bud, List<int>>();
            just_occupied_markers = new HashSet<int>();
            bud_env_input = new Dictionary<Bud, float>();
            // Markers have been spawned. Now we grow towards it.

            // Determine what attraction points affect each node.

            foreach (int marker_id in marker_ids)
            {
                Vector3 marker = markers[marker_id];

                // Find closest node that sees me.
                //
                FindMarkerBud(marker, marker_id);
            }
            
            // Associate each bud with the set of markers that they can see and that they are the closest bud to.
            foreach ((int marker_id, Bud bud) in marker_buds) {
                List<int> perceived_markers;
                
                if (!bud_markers.TryGetValue(bud, out perceived_markers))
                {
                    perceived_markers = new List<int>();
                    bud_markers.Add(bud, perceived_markers);
                }

                perceived_markers.Add(marker_id);
            }

            // For each bud that has markers associated with them, grow.
            foreach((Bud bud, List<int> perceived_markers) in bud_markers)
            {
                // Total direction of all markers.
                Vector3 markerDirection = Vector3.zero;
                foreach(int marker_id in perceived_markers)
                {
                    if (just_occupied_markers.Contains(marker_id))
                        continue;

                    Vector3 marker = markers[marker_id];
                    markerDirection += (marker - bud.position).normalized;
                }

                // Bud doesn't have any unoccupied markers.
                //
                if (markerDirection == Vector3.zero)
                {
                    bud_env_input[bud] = 0f;
                }

                // TODO: Alter bud function to accept custom direction
                //
                bud.direction = markerDirection.normalized;
                bud_env_input[bud] = 1f;

                Node newNode = tree.AddNode(bud, branchLength, branchRadius);

                // Problem: the very first node isn't occupied yet.
                //
                OccupyMarkers(newNode);
            }

            tree.buds.Clear();

            // Add buds and occupy space?
            // TODO: Only occupy space when all new nodes are placed?
            OccupyMarkerIds();

            AddRandomBuds();
        }

        private void AddRandomBuds()
        {
            foreach (Node node in tree.nodes)
            {
                if (node.childBuds.Count == 0 && node.terminal == true)
                {

                    // Generate a random direction based on the node direction, and rotate by it.
                    //
                    Vector3 randomDir = new Vector3(Random.value, Random.value, Random.value);
                    randomDir = Vector3.Normalize(randomDir);
                    float angle = Random.Range(20, 35);
                    // Random positive or negative
                    int posNeg = Random.Range(0, 2) * 2 - 1;

                    Vector3 nodeDirection = node.parentNode != null ? node.Direction() : Vector3.up;
                    Vector3 rotationAxis = Vector3.Cross(randomDir, nodeDirection);

                    // Rotate by random axis and angle along node direction
                    Vector3 newDir = Quaternion.AngleAxis(angle, rotationAxis) * nodeDirection;
                    Vector3 newDirB = Quaternion.AngleAxis(-angle, rotationAxis) * nodeDirection;

                    // Randomly chooses to add a second lateral bud or not.

                    tree.AddBud(node, newDir);

                    if (Random.value > 0.3)
                    {
                        tree.AddBud(node, newDirB);
                    }

                    node.terminal = false;
                }
            }
        }

        // Should be called last to prevent id mismatches or errors.
        private void OccupyMarkerIds()
        {
            // Iterate backwards over the list of marker.
            //
            for (int i = marker_ids.Count - 1; i > 0; i--)
            {
                // 
                int marker_id = marker_ids[i];
                if (!just_occupied_markers.Contains(marker_id))
                    continue;

                marker_ids.RemoveAt(i);
            }
        }

        private void OccupyMarkers(Node node)
        {
            List<int> node_marker_list = new List<int>();
            node_markers.Add(node, node_marker_list);

            // For every marker placed, check if it was just occupied.
            //
            for (int i = 0; i < markers.Count; i++)
            {
                // Already occupied markers don't need to be checked
                // TODO: See if we need to/can make this faster.
                //
                if (just_occupied_markers.Contains(i))
                    continue;

                // If marker isn't close, it wasn't occupied
                //
                Vector3 marker = markers[i];
                if (Vector3.Distance(marker, node.position) > occupancy_radius)
                    continue;

                just_occupied_markers.Add(i);
                occupied_markers.Add(i);
                node_marker_list.Add(i);
            }
        }

        private void FreeSpace(Node node)
        {
            List<int> node_marker_ids = node_markers[node];
            foreach(int id in node_marker_ids)
            {
                occupied_markers.Remove(id);
                marker_ids.Add(id);
            }
        }

        private void FindMarkerBud(Vector3 marker, int marker_id) {

            float smallestDistance = float.MaxValue;

            // For now, query each bud.
            foreach (Bud bud in tree.buds)
            {
                float markerDistance = Vector3.Distance(bud.position, marker);

                // If not the closest or not close enough, continue.
                if (markerDistance > smallestDistance || markerDistance > perception_distance)
                    continue;

                // We've found a closer target, now we determine whether the bud is facing the marker.
                Vector3 budToMarkerDir = (marker - bud.position).normalized;
                float angle = Vector3.Angle(budToMarkerDir, bud.direction);

                // If the angle is not within perception radius, the bud is not facing the marker
                if (angle > (perception_angle / 2))
                    continue;

                smallestDistance = markerDistance;

                // This bud is the closest one that can see this marker. Add/Overwrite the markerBud.
                marker_buds[marker_id] = bud;
            }
        }
    }
}