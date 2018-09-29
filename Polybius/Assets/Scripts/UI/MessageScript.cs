using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace polybius {
    public class MessageScript : MonoBehaviour {

        // References
        public GameObject greenMessage, blueMessage;
        private GameObject messageParent;

        private void Start() {
            messageParent = this.transform.Find("Messages").gameObject;
        }

        void Awake() {
            PolybiusManager.getMessages();
            /*
            for (int i = 0; i < PolybiusManager.messages.Count; i++) {

                // Create message
                GameObject message;
                if (PolybiusManager.messages[i].sender == PolybiusManager.userID) {
                    message = Instantiate(blueMessage, messageParent.transform);
                } else {
                    message = Instantiate(greenMessage, messageParent.transform);
                }
                message.transform.position += new Vector3(0, -150f * PolybiusManager.messages.Count, 0);
                message.transform.Find("Message").GetComponent<TextMeshPro>().text = PolybiusManager.messages[i].message;
                message.transform.Find("Timestamp").GetComponent<TextMeshPro>().text = PolybiusManager.messages[i].time;
                PolybiusManager.clearMessages();
            }
            */
        }

    }
}
