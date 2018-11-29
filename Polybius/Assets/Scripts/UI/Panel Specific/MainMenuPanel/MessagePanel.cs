using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class MessagePanel : MonoBehaviour {
        // References
        public ScrollRect scrollRect;
        public GameObject title, messageParent;

        // Variables
        private User otherUser;
        private List<Message> localMessages;

        public void OnEnable() {
            otherUser = PolybiusManager.dm.getUser(otherUser.getUsername());
            Debug.Assert(otherUser != null && scrollRect != null && title != null && messageParent != null);
            title.GetComponent<TextMeshProUGUI>().text = otherUser.getUsername();
            localMessages = PolybiusManager.dm.getMessages(otherUser);
        }

        public void Update() {
            if (Time.frameCount % 30 == 0)
                localMessages = PolybiusManager.dm.getMessages(otherUser);

            if (localMessages.Count > 0)
                displayMessages();
        }

        void displayMessages() {
            foreach (Message m in localMessages) {
                GameObject newMessage = null;
                if (m.senderID == PolybiusManager.player.getUserID()) {
                    newMessage = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Blue Message"), messageParent.transform);
                } else {
                    newMessage = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Green Message"), messageParent.transform);
                }

                newMessage.transform.Find("Message").gameObject.GetComponent<TextMeshProUGUI>().text = m.message;
                newMessage.transform.Find("Timestamp").gameObject.GetComponent<TextMeshProUGUI>().text = m.time;
            }
            localMessages.Clear();
        }

        IEnumerator ScrollToBottom() {
            yield return new WaitForEndOfFrame();
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public void sendMessage(TMP_InputField inputField) {
            if (!string.IsNullOrEmpty(inputField.text)) {
                Debug.Log("Time: " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToShortTimeString().ToString());
                Message m = new Message(PolybiusManager.player,
                                        otherUser,
                                        DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToShortTimeString().ToString(),
                                        inputField.text);

                PolybiusManager.dm.sendMessage(m);

                inputField.text = "";
                localMessages = PolybiusManager.dm.getMessages(otherUser);
                localMessages.Add(m);
                displayMessages();
                StartCoroutine(ScrollToBottom());
            }
        }

        public void OnDisable() {
            foreach (Transform child in messageParent.transform)
                GameObject.Destroy(child.gameObject);
            localMessages.Clear();
            otherUser = null;
        }

        // Setter functions for username and id
        public void changeOtherUser(User u) {
            otherUser = u;
        }
    }
}
