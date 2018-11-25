using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public class jsontesting : MonoBehaviour {

        string flaskIP = "http://128.211.240.229:5000";

        void Start() {
            addBob();
        }

        public void addBob() {
            string json = "{ \"username\":  \"bob\", " +
                        "\"password\":  \"asdfasdf\", " +
                        "\"email\":     \"asdf@asdf.com\", " +
                        "\"dob\": \"\", " + "\"privacy\": 0, " + "}";
            Debug.Log("Register: " + PolybiusManager.dm.postJson("GET", json, flaskIP + "/users"));
        }

        public void loginBob() {
            string json = "{ \"username\": \"bob\"}";
            User u = new User();
            JsonUtility.FromJsonOverwrite(PolybiusManager.dm.postJson("GET", json, flaskIP + "/users"), u);
            Debug.Log(
                "Username: " + u.getUsername() + "\n" +
                "Password: " + u.getPassword()
                );
        }
    }
}
