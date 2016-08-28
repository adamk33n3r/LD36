using LD36.ScriptableObjects;
using UnityEngine;

namespace LD36.Scripts {
    public class School : MonoBehaviour {
        public Fish Fish { get; protected set; }
        public int Count { get; protected set; }

        [SerializeField]
        private ScriptableObjects.School schoolData;

        private void Awake () {
            this.Fish = this.schoolData.fish;
            this.Count = this.schoolData.count;
        }

        public void TakeFish(int count) {
            this.Count -= count;
            if (this.Count <= 0) {
                Destroy(this.gameObject);
            }
        }
    }
}