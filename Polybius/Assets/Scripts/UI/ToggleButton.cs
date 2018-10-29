using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace polybius {
    public class ToggleButton : MonoBehaviour {

        // Colors
        private Color32 red = new Color32(255, 167, 167, 255);
        private Color32 white = new Color32(255, 255, 255, 255);

        private bool on = false;

        public void toggleIcon() {
            on = !on;
            if (on) {
                GetComponent<Image>().color = red;
            } else {
                GetComponent<Image>().color = white;
            }
        }
    }
}
