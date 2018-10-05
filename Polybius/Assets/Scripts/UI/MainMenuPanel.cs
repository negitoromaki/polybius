using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class MainMenuPanel : MonoBehaviour {
        // This will be replaced later
        public TextMeshProUGUI InfoPanelUsername, ProfilePanelUsername;

        void Start() {
            InfoPanelUsername.text = ProfilePanelUsername.text = PolybiusManager.player.username;
        }
    }
}