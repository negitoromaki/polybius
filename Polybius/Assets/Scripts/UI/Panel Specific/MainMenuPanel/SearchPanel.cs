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

        void Start() {
            Debug.Assert(searchBar != null && parent != null && ProfilePanel != null && ButtonPanel != null);
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

                results = PolybiusManager.dm.userSearch(currSearch);
                Debug.Assert(MainMenuPanel != null &&
                                ProfilePanel != null &&
                                parent != null);

                GameObject result;
                for (int i = 0; i < results.Count; i++) {
                    result = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/FriendButton"), parent.transform);
                    result.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = results[i].getUsername();
                    int temp = i;
                    result.transform.Find("FriendProfile").GetComponent<Button>().onClick.AddListener(() => openUserProfile(temp));
                    result.transform.Find("ChatButton").gameObject.SetActive(false);
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
    }
}