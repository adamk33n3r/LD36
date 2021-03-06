using UnityEngine;

namespace LD36.ScriptableObjects {
    [CreateAssetMenu(fileName = "Net", menuName = "LD36/Net")]
    public class Net: ScriptableObject {
        public int holeSize = 5;
        public int weightCapacity = 50;
    }
}