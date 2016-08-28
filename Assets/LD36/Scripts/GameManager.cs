using System;
using LD36.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LD36.Scripts {
    public class GameManager : MonoBehaviour {
        public GameObject boatPrefab;
        public GameObject schoolPrefab;
        public GameObject fishDisplay;
        public GameObject fishDisplayPrefab;
        public int schoolSpawnCount;
        public const int WORLD_WIDTH = 18;
        public const int WORLD_HEIGHT = 10;


        private void Start () {
            foreach (Species species in Enum.GetValues(typeof(Species))) {
                Debug.Log(species);
                GameObject go = Instantiate(this.fishDisplayPrefab);
                go.name = species.ToString();
                go.transform.SetParent(this.fishDisplay.transform);
                TextDisplay disp = go.GetComponent<TextDisplay>();
                disp.prefix = species.ToString();
                disp.UpdateText(0);
            }
            return;
            for (int i = 0; i < this.schoolSpawnCount; i++) {
                int x = Random.Range(-WORLD_WIDTH / 2 + 1, WORLD_WIDTH / 2 - 1);
                int y = Random.Range(-WORLD_HEIGHT / 2, -1);
                Instantiate(this.schoolPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }

        private void Update () {
        }

        public void SpawnBoat() {
            Instantiate(this.boatPrefab);
        }
    }
}