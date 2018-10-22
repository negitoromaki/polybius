using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace polybius {
    public class StatusBar : MonoBehaviour {

        private TextMeshProUGUI username;

        private void Start() {
            username = transform.Find("Username").GetComponent<TextMeshProUGUI>();
            Debug.Assert(username != null);
        }

        void Update() {
            if (username.text != PolybiusManager.player.username)
                username.text = PolybiusManager.player.username;
        }
    }
}