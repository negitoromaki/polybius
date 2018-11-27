using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public class GamePanel : MonoBehaviour {

        public GameObject gameTypePanel, LobbyPanel;

        private LobbyPanel lobbyPanelScript;
        private UIPanelSwitcher switcher;
        public string currGameType;

        void Start() {
            Debug.Assert(gameTypePanel != null && LobbyPanel != null);
            lobbyPanelScript = LobbyPanel.GetComponent<LobbyPanel>();
            switcher = GetComponent<UIPanelSwitcher>();
            Debug.Assert(switcher != null);
            currGameType = "none";
        }

        void Update() {
            // Menu switching
            if (currGameType == "none") {
                if (!gameTypePanel.activeSelf) {
                    switcher.ChangeMenu(gameTypePanel);
                    Debug.Log("Changing to gameTypePanel");
                }
            } else {
                if (!LobbyPanel.activeSelf) {
                    switcher.ChangeMenu(LobbyPanel);
                    Debug.Log("Changing to LobbyPanel");
                }
            }
        }

        // for UI buttons
        public void setGameType(string s) {
            currGameType = s;
        }
    }
}