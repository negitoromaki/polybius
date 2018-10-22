using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class MessageScript : MonoBehaviour {
        // References
        public GameObject title, messageParent;

        // Variables
        public string senderUsername;
        public int senderID;
 
        private int count;

        public void Start() {
            Debug.Assert(!string.IsNullOrEmpty(senderUsername));
            title.GetComponent<TextMeshProUGUI>().text = senderUsername;
            count = 0;

            displayMessages();
        }

        public void Update() {
            displayMessages();
        }

        void displayMessages() {
            while (PolybiusManager.player.messages.Count > 0) {
                Message m = PolybiusManager.player.messages[0];
                GameObject newMessage = null;
                if (m.sender == PolybiusManager.player.userID) {
                    newMessage = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Blue Message"), messageParent.transform);
                } else {
                    newMessage = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Green Message"), messageParent.transform);
                }

                newMessage.transform.Find("Message").gameObject.GetComponent<TextMeshProUGUI>().text = m.message;
                newMessage.transform.Find("Timestamp").gameObject.GetComponent<TextMeshProUGUI>().text = m.timestamp.ToString();
                
                // Increment number of messages
                PolybiusManager.player.messages.RemoveAt(0);
                count++;
            }
        }

        public void sendMessage(InputField inputField) {
            Message m = new Message(PolybiusManager.player.userID, senderID, System.DateTime.Now, inputField.text);
            PolybiusManager.player.messages.Add(m);
            displayMessages();
        }
    }
}
