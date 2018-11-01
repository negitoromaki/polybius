using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class LobbyPanel : MonoBehaviour {

        public GamePanel gp;
        public GameObject MapPanel, parent, createPanel, scrollRect, ErrorMessage;
        public float currLat, currLong;
        private bool runOnce;

        void OnEnable() {
            if (gp.currGameType != Game.type.none) {
                if (getLocation()) {
                    PolybiusManager.mutex = true;
                    runOnce = true;
                    PolybiusManager.dm.getLobbiesQuery(); // TODO: Change this to use getLobbiesQuery

                    // Debug
                    /*
                    {
                        PolybiusManager.games.Add(new Game(25f, 25f, PolybiusManager.player, gp.currGameType));
                        for (int i = 0; i < PolybiusManager.games.Count; i++) {
                            GameObject game = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Lobby"), parent.transform);
                            float delta = getCoordDist(PolybiusManager.games[i].coordLat, PolybiusManager.games[i].coordLong, currLat, currLong);
                            game.transform.Find("Join Game").Find("Text").GetComponent<TextMeshProUGUI>().text = "Join - " + delta * 5280 + "ft";
                            int temp = i;
                            game.transform.Find("Join Game").GetComponent<Button>().onClick.AddListener(() => startGame(temp));
                            game.transform.Find("Map").GetComponent<Button>().onClick.AddListener(() => displayLocation(temp));
                        }
                    }
                    */
                } else {
                    GetComponent<UIPanelSwitcher>().ChangeMenu(ErrorMessage);
                }
            } else {
                Debug.LogError("Gametype == none!!");
            }
        }

        public void Update() {
            if (!PolybiusManager.mutex && runOnce) {
                runOnce = false;

                Debug.Assert(gp != null &&
                            MapPanel != null &&
                            parent != null &&
                            createPanel != null &&
                            scrollRect != null &&
                            ErrorMessage != null);

                foreach (Transform child in parent.transform)
                    GameObject.Destroy(child.gameObject);

                GameObject game;
                for (int i = 0; i < PolybiusManager.games.Count; i++) {
                    game = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Lobby"), parent.transform);
                    float delta = getCoordDist(PolybiusManager.games[i].coordLat, PolybiusManager.games[i].coordLong, currLat, currLong);
                    game.transform.Find("Join Game").Find("Text").GetComponent<TextMeshProUGUI>().text = "Join - " + delta * 5280 + "ft";
                    int temp = i;
                    game.transform.Find("Join Game").GetComponent<Button>().onClick.AddListener(() => startGame(temp));
                    game.transform.Find("Map").GetComponent<Button>().onClick.AddListener(() => displayLocation(temp));
                }
            }
        }

        public void startGame(int i) {
            // TODO: Switch to game scene
            Debug.Log("Start game " + i);
        }

        public void displayLocation(int i) {
            MapPanel.GetComponent<MapPanel>().setLocation(PolybiusManager.games[i].coordLat, PolybiusManager.games[i].coordLong);
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
            // Get Location
            // Check if location is enabled
            if (!Input.location.isEnabledByUser)
                return false;

            // Wait until service initializes
            Input.location.Start();
            for (int i = 0; i < 20 && Input.location.status == LocationServiceStatus.Initializing; i++)
                Thread.Sleep(1000);

            // Check if service did not initialize
            if (Input.location.status == LocationServiceStatus.Initializing) {
                Debug.LogError("Timed out");
                return false;
            }

            if (Input.location.status == LocationServiceStatus.Failed) {
                Debug.LogError("Unable to determine device location");
                return false;
            } else {
                currLat = Input.location.lastData.latitude;
                currLong = Input.location.lastData.longitude;
                Debug.Log(currLat + ", " + currLong);
            }

            Input.location.Stop();
            return true;
        }

        public void backButton() {
            if (createPanel.activeSelf) {
                GetComponent<UIPanelSwitcher>().ChangeMenu(scrollRect);
            } else {
                gp.currGameType = Game.type.none;
            }
        }
    }
}