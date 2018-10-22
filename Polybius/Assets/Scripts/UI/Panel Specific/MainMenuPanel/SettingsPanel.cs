using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class SettingsPanel : MonoBehaviour {

        public GameObject parent;

        // Username
        private GameObject username;
        private TMP_InputField usernameInput;

        // Date of birth
        private GameObject dob;
        private Color32 red = new Color32(255, 167, 167, 255);
        private Color32 white = new Color32(255, 255, 255, 255);
        private TMP_InputField dobInput;
        private Image dobImage;

        void Start() {
            // Main Settings
            username    = parent.transform.Find("Username").gameObject;
            dob         = parent.transform.Find("Date of Birth").gameObject;
            Debug.Assert(username != null && dob != null);

            // Username
            usernameInput = username.transform.Find("InputField").GetComponent<TMP_InputField>();
            Debug.Assert(usernameInput != null);
            usernameInput.text = PolybiusManager.player.username;

            // Date of Birth
            dobInput = dob.transform.Find("InputField").GetComponent<TMP_InputField>();
            dobImage = dob.transform.Find("InputField").GetComponent<Image>();
            Debug.Assert(dobInput != null && dobImage != null);
            dobImage.color = white;
            dobInput.text = PolybiusManager.player.dob;
        }

        void Update() {
            // Username
            if (usernameInput.text != PolybiusManager.player.username)
                PolybiusManager.player.username = usernameInput.text;

            // Date of Birth
            Match m = Regex.Match(dobInput.text, "(1[012]|0?[1-9])\\/(3[01]|[12][0-9]|0?[1-9])\\/((?:19|20)\\d{2})");
            Debug.Log(m.Success + " " + dobImage.color);
            if (m.Success) {
                PolybiusManager.player.dob = dobInput.text;
                dobImage.color = white;
            } else {
                dobImage.color = red;
            }
        }
    }
}