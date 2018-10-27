using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public class UICanvas : MonoBehaviour {
        public GameObject ConnectionPanel, MainMenuPanel, LoginRegisterPanel, LoginPanel, RegisterPanel;

        private void Start() {
            Debug.Assert(   ConnectionPanel != null &&
                            MainMenuPanel != null &&
                            LoginRegisterPanel != null &&
                            LoginPanel != null &&
                            RegisterPanel != null);
        }

        void Update() {
            if (PolybiusManager.dm.connected) {
                if (PolybiusManager.loggedIn) {
                    GetComponent<UIPanelSwitcher>().ChangeMenu(ConnectionPanel);
                } else {
                    if (!LoginRegisterPanel.activeSelf && !RegisterPanel.activeSelf && !LoginPanel.activeSelf)
                        GetComponent<UIPanelSwitcher>().ChangeMenu(LoginRegisterPanel);
                }
            } else {
                if (!ConnectionPanel.activeSelf)
                    GetComponent<UIPanelSwitcher>().ChangeMenu(ConnectionPanel);
            }
        }
    }
}