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
        private List<User> results;

        private void OnEnable() {
            // Checking
            Debug.Assert(searchBar != null &&
                            parent != null &&
                            ProfilePanel != null &&
                            ButtonPanel != null &&
                            ProfileButton != null &&
                            SearchButton != null);
            searchBarText = searchBar.GetComponent<TMP_InputField>();
            Debug.Assert(searchBarText != null);

            // Clear results
            foreach (Transform child in parent.transform)
                GameObject.Destroy(child.gameObject);
            searchBarText.text = currSearch = "";
        }

        // Update is called once per frame
        void Update() {
            if (searchBarText.text != currSearch) {
                // Clear results
                currSearch = searchBarText.text;
                foreach (Transform child in parent.transform)
                    GameObject.Destroy(child.gameObject);

                // Checking
                Debug.Assert(MainMenuPanel != null &&
                                ProfilePanel != null &&
                                parent != null);

                results = PolybiusManager.dm.searchUsers(currSearch);

                // Display results
                GameObject result;
                for (int i = 0; i < results.Count; i++) {
					if (results[i].getUsername() != null &&
                        results[i].getUsername().ToLower().Contains(currSearch.ToLower()) &&
                        PolybiusManager.player.getUsername() != results[i].getUsername() &&
                        !PolybiusManager.player.blockedUsers.Contains(results[i].getUsername())) {

                        result = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UserButton"), parent.transform);
                        result.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = results[i].getUsername();

                        // Ensure buttons work when clicked
                        int temp = i;
                        result.transform.Find("FriendProfile").GetComponent<Button>().onClick.AddListener(() => openUserProfile(temp));
                        result.transform.Find("AddFriend").GetComponent<Button>().onClick.AddListener(() => toggleFriendStatus(temp));
                        if (PolybiusManager.player.friends.Contains(results[i]))
                            result.transform.Find("AddFriend").GetComponent<FriendButton>().toggleFriendIcon();
                    } else {
                        Debug.Log("User " + results[i].getUsername() + " does not match search");
                    }
                }
            }
        }

        public void openUserProfile(int i) {
            ButtonPanel.GetComponent<ButtonPanel>().PressButton(ProfileButton);
            ProfilePanel p = ProfilePanel.GetComponent<ProfilePanel>();
            p.changeUser(results[i]);
            p.prevButton = SearchButton;
            p.prevPanel = this.gameObject;
            MainMenuPanel.GetComponent<UIPanelSwitcher>().ChangeMenu(ProfilePanel);
        }

        public void toggleFriendStatus(int i) {
            if (PolybiusManager.player.friends.Contains(results[i])) {
				PolybiusManager.dm.RemoveFriend(PolybiusManager.player.getUsername(),results[i].getUserID());
            } else {
				PolybiusManager.dm.AddFriend(PolybiusManager.player.getUsername(),results[i].getUserID());
            }

            // Update friends
            PolybiusManager.player.friends = PolybiusManager.dm.getFriends(PolybiusManager.player.getUserID());
        }
    }
}