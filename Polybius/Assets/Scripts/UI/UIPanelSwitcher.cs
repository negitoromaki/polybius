using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public class UIPanelSwitcher : MonoBehaviour {
        public GameObject initialPanel;
        public bool setInactive;

        private GameObject activePanel;

        private void Start() {
            // Set all children
            if (setInactive) {
                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).gameObject.SetActive(false);
            }

            // Activate
            Debug.Assert(initialPanel != null);
            ChangeMenu(initialPanel);
        }

        // method to switch panels
        public void ChangeMenu(GameObject panel) {
            if (activePanel)
                activePanel.SetActive(false);
            panel.SetActive(true);

            activePanel = panel;
        }
    }
}