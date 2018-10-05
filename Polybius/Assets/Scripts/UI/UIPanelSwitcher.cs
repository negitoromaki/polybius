using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public class UIPanelSwitcher : MonoBehaviour {
        public GameObject initialPanel;
        public bool setInactive;
        public List<GameObject> UIPanels;

        private GameObject activePanel;

        private void Start() {
            // TODO: Change logic so we take playerprefs and know when they logged in or not
            if (setInactive) 
                foreach (GameObject g in UIPanels)
                    g.SetActive(false);

            Debug.Assert(initialPanel != null);
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
