using RNGroot;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements.Experimental;

namespace RNGroot
{
    public class SpaceColonization : IEnvironmentalInput
    {
        public int n_markers = 10000;
        public float scale = 4f;

        public float occupancy_radius = 0.6f;
        public float perception_angle = 90;
        public float perception_distance = 1;

        public IEnvelope envelope;

        private Tree tree;

        // All markers and a matching list of ids. The list of markers will stay the same, while ids will be removed.
        //
        public List<Vector3> markers = new List<Vector3>();
        public List<int> unoccupied_marker_ids = new List<int>();

        // All markers associated with a given node.
        //
        private Dictionary<Node, List<int>> node_markers = new Dictionary<Node, List<int>>();

        // All markers associated with all closest buds in their perception cone.
        //
        private Dictionary<int, Bud> marker_buds;
        private Dictionary<Bud, List<int>> bud_markers;

        // All bud environment and directional values
        //
        Dictionary<Bud, (float, Vector3)> E_values = new Dictionary<Bud, (float, Vector3)>();

        // Two hashsets for occupied markers, one for just occupied ones.
        //
        public Dictionary<int, int> marker_occupation = new Dictionary<int, int>();


        public SpaceColonization(Tree tree, IEnvelope envelope)
        {
            this.tree = tree;
            this.envelope = envelope;

            // Place initial markers.
            //
            PlaceMarkers();

            // Add the first nodes of the tree.
            AddNodes(tree.nodes);
        }

        public void PlaceMarkers()
        {
            for (int i = 0; i < n_markers; i++)
            {
                Vector3 point_in_envelope = envelope.pointInEnvelope;

                // TODO: Do we keep scale like this?
                //
                point_in_envelope.Scale(new Vector3(scale, scale, scale));

                markers.Add(point_in_envelope);
            }

            unoccupied_marker_ids = Enumerable.Range(0, markers.Count).ToList();
        }

        //public float CalculateE(Bud bud)
        //{
        //    List<Vector3> perceived_markers = new List<Vector3>();
        //    Vector3 marker_dir = Vector3.zero;

        //    foreach (int marker_id in unoccupied_marker_ids)
        //    {
        //        Vector3 marker = markers[marker_id];

        //        float markerDistance = Vector3.Distance(bud.position, marker);
        //        if (markerDistance > perception_distance)
        //            continue;

        //        // now we determine whether the bud is facing the marker.
        //        Vector3 budToMarkerDir = (marker - bud.position).normalized;
        //        float angle = Vector3.Angle(budToMarkerDir, bud.direction);

        //        // If the angle is not within perception radius, the bud is not facing the marker
        //        if (angle > (perception_angle / 2))
        //            continue;

        //        // Bud can see this marker.
        //        perceived_markers.Add(marker);
        //        marker_dir += (marker - bud.position).normalized;
        //    }

        //    if (perceived_markers.Count == 0)
        //    {
        //        return 0;
        //    } else
        //    {
        //        bud.direction = perceived_markers.Count == 0 ? bud.direction : marker_dir.normalized;
        //        return 1f;
        //    }
        //}

        public void AddNodes(List<Node> addedNodes)
        {
            // For each new node, update marker occupation.
            //
            foreach (Node node in addedNodes)
            {
                // List of markers associated with this node.
                //
                List<int> node_marker_list = new List<int>();

                for (int i = unoccupied_marker_ids.Count - 1; i > -1; i--)
                {
                    int marker_id = unoccupied_marker_ids[i];
                    Vector3 marker = markers[marker_id];

                    // Check whether the marker is within the occupancy radius of this node.
                    //
                    float markerDistance = Vector3.Distance(node.position, marker);
                    if (markerDistance < occupancy_radius)
                    {
                        node_marker_list.Add(marker_id);

                        // Add 1 to the occupation count for this marker.
                        //
                        if (marker_occupation.TryGetValue(marker_id, out int occupation_count)) {
                            marker_occupation[marker_id] = occupation_count + 1;
                        }
                        else
                        {
                            marker_occupation[marker_id] = 1;
                            unoccupied_marker_ids.RemoveAt(i);
                        }
                    }
                }
                node_markers[node] = node_marker_list;
            }
        }

        public void RemoveNodes(List<Node> cutNodes)
        {
            // For each cut node, free their occupied markers.
            //
            foreach (Node node in cutNodes)
            {
                List<int> node_marker_list = node_markers[node];

                // Decrement marker occupancy.
                //
                foreach(int marker_id in node_marker_list)
                {
                    marker_occupation[marker_id] -= 1;

                    // If all nodes are removed, free the marker.
                    //
                    if (marker_occupation[marker_id] == 0)
                    {
                        unoccupied_marker_ids.Add(marker_id);
                        marker_occupation.Remove(marker_id);
                    }
                }

                node_markers.Remove(node);
            }

        }

        public Dictionary<Bud, (float, Vector3)> CalculateBudInformation()
        {
            Dictionary<int, Bud> marker_buds = new Dictionary<int, Bud>();
            Dictionary<Bud, List<int>> bud_markers = new Dictionary<Bud, List<int>>();


            foreach (int marker_id in unoccupied_marker_ids)
            {
                Vector3 marker = markers[marker_id];

                // Find closest node that sees me.
                //
                FindMarkerBud(marker, marker_id, marker_buds);
            }

            // Associate each bud with the set of markers that they can see and that they are the closest bud to.
            //
            foreach ((int marker_id, Bud bud) in marker_buds)
            {
                List<int> perceived_markers;

                if (!bud_markers.TryGetValue(bud, out perceived_markers))
                {
                    perceived_markers = new List<int>();
                    bud_markers.Add(bud, perceived_markers);
                }

                perceived_markers.Add(marker_id);
            }

            // Standard E values.
            //
            foreach (Bud bud in tree.buds)
            {
                E_values[bud] = (0, bud.direction);
            }

            // Calculate direction and determine E value for all buds with perceived markers.
            //
            foreach ((Bud bud, List<int> perceived_markers) in bud_markers)
            {
                float E = 1;
                Vector3 marker_dir = new Vector3();
                foreach (int marker_id in perceived_markers)
                {
                    Vector3 marker = markers[marker_id];
                    marker_dir += (marker - bud.position).normalized;
                }
                marker_dir = marker_dir.normalized;
                

                // Add E and direction to E_values.
                E_values[bud] = (E, marker_dir);
            }



            return E_values;
        }

        private void FindMarkerBud(Vector3 marker, int marker_id, Dictionary<int, Bud> marker_buds)
        {

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
