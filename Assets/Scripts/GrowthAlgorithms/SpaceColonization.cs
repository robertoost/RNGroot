using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{
    public class SpaceColonization : GrowthAlgorithm
    {
        private List<Vector3> markers = new List<Vector3>();
        private List<Vector3> occupied_markers = new List<Vector3>();
        public int n_markers = 100;


        public float occupancy_radius = 2;
        public float perception_angle = 90;
        public float perception_distance = 4;

        private Dictionary<int, Bud> markerBuds;
        private Dictionary<Bud, List<(int, Vector3)>> budMarkers;
        private HashSet<int> just_occupied_markers;

        // Start is called before the first frame update
        void Start()
        {
            tree.AddBud(tree.baseNode, Vector3.up);
            for (int i = 0; i < n_markers; i++)
            {
                markers.Add(new Vector3(0, 6, 0) + ((Random.onUnitSphere * 5) + Random.insideUnitSphere));
            }

            markers.Add(new Vector3(0, 1, 0));
            markers.Add(new Vector3(0, 2, 0));

            just_occupied_markers = new HashSet<int>();

            // Occupy any markers that were already near the existing tree node.
            // Problem: If there's more than one, the occupied markers may not pick the closest marker.
            foreach (Node node in tree.nodes)
            {
                OccupySpace(node);
            }
        }

        // Update is called once per frame

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
                return;
            Gizmos.color = Color.cyan;
            foreach (Vector3 marker in markers)
            {
                Gizmos.DrawSphere(marker, 0.05f);
            }

            Gizmos.color = Color.red;
            foreach (Vector3 marker in occupied_markers)
            {
                Gizmos.DrawSphere(marker, 0.05f);
            }
            Gizmos.color = new Color(1, 0.92f, 0.016f, 0.2f);

            foreach (Bud bud in tree.buds)
            {
                Gizmos.DrawWireSphere(bud.position, occupancy_radius);
            }
        }

        // each tree node has a set of markers associated with it.

        public override void Grow()
        {
            // TODO: If there's no viable markers left, we should abort early...

            markerBuds = new Dictionary<int, Bud>();
            budMarkers = new Dictionary<Bud, List<(int, Vector3)>>();
            just_occupied_markers = new HashSet<int>();
            // Markers have been spawned. Now we grow towards it.

            // Determine what attraction points affect each node.

            for (int i = 0; i < markers.Count; i++)
            {
                Vector3 marker = markers[i];

                // Find closest node that sees me.
                //
                FindMarkerBud(marker, i);
            }
            
            // Associate each bud with the set of markers that they can see and that they are the closest bud to.
            foreach ((int marker_id, Bud bud) in markerBuds) {
                List<(int, Vector3)> markerList;
                
                if (!budMarkers.TryGetValue(bud, out markerList))
                {
                    markerList = new List<(int, Vector3)>();
                    budMarkers.Add(bud, markerList);
                }
                
                markerList.Add((marker_id, markers[marker_id]));
            }

            // For each bud that has markers associated with them, grow.
            foreach((Bud bud, List<(int, Vector3)> markers) in budMarkers)
            {
                // Total direction of all markers.
                Vector3 markerDirection = Vector3.zero;
                foreach((int marker_id, Vector3 marker) in markers)
                {
                    if (just_occupied_markers.Contains(marker_id))
                        continue;
                    markerDirection += (marker - bud.position).normalized;
                }

                // Bud doesn't have any unoccupied markers... 
                if (markerDirection == Vector3.zero)
                    continue;

                // TODO: Alter bud function to accept custom direction...
                bud.direction = markerDirection.normalized;

                Node newNode = tree.AddNode(bud, branchLength, branchRadius);   

                // Problem: the very first node isn't occupied yet.
                OccupySpace(newNode);
            }
            tree.buds.Clear();
            // Add buds and occupy space?
            RemoveOccupiedMarkers();

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

        private void RemoveOccupiedMarkers()
        {
            for (int i = markers.Count - 1; i > 0; i--)
            {
                if (!just_occupied_markers.Contains(i))
                    continue;
                Vector3 marker = markers[i];

                // TODO: Associate nodes with occupied markers.
                occupied_markers.Add(marker);
                markers.RemoveAt(i);
            }
        }

        private void OccupySpace(Node node)
        {
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

                // This bud is the closest one that can see this marker. Add/Overwrite the markerBud.
                markerBuds[marker_id] = bud;
            }
        }
    }
}