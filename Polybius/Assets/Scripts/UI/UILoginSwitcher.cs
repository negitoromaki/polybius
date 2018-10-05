using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public class UILoginSwitcher : MonoBehaviour {
        // This class switches the UI to the main menu panel when logged in

        public GameObject MainMenuPanel;

        // This prevents it from activating all the time
        private bool justLoggedIn = true;
        private UIPanelSwitcher switcher;

        private void Start() {
            switcher = this.GetComponent<UIPanelSwitcher>();
            Debug.Assert(switcher != null);
        }

        void Update() {
            // If logged out, reset
            if (!PolybiusManager.loggedIn)
                justLoggedIn = true;

            if (justLoggedIn && PolybiusManager.loggedIn) {
                switcher.ChangeMenu(MainMenuPanel);
                justLoggedIn = false;
            }
        }
    }
}
