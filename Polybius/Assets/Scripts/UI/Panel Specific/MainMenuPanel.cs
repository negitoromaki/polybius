using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class MainMenuPanel : MonoBehaviour {
        // This will be replaced later
        public TextMeshProUGUI InfoPanelUsername, ProfilePanelUsername, usernameStat, dateOfBirthStat;

        void Update() {
            if (InfoPanelUsername.text      != PolybiusManager.player.username ||
                ProfilePanelUsername.text   != PolybiusManager.player.username ||
                usernameStat.text != PolybiusManager.player.username)

                InfoPanelUsername.text = usernameStat.text = ProfilePanelUsername.text = PolybiusManager.player.username;

            if (dateOfBirthStat.text != PolybiusManager.player.dob)
                dateOfBirthStat.text = PolybiusManager.player.dob;
        }
    }
}