using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class FriendsPanel : MonoBehaviour {

        public GameObject MainMenuPanel, ProfilePanel, MessagePanel, ButtonPanel, parent;
        public GameObject ProfileButton, FriendButton;

        void OnEnable() {
            foreach (Transform child in parent.transform)
                GameObject.Destroy(child.gameObject);

            // Update friends
            PolybiusManager.player.friends = PolybiusManager.dm.getFriends(PolybiusManager.player.getUserID());
            Debug.Log("Number of friends: " + PolybiusManager.player.friends.Count);

            // Checking
            Debug.Assert(MainMenuPanel != null &&
                            ProfilePanel != null &&
                            MessagePanel != null &&
                            ButtonPanel != null &&
                            ProfileButton != null &&
                            FriendButton != null &&
                            parent != null);

            // Display friends
            GameObject friend;
            for (int i = 0; i < PolybiusManager.player.friends.Count; i++) {
                if (!PolybiusManager.player.blockedUsers.Contains(PolybiusManager.player.friends[i].getUserID())) {
                    friend = Instantiate(Resources.Load<GameObject>("Prefabs/UI/FriendButton"), parent.transform);
                    friend.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = PolybiusManager.player.friends[i].getUsername();
                    int temp = i;
                    friend.transform.Find("FriendProfile").GetComponent<Button>().onClick.AddListener(() => openUserProfile(temp));
                    friend.transform.Find("ChatButton").GetComponent<Button>().onClick.AddListener(() => openMessage(temp));
                }
            }
        }

        public void openUserProfile(int i) {
            ButtonPanel.GetComponent<ButtonPanel>().PressButton(ProfileButton);
            ProfilePanel p = ProfilePanel.GetComponent<ProfilePanel>();
            p.changeUser(PolybiusManager.player.friends[i]);
            p.prevButton = FriendButton;
            p.prevPanel = this.gameObject;
            MainMenuPanel.GetComponent<UIPanelSwitcher>().ChangeMenu(ProfilePanel);
        }

        public void openMessage(int i) {
            MessagePanel.GetComponent<MessagePanel>().changeOtherUser(PolybiusManager.player.friends[i]);
            MainMenuPanel.GetComponent<UIPanelSwitcher>().ChangeMenu(MessagePanel);
        }
    }
}
