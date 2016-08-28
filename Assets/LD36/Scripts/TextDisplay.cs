using UnityEngine;
using UnityEngine.UI;

namespace LD36.Scripts {
    public class TextDisplay : MonoBehaviour {
        public string prefix;

        private Text textUI;
        private int prevValue;

        private void Awake() {
            this.textUI = GetComponent<Text>();
        }

        public void UpdateText(string text) {
            this.textUI.text = string.Format("{0}: {1}", this.prefix, text);
        }

        public void UpdateText(int num, bool add = false) {
            this.prevValue = add ? this.prevValue + num : num;
            this.textUI.text = string.Format("{0}: {1}", this.prefix, this.prevValue);
        }
    }
}