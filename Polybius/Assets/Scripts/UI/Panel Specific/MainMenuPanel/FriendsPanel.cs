using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class FriendsPanel : MonoBehaviour {

        public GameObject MainMenuPanel, ProfilePanel, MessagePanel, ButtonPanel, parent;
        public GameObject ProfileButton, FriendButton;
        private List<User> friends = PolybiusManager.player.friends;
        private bool friendsUpdated = false;

        void Start() {
            friendsUpdated = false;

            PolybiusManager.dm.getFriendsQuery();
            for (int i = 0; i < 5 && friendsUpdated; i++)
                System.Threading.Thread.Sleep(1000);

            if (friendsUpdated) {
                Debug.Assert(MainMenuPanel != null &&
                                ProfilePanel != null &&
                                MessagePanel != null &&
                                ButtonPanel != null &&
                                ProfileButton != null &&
                                FriendButton != null &&
                                parent != null);

                GameObject friend;
                for (int i = 0; i < friends.Count; i++) {
                    friend = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/FriendButton"), parent.transform);
                    friend.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = friends[i].getUsername();
                    int temp = i;
                    friend.transform.Find("FriendProfile").GetComponent<Button>().onClick.AddListener(() => openUserProfile(temp));
                    friend.transform.Find("ChatButton").GetComponent<Button>().onClick.AddListener(() => openMessage(temp));
                }
            } else {
                Debug.LogError("Friends could not be updated");
            }
        } 

        public void openUserProfile(int i) {
            ButtonPanel.GetComponent<ButtonPanel>().PressButton(ProfileButton);
            ProfilePanel p = ProfilePanel.GetComponent<ProfilePanel>();
            p.changeUser(friends[i]);
            p.prevButton = FriendButton;
            p.prevPanel = this.gameObject;
            MainMenuPanel.GetComponent<UIPanelSwitcher>().ChangeMenu(ProfilePanel);
        }

        public void openMessage(int i) {
            MessagePanel.GetComponent<MessagePanel>().changeOtherUser(friends[i]);
            MainMenuPanel.GetComponent<UIPanelSwitcher>().ChangeMenu(MessagePanel);
        }

        public void setFriends(List<User> friends) {
            PolybiusManager.player.friends = friends;
            friendsUpdated = true;
        }
    }
}