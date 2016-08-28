using UnityEngine;

namespace LD36.Scripts {
    public class MouseManager : MonoBehaviour {
        public GameObject boatPrefab;

        private GameObject shadowBoat;
        private bool buildingBoat;

        private void Start() {
            this.shadowBoat = Instantiate(this.boatPrefab);
            Destroy(this.shadowBoat.GetComponent<Boat>());
            this.shadowBoat.SetActive(false);
        }

        private void Update() {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 curPos = new Vector2(mouseWorldPos.x, 0);
            if (this.buildingBoat) {
                this.shadowBoat.transform.position = curPos;
                if (Input.GetMouseButtonDown(0)) {
                    this.buildingBoat = false;
                    this.shadowBoat.SetActive(false);
                    Instantiate(this.boatPrefab, curPos, Quaternion.identity);
                }
            }
        }

        public void BuildBoat() {
            this.buildingBoat = true;
            this.shadowBoat.SetActive(true);
        }
    }
}
