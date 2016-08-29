using UnityEngine;

namespace LD36.ScriptableObjects {
    [CreateAssetMenu(fileName = "Boat", menuName = "LD36/Boat")]
    public class Boat : ScriptableObject {
        public float netDepth = 3f;
        public float maxWeight = 100f;
        public int cost = 100;
    }
}
