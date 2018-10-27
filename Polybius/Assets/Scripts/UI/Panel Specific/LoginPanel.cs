using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class LoginPanel : MonoBehaviour {

        public GameObject username, password;
        private TextMeshProUGUI usernameText, passwordText;
        private Image usernameImage, passwordImage;

        void Start() {
            Debug.Assert(username != null && password != null);
            
            // TMP Input fields
            usernameText = username.GetComponent<TextMeshProUGUI>();
            passwordText = password.GetComponent<TextMeshProUGUI>();
            Debug.Assert(usernameText != null && passwordText != null);

            // Images
            usernameImage = username.GetComponent<Image>();
            passwordImage = password.GetComponent<Image>();
            Debug.Assert(usernameImage != null && passwordImage != null);
        }

        void Update() {
            // Username
            if (PolybiusManager.player.setUsername(usernameText.text)) {
                usernameImage.color = Color.white;
            } else {
                usernameImage.color = Color.red;
            }

            // Password
            if (PolybiusManager.player.setPassword(passwordText.text)) {
                passwordImage.color = Color.white;
            } else {
                passwordImage.color = Color.red;
            }
        }

        public void loginButton() {
            if (!string.IsNullOrEmpty(PolybiusManager.player.getUsername()) &&
                !string.IsNullOrEmpty(PolybiusManager.player.getPassword()))
                PolybiusManager.dm.login();
        }
    }
}