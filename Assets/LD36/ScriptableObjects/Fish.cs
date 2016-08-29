using UnityEngine;

namespace LD36.ScriptableObjects {
    // TODO: make species a scriptable object
    public enum Species {
        Sardine,
        Salmon,
        Trout,
        Tuna
    }

    [CreateAssetMenu(fileName = "Fish", menuName = "LD36/Fish")]
    public class Fish : ScriptableObject {
        /// <summary>
        /// Species of fish
        /// </summary>
        public Species species;

        /// <summary>
        /// Size of fish.
        /// </summary>
        public int size;

        /// <summary>
        /// Weight of fish.
        /// </summary>
        public int weight;

        /// <summary>
        /// Price of fish.
        /// </summary>
        public int price;
    }
}