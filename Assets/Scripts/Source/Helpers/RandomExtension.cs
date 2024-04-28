using UnityEngine;

namespace RNGroot
{
    public static class RandomExtension
    {
        
        public static Vector3 insideUnitCylinder()
        {
            Vector3 randomInCircle = Random.insideUnitCircle;
            return new Vector3(randomInCircle.x, (2 * Random.value) - 1, randomInCircle.y);
        }
    }

}