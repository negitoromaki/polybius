using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public class UIMain : MonoBehaviour {
        public List<GameObject> UIPanels;
        public GameObject initialPanel;

        private GameObject activePanel;

        // always opens the initial panel so runtime will always look good
        // makes it so it doesn't matter what panel we have active while editing
        private void Start() {
            // TODO: Change logic so we take playerprefs and know when they logged in or not
            foreach (GameObject g in UIPanels) {
                g.SetActive(false);
            }

            initialPanel.SetActive(true);
            activePanel = initialPanel;
        }

        // method to switch panels
        public void ChangeMenu(GameObject panel) {
            activePanel.SetActive(false);
            panel.SetActive(true);

            activePanel = panel;
        }
    }
}
