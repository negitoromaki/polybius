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
        private bool resultsUpdated = false;

        void Start() {
            Debug.Assert(   searchBar != null &&
                            parent != null &&
                            ProfilePanel != null &&
                            ButtonPanel != null &&
                            ProfileButton != null &&
                            SearchButton != null);
            searchBarText = searchBar.GetComponent<TMP_InputField>();
            Debug.Assert(searchBarText != null);
            currSearch = "";
        }

        // Update is called once per frame
        void Update() {
            if (searchBarText.text != currSearch) {
                currSearch = searchBarText.text;
                foreach (Transform child in parent.transform)
                    GameObject.Destroy(child.gameObject);

                resultsUpdated = false;
                PolybiusManager.dm.userSearchQuery(currSearch);
                for (int i = 0; i < 10 && resultsUpdated; i++)
                    System.Threading.Thread.Sleep(1000);

                if (resultsUpdated) {

                    Debug.Assert(MainMenuPanel != null &&
                                    ProfilePanel != null &&
                                    parent != null);

                    GameObject result;
                    for (int i = 0; i < results.Count; i++) {
                        result = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UserButton"), parent.transform);
                        result.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = results[i].getUsername();
                        int temp = i;
                        result.transform.Find("FriendProfile").GetComponent<Button>().onClick.AddListener(() => openUserProfile(temp));
                        result.transform.Find("AddFriend").GetComponent<Button>().onClick.AddListener(() => toggleFriendStatus(temp));
                        if (PolybiusManager.player.friends.Contains(results[i]))
                            result.transform.Find("AddFriend").GetComponent<FriendButton>().toggleFriendIcon();
                    }
                } else {
                    Debug.LogError("Search Results could not be updated");
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
                PolybiusManager.dm.RemoveFriend(results[i].getUsername());
            } else {
                PolybiusManager.dm.AddFriend(results[i].getUsername());
            }
        }

        public void setResults(List<User> results) {
            this.results = results;
            resultsUpdated = true;
        }
    }
}