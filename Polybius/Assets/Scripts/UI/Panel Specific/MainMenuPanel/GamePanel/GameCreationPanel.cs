using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace polybius {
    public class GameCreationPanel : MonoBehaviour {

        public GamePanel gp;
        public GameObject errorMessage;
        public TextMeshProUGUI header;
        private float currLat, currLong;

        public void OnEnable() {
            Debug.Assert(errorMessage != null && header != null);
            header.text = "Create new " + gp.currGameType.ToString() + " game";
        }

        public void createGame() {
            if (getLocation()) {
                // TODO: call game creation method
            } else {
                Debug.LogError("Could not get location");
                gp.GetComponent<UIPanelSwitcher>().ChangeMenu(errorMessage);
            }
        }

        bool getLocation() {
            // Get Location
            // Check if location is enabled
            if (!Input.location.isEnabledByUser)
                return false;

            // Wait until service initializes
            Input.location.Start();
            for (int i = 0; i < 20 && Input.location.status == LocationServiceStatus.Initializing; i++)
                System.Threading.Thread.Sleep(1000);

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
    }
}