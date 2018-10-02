using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace polybius {
    public class MessageScript : MonoBehaviour {
        // Variables
        public string senderUsername;
        public int senderID;

        // References
        public GameObject messageParent;
        public GameObject title;
        private GameObject greenMessage, blueMessage;

        void Awake() {
            // DEBUG
            senderID = 0;
            senderUsername = "Bob";
            PolybiusManager.userID = 3;
            for (int i = 0; i < 5; i++) {
                Message m = new Message(i, System.DateTime.Now, "Hello: " + i);
                PolybiusManager.messages.Add(m);
            }

            // Initialization
            greenMessage = (GameObject) Resources.Load("Prefabs/UI/Green Message");
            blueMessage = (GameObject) Resources.Load("Prefabs/UI/Blue Message");
            title.GetComponent<TextMeshProUGUI>().text = senderUsername;

            // Display messages
            for (int i = 0; i < PolybiusManager.messages.Count; i++) {
                GameObject message;
                if (PolybiusManager.messages[i].sender == PolybiusManager.userID) {
                    message = Instantiate(blueMessage, messageParent.transform);
                } else {
                    message = Instantiate(greenMessage, messageParent.transform);
                }
                message.transform.position += new Vector3(0, -150f * PolybiusManager.messages.Count, 0);
                message.transform.Find("Message").gameObject.GetComponent<TextMeshPro>().text = PolybiusManager.messages[i].message;
                message.transform.Find("Timestamp").gameObject.GetComponent<TextMeshPro>().text = PolybiusManager.messages[i].timestamp.ToString();
            }
            PolybiusManager.messages.Clear();
        }

    }
}
