using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public class NotificationManager : MonoBehaviour {

        // This prevents it from activating all the time
        private int count;

        void Update() {
            if (count != PolybiusManager.player.messages.Count &&
                PolybiusManager.player.messages.Count != 0) {
                Assets.SimpleAndroidNotifications.NotificationManager.SendWithAppIcon(System.TimeSpan.FromSeconds(5),
                                                                                            "New Messages",
                                                                                            "You have new messages from Polybius!",
                                                                                            new Color(0, 0.6f, 1),
                                                                                            Assets.SimpleAndroidNotifications.NotificationIcon.Message);
                count = PolybiusManager.player.messages.Count;
            }
        }
    }
}