using UnityEngine;

namespace LD36.Scripts {
    public class MainMenuManager : MonoBehaviour {
        private TextDisplay moneyDisplay;
        private TextDisplay boatDisplay;
        private TextDisplay netDisplay;

        private void Awake() {
            this.moneyDisplay = GameObject.Find("Money").GetComponent<TextDisplay>();
            this.boatDisplay = GameObject.Find("Boat").GetComponent<TextDisplay>();
            this.netDisplay = GameObject.Find("Net").GetComponent<TextDisplay>();
        }

        private void Start() {
            GameManager.Instance.UpdateTexts();
        }

        public void UpdateMoney(int money) {
            this.moneyDisplay.UpdateText(money);
        }

        public void UpdateBoatLevel(int level) {
            this.boatDisplay.UpdateText(level);
        }

        public void UpdateNetLevel(int level) {
            this.netDisplay.UpdateText(level);
        }
    }
}
