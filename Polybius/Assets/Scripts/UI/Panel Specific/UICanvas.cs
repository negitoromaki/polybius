﻿using System.Collections;
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

        public void Update() {
            if (PolybiusManager.dm != null) {
                if (PolybiusManager.loggedIn) {
                    if (!MainMenuPanel.activeSelf)
                        GetComponent<UIPanelSwitcher>().ChangeMenu(MainMenuPanel);
                } else {
                    if (!LoginRegisterPanel.activeSelf && !RegisterPanel.activeSelf && !LoginPanel.activeSelf)
                        GetComponent<UIPanelSwitcher>().ChangeMenu(LoginRegisterPanel);
                }
            }
        }
    }
}