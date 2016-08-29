using UnityEngine;

namespace LD36.ScriptableObjects {
    [CreateAssetMenu(fileName = "School", menuName = "LD36/School")]
    public class School : ScriptableObject {
        /// <summary>
        /// Fish in school
        /// </summary>
        public Fish fish;

        /// <summary>
        /// Count of fish
        /// </summary>
        public int count = 5;
    }
}