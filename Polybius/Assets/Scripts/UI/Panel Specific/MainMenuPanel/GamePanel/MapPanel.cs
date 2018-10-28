using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;

namespace polybius {
    public class MapPanel : MonoBehaviour {

        private Mapbox.Utils.Vector2d coord;

        void OnEnable() {
            Debug.Assert(coord != null);
            GetComponent<AbstractMap>().SetCenterLatitudeLongitude(coord);
        }

        public void setLocation(float dispLat, float dispLong) {
            coord = new Mapbox.Utils.Vector2d(dispLat, dispLong);
        }
    }
}