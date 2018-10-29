using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace polybius {
    public class StatusBar : MonoBehaviour {

        public TextMeshProUGUI username;

        private void Start() {
            Debug.Assert(username != null);
        }

        void Update() {
            if (username.text != PolybiusManager.player.getUsername())
                username.text = PolybiusManager.player.getUsername();
        }
    }
}