using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class SettingsPanel : MonoBehaviour {

        public GameObject parent;

        // Colors
        private Color32 red = new Color32(255, 167, 167, 255);
        private Color32 white = new Color32(255, 255, 255, 255);

        // Username
        public GameObject username;
        private TMP_InputField usernameInput;
        private Image usernameImage;

        // Date of birth
        public GameObject dob;
        private TMP_InputField dobInput;
        private Image dobImage;

        // Privacy
        public TextMeshProUGUI privacyState;

        void Start() {
            // Main Settings
            username    = parent.transform.Find("Username").gameObject;
            dob         = parent.transform.Find("Date of Birth").gameObject;
            Debug.Assert(username != null && dob != null);

            // Username
            usernameInput = username.transform.Find("InputField").GetComponent<TMP_InputField>();
            usernameImage = username.transform.Find("InputField").GetComponent<Image>();
            Debug.Assert(usernameInput != null && usernameImage != null);
            usernameImage.color = white;
            usernameInput.text = PolybiusManager.player.getUsername();

            // Date of Birth
            dobInput = dob.transform.Find("InputField").GetComponent<TMP_InputField>();
            dobImage = dob.transform.Find("InputField").GetComponent<Image>();
            Debug.Assert(dobInput != null && dobImage != null);
            dobImage.color = white;
            dobInput.text = PolybiusManager.player.getDob();

            // Privacy
            Debug.Assert(privacyState != null);

        }

        void Update() {
            // Username
            if (PolybiusManager.player.setUsername(usernameInput.text)) {
                usernameImage.color = white;
            } else {
                usernameImage.color = red;
            }

            // Dob
            if (PolybiusManager.player.setDob(dobInput.text)) {
                dobImage.color = white;
            } else {
                dobImage.color = red;
            }

            // Social
            if (PolybiusManager.player.getPrivacy()==1) {
                privacyState.text = "Private";
            } else {
                privacyState.text = "Public";
            }
        }

        public void toggleSocial() {
            if (PolybiusManager.player.getPrivacy() == 0) {
                PolybiusManager.player.setPrivacy(1);
            } else {
                PolybiusManager.player.setPrivacy(0);
            }
        }
    }
}
