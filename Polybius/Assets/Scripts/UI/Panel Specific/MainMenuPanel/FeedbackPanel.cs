using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace polybius {
    public class FeedbackPanel : MonoBehaviour {
        public GameObject topic, feedback;


        private void Start() {
            Debug.Assert(topic != null && feedback != null);
        }


        void sendFeedback() {
            PolybiusManager.dm.sendFeedBack("Topic: " + topic.GetComponent<TMP_InputField>().text + "\n" +
                feedback.GetComponent<TMP_InputField>().text);
        }
    }
}