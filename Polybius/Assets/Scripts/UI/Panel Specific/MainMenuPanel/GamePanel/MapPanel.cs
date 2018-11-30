using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;

namespace polybius {
    public class MapPanel : MonoBehaviour {

        public string locationString;
        public Vector2d location, userLoc;

        public AbstractMap map;
        private float _spawnScale = 30f;
        public GameObject _markerPrefab, userPrefab, markerParent;

        private GameObject userLocObj;
        private List<GameObject> _spawnedObjects;

        void OnEnable() {
            displayMap();
        }

        private void displayMap() {
            locationString = (PolybiusManager.currGame.latCoord + "," + PolybiusManager.currGame.longCoord);
            Debug.Log("Set location to: " + locationString);

            Debug.Assert(!string.IsNullOrEmpty(locationString) &&
                            map != null &&
                            markerParent != null &&
                            _markerPrefab != null &&
                            userPrefab != null);

            foreach (Transform child in markerParent.transform)
                GameObject.Destroy(child.gameObject);

            _spawnedObjects = new List<GameObject>();
            {
                location = Conversions.StringToLatLon(locationString);
                map.SetCenterLatitudeLongitude(location);
                map.Initialize(location, 15);
                GameObject instance = Instantiate(_markerPrefab, markerParent.transform);
                instance.transform.localPosition = map.GeoToWorldPosition(location, true);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                _spawnedObjects.Add(instance);
            }

            // User marker prefab
            {
                userLoc = Conversions.StringToLatLon(PolybiusManager.currLat + ", " + PolybiusManager.currLong);
                userLocObj = Instantiate(userPrefab, markerParent.transform);
                userLocObj.transform.localPosition = map.GeoToWorldPosition(userLoc, true);
                userLocObj.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }
        }

        private void Update() {
            int count = _spawnedObjects.Count;
            for (int i = 0; i < count; i++) {
                var spawnedObject = _spawnedObjects[i];
                spawnedObject.transform.localPosition = map.GeoToWorldPosition(location, true);
                spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }
            userLocObj.transform.localPosition = map.GeoToWorldPosition(userLoc, true);
            userLocObj.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        }
    }
}