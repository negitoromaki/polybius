using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public class UICanvas : MonoBehaviour {
        
        private GameObject MainMenuPanel;
        private GameObject ConnectionPanel;
        private bool justLoggedIn = true;

        private void Start() {
            ConnectionPanel = transform.Find("ConnectionPanel").gameObject;
            MainMenuPanel = transform.Find("MainMenuPanel").gameObject;
            Debug.Assert(ConnectionPanel != null && MainMenuPanel != null);
        }

        void Update() {
            // If loss of connection, switch to connecting panel
            if (!PolybiusManager.dm.connected && !ConnectionPanel.activeSelf)
                GetComponent<UIPanelSwitcher>().ChangeMenu(ConnectionPanel);

            // Switch the UI to the main menu panel when logged in
            // If logged out, reset
            if (!PolybiusManager.loggedIn)
                justLoggedIn = true;

            if (justLoggedIn && PolybiusManager.loggedIn) {
                GetComponent<UIPanelSwitcher>().ChangeMenu(MainMenuPanel);
                justLoggedIn = false;
            }
        }
    }
}