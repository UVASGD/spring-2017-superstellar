using UnityEngine;
using UnityEngine.UI;

namespace UnityPhoton {
    public class InGameLogDisplay : MonoBehaviour {

        [SerializeField]
        private Text logText;

        private void Start() {
            logText.text = "";
        }

        public void Print(string message) {
            logText.text += message;
        }

        public void PrintLine(string message) {
            Print("\n" + message);
        }
    }
}