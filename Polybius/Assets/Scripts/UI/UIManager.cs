using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius
{
    public class UIManager : MonoBehaviour
    {
        // TEMP, will be removed
        public List<TextMeshProUGUI> usernameTexts = new List<TextMeshProUGUI>();

        private void Update()
        {
            if (!string.IsNullOrEmpty(PolybiusManager.player.username))
            {
                foreach (TextMeshProUGUI t in usernameTexts)
                {
                    t.text = PolybiusManager.player.username;
                }
            }
        }
    }
}