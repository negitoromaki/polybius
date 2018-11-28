using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace polybius {
    public class FeedbackPanel : MonoBehaviour {
        public GameObject topic, feedback;


        private void Start() {
            Debug.Assert(topic != null && feedback != null);
        }


        public void sendFeedback() {
            PolybiusManager.dm.sendFeedBack("Topic: " + topic.GetComponent<TMP_InputField>().text,
                feedback.GetComponent<TMP_InputField>().text);
        }
    }
}