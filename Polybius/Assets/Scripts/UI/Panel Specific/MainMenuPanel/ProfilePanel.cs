using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace polybius {
    public class ProfilePanel : MonoBehaviour {

        public GameObject parent;


        private List<GameObject> stats = new List<GameObject>();
        private GameObject titlePrefab;
        private GameObject statisticPrefab;

        private void Awake() {
            // Load resources
            titlePrefab = Resources.Load<GameObject>("Prefabs/UI/Title");
            statisticPrefab = Resources.Load<GameObject>("Prefabs/UI/Statistic");
            Debug.Assert(titlePrefab != null && statisticPrefab != null && parent != null);
        }

        void Start() {
            // Load Statistics

            // multidimensional array, [i,j], where i is number of stats,
            // j == 0 for stat title, and j == 1 for the value.
            // Section title is just a string
            string[,] userStats = new string[2,2]; 
            userStats[0, 0] = "Username: ";
                userStats[0, 1] = PolybiusManager.player.username;
            userStats[1, 0] = "Date of Birth: ";
                userStats[1, 1] = PolybiusManager.player.dob;
            addSection("User Statistics:", userStats);

            string[,] connect4Stats = new string[1,2];
            connect4Stats[0, 0] = "Games Won: ";
                connect4Stats[0, 1] = "3000";
            addSection("Connect 4 Statistics:", connect4Stats);
        }

        public void addSection(string titleText, string[,] statTexts) {
            // Instantiate Title
            GameObject stat = Instantiate(titlePrefab, parent.transform);
            stat.GetComponent<TextMeshProUGUI>().text = titleText;

            // Instantiate Statistics
            int len = statTexts.GetLength(0);
            for (int i = 0; i < len; i++) {
                stat = Instantiate(statisticPrefab, parent.transform);
                stat.transform.Find("StatTitle").GetComponent<TextMeshProUGUI>().text = statTexts[i, 0];
                stat.transform.Find("StatText").GetComponent<TextMeshProUGUI>().text = statTexts[i, 1];
            }
        }
    }
}
