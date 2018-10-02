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
        public GameObject messageParent, title;

        private static GameObject greenMessage, blueMessage;
        private static TMP_InputField input;
        private static RectTransform scrollView;
        private static int messageCount;

        void Awake() {
            // DEBUG
            /*
            for (int i = 0; i < 10; i++) {
                Message m = new Message(i, System.DateTime.Now, "Hello: " + i);
                PolybiusManager.player.messages.Add(m);
            }
            */

            // Initialization
            

            // Display messages
            while (PolybiusManager.player.messages.Count > 0) {
                displayMessage(PolybiusManager.player.messages[0]);
                PolybiusManager.player.messages.RemoveAt(0);
            }
        }

        void init() {
            greenMessage = (GameObject)Resources.Load("Prefabs/UI/Green Message");
            blueMessage = (GameObject)Resources.Load("Prefabs/UI/Blue Message");
            input = this.transform.Find("Footer").transform.Find("Input Bar").GetComponent<TMP_InputField>();
            scrollView = messageParent.GetComponent<RectTransform>();
            title.GetComponent<TextMeshProUGUI>().text = senderUsername;
            scrollView.sizeDelta = new Vector2(scrollView.sizeDelta.x, 200);
            messageCount = 0;
        }

        void displayMessage(Message m) {
            GameObject message;
            if (m.sender == PolybiusManager.player.userID) {
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

        public void sendMessage() {
            init();
            Debug.Log(PolybiusManager.player.userID + ", " + System.DateTime.Now + ", " + input.text);
            Message m = new Message(PolybiusManager.player.userID, System.DateTime.Now, input.GetComponent<TMP_InputField>().text);
            PolybiusManager.player.messages.Add(m);
            displayMessage(m);
        }

    }
}
