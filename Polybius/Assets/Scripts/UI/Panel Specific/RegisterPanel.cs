﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class RegisterPanel : MonoBehaviour {

        public GameObject username, password, email;
        private TMP_InputField usernameText, passwordText, emailText;
        private Image usernameImage, passwordImage, emailImage;

        void Start() {
            Debug.Assert(username != null && password != null && email != null);

            // TMP Input fields
            usernameText    = username.GetComponent<TMP_InputField>();
            passwordText    = password.GetComponent<TMP_InputField>();
            emailText       = email.GetComponent<TMP_InputField>();
            Debug.Assert(usernameText != null && passwordText != null && emailText != null);

            // Images
            usernameImage = username.GetComponent<Image>();
            passwordImage = password.GetComponent<Image>();
            emailImage    = email.GetComponent<Image>();
            Debug.Assert(usernameImage != null && passwordImage != null && emailImage != null);
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

            // Password
            if (PolybiusManager.player.setEmail(emailText.text)) {
                emailImage.color = Color.white;
            } else {
                emailImage.color = Color.red;
            }
        }

        public void RegisterButton() {
            if (!string.IsNullOrEmpty(PolybiusManager.player.getUsername()) &&
                !string.IsNullOrEmpty(PolybiusManager.player.getPassword()) &&
                !string.IsNullOrEmpty(PolybiusManager.player.getEmail())) {
                PolybiusManager.dm.create();
                PolybiusManager.dm.login();
            }
        }
    }
}