using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace polybius {
    public class ConnectionPanel : MonoBehaviour {

        public string text;
        public TextMeshProUGUI loadingText;
        public GameObject redirect; // Menu to redirect to

        private int counter;

        private void Start() {
            counter = 0;
            loadingText.text = text;
        }

        void Update() {
            if (PolybiusManager.dm.connected) {
                GetComponentInParent<UIPanelSwitcher>().ChangeMenu(redirect);
            } else {
                if (Time.frameCount % 50 == 0) {
                    if (++counter >= 4) {
                        PolybiusManager.dm.getConnection();
                        counter = 0;
                        loadingText.text = text;
                    } else {
                        // Add period
                        loadingText.text = loadingText.text + ".";
                    }
                }
            }
        }
    }
}
