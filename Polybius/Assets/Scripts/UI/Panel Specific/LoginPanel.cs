using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class LoginPanel : MonoBehaviour {

        public TMP_InputField usernameText, passwordText;
        public Image usernameImage, passwordImage;

        void Start() {
            // TMP Input fields
            Debug.Assert(usernameText != null && passwordText != null);

            // Images
            Debug.Assert(usernameImage != null && passwordImage != null);
        }

        void Update() {
            // Username
            if (PolybiusManager.player.setUsername(usernameText.text)) {
                usernameImage.color = Color.white;
            } else {
                usernameImage.color = new Color32(255, 0, 0, 150);
            }

            // Password
            if (PolybiusManager.player.setPassword(passwordText.text)) {
               passwordImage.color = Color.white;
            } else {
               passwordImage.color = new Color32(255, 0, 0, 150);
            }
        }
    }
}