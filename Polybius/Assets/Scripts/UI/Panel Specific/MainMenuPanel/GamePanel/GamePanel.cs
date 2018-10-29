using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public class GamePanel : MonoBehaviour {

        public GameObject gameTypePanel, LobbyPanel;

        private LobbyPanel lobbyPanelScript;
        private UIPanelSwitcher switcher;
        private Game.type currGameType = Game.type.none;

        void Start() {
            Debug.Assert(gameTypePanel != null && LobbyPanel != null);
            lobbyPanelScript = LobbyPanel.GetComponent<LobbyPanel>();
            switcher = GetComponent<UIPanelSwitcher>();
            Debug.Assert(switcher != null);
        }

        void Update() {
            // Menu switching
            if (currGameType == Game.type.none) {
                if (!gameTypePanel.activeSelf) {
                    switcher.ChangeMenu(gameTypePanel);
                    Debug.Log("Changing to gameTypePanel");
                }
            } else {
                if (!LobbyPanel.activeSelf) {
                    lobbyPanelScript.setGameType(currGameType);
                    switcher.ChangeMenu(LobbyPanel);
                    Debug.Log("Changing to LobbyPanel");
                }
            }
        }

        // for UI buttons
        public void setGameType(string s) {
            if (s == "none") {
                currGameType = Game.type.none;
            } else if (s == "pong") {
                currGameType = Game.type.pong;
            } else if (s == "connect4") {
                currGameType = Game.type.connect4;
            } else if (s == "tictactoe") {
                currGameType = Game.type.tictactoe;
            } else {
                Debug.LogError("Gametype not found: " + s);
            }
        }
    }
}