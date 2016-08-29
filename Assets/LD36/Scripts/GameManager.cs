using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LD36.Scripts {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; protected set; }
        public MouseManager mouseManager;
        public ScriptableObjects.Boat[] boatTypes;
        public ScriptableObjects.Net[] netTypes;
        private int currentBoatType;
        private int currentNetType;
        public int Money { get; protected set; }
        private MainMenuManager menuManager;

        private void Awake() {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
            Debug.Log("GAME MANAGER UPDATE");
        }

        private void OnLevelWasLoaded() {
            Debug.Log("ASDFASDFASDFSADFASD");
            this.mouseManager = FindObjectOfType<MouseManager>();
            Debug.Log(this.mouseManager);
            this.menuManager = FindObjectOfType<MainMenuManager>();
            GameObject bb = GameObject.Find("BoatButton");
            if (bb != null) {
                bb.GetComponent<Button>().onClick.AddListener(UpgradeBoat);
            }
            bb = GameObject.Find("NetButton");
            if (bb != null) {
                bb.GetComponent<Button>().onClick.AddListener(UpgradeNet);
            }
            bb = GameObject.Find("GoFishing");
            if (bb != null) {
                bb.GetComponent<Button>().onClick.AddListener(GoFishing);
            }
        }

        private void Start() {
            this.currentBoatType = 0;
            this.currentNetType = 0;
            this.Money = 100;
            Debug.Log("GAME MANAGER START");
            SceneManager.LoadScene("Main");
        }

        private void Update() {
        
        }

        public void AddMoney(int money) {
            this.Money += money;
            Debug.Log("have this money: " + this.Money);
        }

        public void BuyBoat() {
            ScriptableObjects.Boat boatType = this.boatTypes[this.currentBoatType];
            if (this.Money < boatType.cost) {
                return;
            }
            this.Money -= boatType.cost;
            this.mouseManager.BuildBoat(boatType, this.netTypes[this.currentNetType]);
        }

        public void MoveBoat(GameObject boat) {
            this.mouseManager.MoveBoat(boat);
        }

        public void UpgradeBoat() {
            if (this.Money < 500) {
                return;
            }
            this.Money -= 500;
            this.currentBoatType++;
            this.menuManager.UpdateMoney(this.Money);
            this.menuManager.UpdateBoatLevel(this.currentBoatType + 1);
            if (this.currentBoatType == this.boatTypes.Length - 1) {
                GameObject.Find("BoatButton").GetComponent<Button>().enabled = false;
            }
        }

        public void UpgradeNet() {
            if (this.Money < 250) {
                return;
            }
            this.Money -= 250;
            this.currentNetType++;
            this.menuManager.UpdateMoney(this.Money);
            this.menuManager.UpdateNetLevel(this.currentNetType + 1);
            if (this.currentNetType == this.netTypes.Length - 1) {
                GameObject.Find("NetButton").GetComponent<Button>().enabled = false;
            }
        }

        public void GoFishing() {
            SceneManager.LoadScene("Level");
        }

        public void LoadMainMenu() {
            SceneManager.LoadScene("Main");
        }

        public void UpdateTexts() {
            // TODO: ew
            Debug.Log("updatetexts was called!!!!!!!!");
            Debug.Log(this.Money);
            this.menuManager.UpdateMoney(this.Money);
        }

    }
}
