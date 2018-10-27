using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace polybius {
    public class SearchPanel : MonoBehaviour {

        public GameObject searchBar, parent;
        private TMP_InputField searchBarText;
        private string currSearch;

        // Use this for initialization
        void Start() {
            Debug.Assert(searchBar != null && parent != null);
            searchBarText = searchBar.GetComponent<TMP_InputField>();
            Debug.Assert(searchBarText != null);
            currSearch = "";
        }

        // Update is called once per frame
        void Update() {
            if (searchBarText.text != currSearch) {
                // TODO: Call PolybiusManager.dm.userSearch(string username)
            }
        }
    }
}