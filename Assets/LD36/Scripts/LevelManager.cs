using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD36.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace LD36.Scripts {
    public class LevelManager : MonoBehaviour {
        public static LevelManager Instance { get; protected set; }

        public GameObject sardineSchoolPrefab;
        public GameObject tunaSchoolPrefab;
        public GameObject salmonSchoolPrefab;
        public GameObject troutSchoolPrefab;
        public GameObject fishDisplay;
        public GameObject fishDisplayPrefab;
        public int schoolSpawnCount;
        public const int WORLD_WIDTH = 18;
        public const int WORLD_HEIGHT = 10;

        private float time;
        private bool ending;
        private List<Boat> boatsInPlay;

        private Dictionary<Species, TextDisplay> displayDict;
        private List<Fish> fishCaught;

        private TextDisplay moneyDisplay;

        private void Awake() {
            Instance = this;
            this.fishDisplay = GameObject.Find("Fish");
            this.displayDict = new Dictionary<Species, TextDisplay>();
            this.fishCaught = new List<Fish>();
            this.boatsInPlay = new List<Boat>();
            this.moneyDisplay = GameObject.Find("Money").GetComponent<TextDisplay>();
        }

        private void Start () {
            foreach (Species species in Enum.GetValues(typeof(Species))) {
                Debug.Log(species);
                GameObject go = Instantiate(this.fishDisplayPrefab);
                go.name = species.ToString();
                go.transform.SetParent(this.fishDisplay.transform);
                TextDisplay disp = go.GetComponent<TextDisplay>();
                disp.prefix = species.ToString();
                disp.UpdateText(0);
                this.displayDict.Add(species, disp);
            }
            // TODO: put different fish at different depth ranges. sardines up high. tuna low. (maybe tuna up high if by sardines)
//            for (int i = 0; i < this.schoolSpawnCount; i++) {
//                int x = Random.Range(-WORLD_WIDTH / 2 + 1, WORLD_WIDTH / 2 - 1);
//                int y = Random.Range(-WORLD_HEIGHT / 2, -1);
//                Instantiate(this.schoolPrefab, new Vector3(x, y, this.schoolPrefab.transform.position.z), Quaternion.identity);
//            }
            int x = 0;
            int y = 0;

            // Sardines
            y = -1;
            x = Random.Range(-WORLD_WIDTH / 2 + 1, WORLD_WIDTH / 2 - 1);
            Instantiate(this.sardineSchoolPrefab, new Vector3(x, y, this.sardineSchoolPrefab.transform.position.z), Quaternion.identity);
            x = Random.Range(-WORLD_WIDTH / 2 + 1, WORLD_WIDTH / 2 - 1);
            Instantiate(this.sardineSchoolPrefab, new Vector3(x, y, this.sardineSchoolPrefab.transform.position.z), Quaternion.identity);
            x = Random.Range(-WORLD_WIDTH / 2 + 1, WORLD_WIDTH / 2 - 1);
            Instantiate(this.sardineSchoolPrefab, new Vector3(x, y, this.sardineSchoolPrefab.transform.position.z), Quaternion.identity);
            
            // Trout
            y = -2;
            x = Random.Range(-WORLD_WIDTH / 2 + 1, WORLD_WIDTH / 2 - 1);
            Instantiate(this.troutSchoolPrefab, new Vector3(x, y, this.troutSchoolPrefab.transform.position.z), Quaternion.identity);
            x = Random.Range(-WORLD_WIDTH / 2 + 1, WORLD_WIDTH / 2 - 1);
            Instantiate(this.troutSchoolPrefab, new Vector3(x, y, this.troutSchoolPrefab.transform.position.z), Quaternion.identity);

            // Salmon
            y = -4;
            x = Random.Range(-WORLD_WIDTH / 2 + 1, WORLD_WIDTH / 2 - 1);
            Instantiate(this.salmonSchoolPrefab, new Vector3(x, y, this.salmonSchoolPrefab.transform.position.z), Quaternion.identity);
            x = Random.Range(-WORLD_WIDTH / 2 + 1, WORLD_WIDTH / 2 - 1);
            Instantiate(this.salmonSchoolPrefab, new Vector3(x, y, this.salmonSchoolPrefab.transform.position.z), Quaternion.identity);

            // Tuna
            y = -5;
            x = Random.Range(-WORLD_WIDTH / 2 + 1, WORLD_WIDTH / 2 - 1);
            Instantiate(this.tunaSchoolPrefab, new Vector3(x, y, this.tunaSchoolPrefab.transform.position.z), Quaternion.identity);
            x = Random.Range(-WORLD_WIDTH / 2 + 1, WORLD_WIDTH / 2 - 1);
            Instantiate(this.tunaSchoolPrefab, new Vector3(x, y, this.tunaSchoolPrefab.transform.position.z), Quaternion.identity);

            this.moneyDisplay.UpdateText(GameManager.Instance.Money);
        }

        private void Update () {
            this.time += Time.deltaTime;
            if (!this.ending && this.time > 5 * 60) {
                this.ending = true;
                StartCoroutine(WaitForBoatsToLeave());
            }
        }
        
        private IEnumerator WaitForBoatsToLeave() {
            Debug.Log("TELLING ALL BOATS TO LEAVE");
            foreach (Boat boat in this.boatsInPlay) {
                boat.Empty();
            }
            Debug.Log("WAITING FOR ALL BOATS TO LEAVE");
            while (this.boatsInPlay.Count > 0) {
                yield return null;
            }
            Debug.Log("Switching scenes");
            GameManager.Instance.LoadMainMenu();
        }

        public void BuyBoat() {
            if (this.ending) {
                return;
            }
            GameManager.Instance.BuyBoat();
        }

        public void RegisterBoat(Boat boat) {
            this.boatsInPlay.Add(boat);
            this.moneyDisplay.UpdateText(GameManager.Instance.Money);
            boat.RegisterOnLeaveCallback(() => {
                this.boatsInPlay.Remove(boat);
            });
        }

        public void AddFish(List<Fish> fish) {
            foreach (Fish fishy in fish) {
                this.displayDict[fishy.species].UpdateText(1, true);
                this.fishCaught.Add(fishy);
            }
            GameManager.Instance.AddMoney(fish.Sum((fishy) => fishy.price));
            this.moneyDisplay.UpdateText(GameManager.Instance.Money);
        }

        public void MoveBoat(GameObject boat) {
            GameManager.Instance.MoveBoat(boat);
        }
    }
}