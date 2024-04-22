using UnityEngine;
namespace RNGroot
{
    public class SphereTreeEnvelope : IEnvelope
    {
        public Vector3 pointInEnvelope {
            get {
                // For the foliage.
                //
                float sphereRadius = 1f;

                // For the trunk
                //
                float cylinderRadius = 0.1f;
                float cylinderHeight = 1f;

                // Solve for volume of sphere: ¾πr³
                // where r = radius.
                //
                float sphereVolume = (4 / 3) * Mathf.PI * Mathf.Pow(sphereRadius, 3);

                // Solve for volume of cylinder: πr²h
                // where r = radius and h = height.
                //
                float cylinderVolume = Mathf.PI * Mathf.Pow(cylinderRadius, 2) * cylinderHeight;

                float totalVolume = sphereVolume + cylinderVolume;

                // Uniformly distribute values between the trunk and foliage.
                //
                if (Random.value < sphereVolume / totalVolume)
                {
                    // Lift the foliage up by the trunk (cylinder) height.
                    //
                    Vector3 foliagePoint = Random.insideUnitSphere * sphereRadius;
                    foliagePoint += new Vector3(0, cylinderHeight + 1);
                    return foliagePoint;
                }
                else
                {
                    // Trunk is smaller than the foliage, so we scale by radius and height.
                    //
                    Vector3 cylinderPoint = RandomExtension.insideUnitCylinder();
                    cylinderPoint += new Vector3(0, 1, 0);
                    cylinderPoint.Scale(new Vector3(cylinderRadius, cylinderHeight/2, cylinderRadius));
                    return cylinderPoint;
                }
            }
        }
    }
}