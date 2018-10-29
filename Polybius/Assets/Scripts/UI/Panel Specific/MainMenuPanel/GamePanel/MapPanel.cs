using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;

namespace polybius {
    public class MapPanel : MonoBehaviour {

        [Geocode]
        public string locationString;
        public Vector2d location;

        public AbstractMap map;
        private float _spawnScale = 10f;
        public GameObject _markerPrefab;

        private List<GameObject> _spawnedObjects;

        void OnEnable() {
            Debug.Assert(map != null);
            _spawnedObjects = new List<GameObject>();

            location = Conversions.StringToLatLon(locationString);
            map.SetCenterLatitudeLongitude(location);
            map.Initialize(location, 10);
            GameObject instance = Instantiate(_markerPrefab);
            instance.transform.localPosition = map.GeoToWorldPosition(location, true);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            _spawnedObjects.Add(instance);

        }

        private void Update() {
            int count = _spawnedObjects.Count;
            for (int i = 0; i < count; i++) {
                var spawnedObject = _spawnedObjects[i];
                spawnedObject.transform.localPosition = map.GeoToWorldPosition(location, true);
                spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }
        }

        public void setLocation(float dispLat, float dispLong) {
            locationString = (dispLat + ", " + dispLong);
        }
    }
}