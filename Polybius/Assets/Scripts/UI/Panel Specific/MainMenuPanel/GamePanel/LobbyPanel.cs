using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace polybius {
    public class LobbyPanel : MonoBehaviour {

        public GameObject MapPanel, parent, ErrorMessage;
        private List<Game> games;
        private float currLat, currLong;
        private Game.type gameType = Game.type.none;

        void OnEnable() {
            if (getLocation()) {

                Debug.Assert(gameType != Game.type.none);

                games = PolybiusManager.dm.getLobbies(gameType);
                GameObject game;
                for (int i = 0; i < games.Count; i++) {
                    game = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Lobby"), parent.transform);
                    float delta = getCoordDist(games[i].coordLat, games[i].coordLong, currLat, currLong);
                    game.transform.Find("Join Game").Find("Text").GetComponent<TextMeshProUGUI>().text = "Join - " + delta/5280 + "ft";
                    int temp = i;
                    game.transform.Find("Join Game").GetComponent<Button>().onClick.AddListener(() => startGame(i));
                    game.transform.Find("Map").GetComponent<Button>().onClick.AddListener(() => displayLocation(i));
                }
            } else {
                GetComponent<UIPanelSwitcher>().ChangeMenu(ErrorMessage);
            }
        }

        public void startGame(int i) {

        }

        public void displayLocation(int i) {
            MapPanel.GetComponent<MapPanel>().setLocation(games[i].coordLat, games[i].coordLong);
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
            Input.location.Start();

            // Wait until service initializes
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
            }

            Input.location.Stop();
            return true;
        }

        public void setGameType(Game.type gameType) {
            this.gameType = gameType;
        }
    }
}