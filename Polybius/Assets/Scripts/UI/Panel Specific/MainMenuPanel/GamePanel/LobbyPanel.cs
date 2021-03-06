using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace polybius {
    public class LobbyPanel : MonoBehaviour {

        public GamePanel gp;
        public GameObject MapPanel, parent, createPanel, scrollRect, ErrorMessage;
        private List<Game> games;

        void Update() {
            if (Time.frameCount % 30 == 0) {
                if (gp.currGameType != "none") {
                    if (getLocation()) {
                    } else {
                        //GetComponent<UIPanelSwitcher>().ChangeMenu(ErrorMessage);
                        // DEBUG: Location
                        PolybiusManager.currLat = 40.427604f;
                        PolybiusManager.currLong = -86.916966f;
                    }

                    // Checking
                    Debug.Assert(   gp != null &&
                                    MapPanel != null &&
                                    parent != null &&
                                    createPanel != null &&
                                    scrollRect != null &&
                                    ErrorMessage != null);

                    foreach (Transform child in parent.transform)
                        GameObject.Destroy(child.gameObject);

                    // Get games
                    games = PolybiusManager.dm.getLobbies(gp.currGameType);

                    GameObject game;
                    for (int i = 0; i < games.Count; i++) {
                        if (games[i].gameType == gp.currGameType) {
                            game = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Lobby"), parent.transform);
                            float delta = getCoordDist(games[i].latCoord, games[i].longCoord, PolybiusManager.currLat, PolybiusManager.currLong);
                            game.transform.Find("Join Game").Find("Text").GetComponent<TextMeshProUGUI>().text = "Join " + games[i].name + " - " + delta * 5280 + "ft";

                            // Ensure buttons work when clicked
                            int temp = i;
                            game.transform.Find("Join Game").GetComponent<Button>().onClick.AddListener(() => startGame(temp));
                            game.transform.Find("Map").GetComponent<Button>().onClick.AddListener(() => displayLocation(temp));
                        }
                    }
                } else {
                    Debug.LogError("Gametype == none!!");
                }
            }
        }

        public void startGame(int i) {
            PolybiusManager.currGame = games[i];

            // Start correct game scene
            if (games[i].gameType == "none") {
                Debug.LogError("Attempting to join game of type none!");
            } else if (games[i].gameType.ToLower() == "pong") {
                SceneManager.LoadScene("Pong");
            } else if (games[i].gameType.ToLower() == "tictactoe") {
                SceneManager.LoadScene("TicTacToe");
            } else if (games[i].gameType.ToLower() == "connect4") {
                SceneManager.LoadScene("Connect4");
            }
        }

        public void displayLocation(int i) {
            PolybiusManager.currGame = games[i];
            GetComponent<UIPanelSwitcher>().ChangeMenu(MapPanel);
        }

        float degreesToRadians(float degrees) {
            return degrees * Mathf.PI / 180;
        }

        // Based off of https://stackoverflow.com/questions/365826/calculate-distance-between-2-gps-coordinates
        float getCoordDist(float lat1, float long1, float lat2, float long2) {
            int earthRadius = 3959;
            float dLat  = degreesToRadians(lat2 - lat1);
            float dLong = degreesToRadians(long2 - long1);
            float templat  = degreesToRadians(lat1);
            float templong = degreesToRadians(lat2);

            float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) + Mathf.Sin(dLong / 2) * Mathf.Sin(dLong / 2) * Mathf.Cos(templat) * Mathf.Cos(templong);
            return earthRadius * 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        }

        bool getLocation() {

            #if UNITY_ANDROID
            Input.compass.enabled = true;
            #endif

            // Get Location
            // Check if location is enabled
            if (!Input.location.isEnabledByUser)
                return false;

            // Wait until service initializes
            Input.location.Start();
            for (int i = 0; i < 1 && Input.location.status == LocationServiceStatus.Initializing; i++)
                Thread.Sleep(500);

            // Check if service did not initialize
            if (Input.location.status == LocationServiceStatus.Initializing) {
                Debug.LogError("Timed out");
                return false;
            }

            if (Input.location.status == LocationServiceStatus.Failed) {
                Debug.LogError("Unable to determine device location");
                return false;
            }

            PolybiusManager.currLat = Input.location.lastData.latitude;
            PolybiusManager.currLong = Input.location.lastData.longitude;
            Debug.Log(PolybiusManager.currLat + ", " + PolybiusManager.currLong);

            Input.location.Stop();
            return true;
        }

        public void backButton() {
            if (createPanel.activeSelf) {
                GetComponent<UIPanelSwitcher>().ChangeMenu(scrollRect);
            } else {
                gp.currGameType = "none";
            }
        }
    }
}