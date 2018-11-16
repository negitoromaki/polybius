using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class SearchPanel : MonoBehaviour {

        public GameObject searchBar, MainMenuPanel, ProfilePanel, ButtonPanel, parent;
        public GameObject ProfileButton, SearchButton;

        private TMP_InputField searchBarText;
        private string currSearch;
        private static bool runOnce = false;

        private void OnEnable() {
            Debug.Assert(searchBar != null &&
                            parent != null &&
                            ProfilePanel != null &&
                            ButtonPanel != null &&
                            ProfileButton != null &&
                            SearchButton != null);
            searchBarText = searchBar.GetComponent<TMP_InputField>();
            Debug.Assert(searchBarText != null);
            foreach (Transform child in parent.transform)
                GameObject.Destroy(child.gameObject);
            searchBarText.text = currSearch = "";
            PolybiusManager.dm.getBlockQuery();
        }

        // Update is called once per frame
        void Update() {
            if (searchBarText.text != currSearch) {
                currSearch = searchBarText.text;
                foreach (Transform child in parent.transform)
                    GameObject.Destroy(child.gameObject);

                PolybiusManager.mutex = true;
                runOnce = true;
                PolybiusManager.dm.getUsersQuery();
            }

            if (!PolybiusManager.mutex && runOnce) {
                runOnce = false;
                Debug.Assert(MainMenuPanel != null &&
                                ProfilePanel != null &&
                                parent != null);

                GameObject result;
                for (int i = 0; i < PolybiusManager.results.Count; i++) {
					if (PolybiusManager.results[i].getUsername() != null &&
                        PolybiusManager.results[i].getUsername().ToLower().Contains(currSearch.ToLower()) &&
                        PolybiusManager.player.getUsername() != PolybiusManager.results[i].getUsername() &&
                        !PolybiusManager.player.blockedUsers.Contains(PolybiusManager.player.friends[i].getUsername())) {
                        result = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UserButton"), parent.transform);
                        result.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = PolybiusManager.results[i].getUsername();
                        int temp = i;
                        result.transform.Find("FriendProfile").GetComponent<Button>().onClick.AddListener(() => openUserProfile(temp));
                        result.transform.Find("AddFriend").GetComponent<Button>().onClick.AddListener(() => toggleFriendStatus(temp));
                        if (PolybiusManager.player.friends.Contains(PolybiusManager.results[i]))
                            result.transform.Find("AddFriend").GetComponent<FriendButton>().toggleFriendIcon();
                    } else {
                        Debug.Log("User " + PolybiusManager.results[i].getUsername() + " does not match search");
                    }
                }
            }
        }

        public void openUserProfile(int i) {
            ButtonPanel.GetComponent<ButtonPanel>().PressButton(ProfileButton);
            ProfilePanel p = ProfilePanel.GetComponent<ProfilePanel>();
            p.changeUser(PolybiusManager.results[i]);
            p.prevButton = SearchButton;
            p.prevPanel = this.gameObject;
            MainMenuPanel.GetComponent<UIPanelSwitcher>().ChangeMenu(ProfilePanel);
        }

        public void toggleFriendStatus(int i) {
            if (PolybiusManager.player.friends.Contains(PolybiusManager.results[i])) {
				PolybiusManager.dm.RemoveFriend(PolybiusManager.player.getUsername(),PolybiusManager.results[i].getUserID());
            } else {
				PolybiusManager.dm.AddFriend(PolybiusManager.player.getUsername(),PolybiusManager.results[i].getUserID());
            }
            PolybiusManager.dm.getFriendsQuery();
        }
    }
}