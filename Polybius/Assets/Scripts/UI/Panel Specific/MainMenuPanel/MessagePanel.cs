using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class MessagePanel : MonoBehaviour {
        // References
        public GameObject title, messageParent;

        // Variables
        private User otherUser;
        private List<Message> messages;

        public void OnAwake() {
            Debug.Assert(otherUser != null);
            title.GetComponent<TextMeshProUGUI>().text = otherUser.getUsername();
            messages = PolybiusManager.player.getMessages(otherUser.getUserID());
        }

        public void Update() {
            if (Time.frameCount % 30 == 0)
                messages = PolybiusManager.player.getMessages(otherUser.getUserID());
            displayMessages();
        }

        void displayMessages() {
            while (messages.Count > 0) {
                GameObject newMessage = null;
                if (messages[0].sender == PolybiusManager.player.getUserID()) {
                    newMessage = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Blue Message"), messageParent.transform);
                } else {
                    newMessage = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Green Message"), messageParent.transform);
                }

                newMessage.transform.Find("Message").gameObject.GetComponent<TextMeshProUGUI>().text = messages[0].message;
                newMessage.transform.Find("Timestamp").gameObject.GetComponent<TextMeshProUGUI>().text = messages[0].timestamp.ToString();
            }
        }

        public void sendMessage(InputField inputField) {
            Message m = new Message(PolybiusManager.player.getUserID(), otherUser.getUserID(), System.DateTime.Now, inputField.text);
            PolybiusManager.player.sendMessage(m);
        }

        // Setter functions for username and id
        public void changeOtherUser(User u) {
            otherUser = u;
        }
    }
}
