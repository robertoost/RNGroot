using UnityEngine;

namespace RNGroot
{
    public class UnitHemisphereEnvelope : IEnvelope
    {
        public Vector3 pointInEnvelope
        {
            get
            {
                Vector3 randomInSphere = Random.insideUnitSphere * 2;
                Vector3 randomInHemisphere = new Vector3(randomInSphere.x, Mathf.Abs(randomInSphere.y), randomInSphere.z);
                return randomInHemisphere;
            }
        }
    }
}