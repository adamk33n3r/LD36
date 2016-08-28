using UnityEngine;

namespace LD36.ScriptableObjects {
    // TODO: make species a scriptable object
    public enum Species {
        Sardine,
        Salmon,
        Trout,
        Tuna
    }

    [CreateAssetMenu(fileName = "Fish", menuName = "LD36/Fish", order = 1)]
    public class Fish : ScriptableObject {
        /// <summary>
        /// Species of fish
        /// </summary>
        public Species species;

        /// <summary>
        /// Size of fish.
        /// </summary>
        public int size;

        public static float GetWeightOfSpecies(Species species) {
            switch (species) {
                case Species.Salmon:
                    return 10;
                case Species.Sardine:
                    return 1;
                case Species.Trout:
                    return 3;
                case Species.Tuna:
                    return 20;
                default:
                    // Shouldn't happen so this will make it obvious
                    return 1000;
            }
        }

    }
}