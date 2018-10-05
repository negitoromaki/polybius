using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace polybius {
    public class UISetters : MonoBehaviour {
        // Player Setter Methods
        public void setUsername(InputField inputField) {
            SetPlayerField(inputField, "username");
        }

        public void setPassword(InputField inputField) {
            SetPlayerField(inputField, "password");
        }

        public void setEmail(InputField inputField) {
            SetPlayerField(inputField, "email");
        }

        public void setDob(InputField inputField) {
            SetPlayerField(inputField, "dob");
        }

        public void SetPlayerField(InputField inputField, string variable) {
            if (!string.IsNullOrEmpty(inputField.text)) {
                FieldInfo field = typeof(User).GetField(variable);
                field.SetValue(PolybiusManager.player, inputField.text);
                Debug.Log(variable + " set to: " + inputField.text);
            }
        }
    }
}
