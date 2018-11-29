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
        private List<Message> messages;

        public void OnEnable() {
            Debug.Assert(otherUser != null && scrollRect != null && title != null && messageParent != null);
            title.GetComponent<TextMeshProUGUI>().text = otherUser.getUsername();
            messages = PolybiusManager.dm.getMessages(otherUser.getUsername());
        }

        public void Update() {
            if (Time.frameCount % 30 == 0)
            {
                List<Message> m = PolybiusManager.dm.getMessages(otherUser.getUsername());
                if(m!=null)
                messages = m;

            }

            if (messages.Count > 0)
                displayMessages();
        }

        void displayMessages() {
            for (int i = 0; i < messages.Count; i++) {
                GameObject newMessage = null;
                if (messages[i].senderID == PolybiusManager.player.getUserID()) {
                    newMessage = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Blue Message"), messageParent.transform);
                } else {
                    newMessage = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Green Message"), messageParent.transform);
                }

                newMessage.transform.Find("Message").gameObject.GetComponent<TextMeshProUGUI>().text = messages[i].message;
                newMessage.transform.Find("Timestamp").gameObject.GetComponent<TextMeshProUGUI>().text = messages[i].time;
            }
            messages.Clear();
        }

        IEnumerator ScrollToBottom() {
            yield return new WaitForEndOfFrame();
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public void sendMessage(TMP_InputField inputField) {
            if (!string.IsNullOrEmpty(inputField.text)) {
                Message m = new Message(PolybiusManager.player,
                                        otherUser,
                                        DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(),
                                        inputField.text);

                messages.Add(m);
                PolybiusManager.dm.sendMessage(m);

                inputField.text = "";
                displayMessages();
                StartCoroutine(ScrollToBottom());
            }
        }

        // Setter functions for username and id
        public void changeOtherUser(User u) {
            otherUser = u;
        }
    }
}
