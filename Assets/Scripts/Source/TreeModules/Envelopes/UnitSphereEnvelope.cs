using UnityEngine;

namespace RNGroot
{
    public class UnitSphereEnvelope : IEnvelope
    {
        public Vector3 pointInEnvelope {
            get
            {
                float scale = 2f;
                return scale * Random.insideUnitSphere + new Vector3(0, scale, 0);
            }
        }
    }
}