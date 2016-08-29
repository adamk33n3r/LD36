using UnityEngine;

namespace LD36.Scripts {
    public class MouseManager : MonoBehaviour {
        public GameObject boatPrefab;

        private GameObject shadowBoat;
        private GameObject tempBoat;
        private bool buildingBoat;
        private bool movingBoat;
        private ScriptableObjects.Boat currentBoatData;
        private ScriptableObjects.Net currentNetData;

        private void Start() {
            this.shadowBoat = Instantiate(this.boatPrefab);
            Destroy(this.shadowBoat.GetComponent<Boat>());
            Destroy(this.shadowBoat.GetComponentInChildren<Net>());
            this.shadowBoat.transform.Find("Canvas").gameObject.SetActive(false);
            this.shadowBoat.SetActive(false);
        }

        private void Update() {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 curPos = new Vector2(mouseWorldPos.x, 0);
            if (this.buildingBoat || this.movingBoat) {
                this.shadowBoat.transform.position = curPos;
                if (Input.GetMouseButtonDown(0)) {
                    if (this.buildingBoat) {
                        this.buildingBoat = false;
                        this.shadowBoat.SetActive(false);
                        Boat boat = ((GameObject) Instantiate(this.boatPrefab, curPos, Quaternion.identity)).GetComponent<Boat>();
                        boat.SetBoatType(this.currentBoatData);
                        boat.SetNetType(this.currentNetData);
                        LevelManager.Instance.RegisterBoat(boat);
                    } else if (this.movingBoat) {
                        this.movingBoat = false;
                        this.shadowBoat.SetActive(false);
                        this.tempBoat.transform.position = curPos;
//                        this.tempBoat.SetActive(true);
//                        this.tempBoat.GetComponent<Boat>().StartFishing();
                    }
                }
            }
        }

        public void BuildBoat(ScriptableObjects.Boat boatData, ScriptableObjects.Net netData) {
            this.buildingBoat = true;
            this.shadowBoat.SetActive(true);
            this.currentBoatData = boatData;
            this.currentNetData = netData;
        }

        public void MoveBoat(GameObject boat) {
            this.movingBoat = true;
            this.shadowBoat.SetActive(true);
            this.tempBoat = boat;
//            this.tempBoat.SetActive(false);
        }
    }
}
