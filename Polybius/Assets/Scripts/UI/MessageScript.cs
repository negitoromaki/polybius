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
        private RectTransform scrollView;
        private int messageCount;

        void Awake() {
            // DEBUG
            senderID = 0;
            senderUsername = "Bob";
            PolybiusManager.userID = 3;
            for (int i = 0; i < 10; i++) {
                Message m = new Message(i, System.DateTime.Now, "Hello: " + i);
                PolybiusManager.messages.Add(m);
            }

            // Initialization
            greenMessage = (GameObject) Resources.Load("Prefabs/UI/Green Message");
            blueMessage = (GameObject) Resources.Load("Prefabs/UI/Blue Message");
            scrollView = messageParent.GetComponent<RectTransform>();
            title.GetComponent<TextMeshProUGUI>().text = senderUsername;
            scrollView.sizeDelta = new Vector2(scrollView.sizeDelta.x, 200);
            messageCount = 0;

            // Display messages
            while (PolybiusManager.messages.Count > 0) {
                displayMessage(PolybiusManager.messages[0]);
                PolybiusManager.messages.RemoveAt(0);
            }
        }

        void displayMessage(Message m) {
            GameObject message;
            if (m.sender == PolybiusManager.userID) {
                message = Instantiate(blueMessage, messageParent.transform);
            } else {
                message = Instantiate(greenMessage, messageParent.transform);
            }

            // Make sure there is enough space
            scrollView.sizeDelta = new Vector2(scrollView.sizeDelta.x, 250 + messageParent.GetComponent<RectTransform>().sizeDelta.y);
            message.transform.position += new Vector3(0, -250 * messageCount, 0);
            message.transform.Find("Message").gameObject.GetComponent<TextMeshProUGUI>().text = m.message;
            message.transform.Find("Timestamp").gameObject.GetComponent<TextMeshProUGUI>().text = m.timestamp.ToString();

            // Increment counter
            messageCount++;
        }

        void sendMessage() {

        }
    }
}
