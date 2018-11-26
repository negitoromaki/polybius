﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Proyecto26;

namespace polybius {
    public class jsontesting : MonoBehaviour {

        string flaskIP = "http://128.211.240.229:5000";

        void Start() {
            addBob();
        }

        public void addBob() {
            // JSON
            User u = new User("bob", "asdfasdf", "asdf@asdf.com", "--/--/----", -1, 1);
            RestClient.Post<User>(flaskIP + "/users", u).Then(resp => { Debug.Log(JsonUtility.ToJson(resp, true)); });
        }

        public void loginBob() {
            // Get user request
            User u = new User();
            string j = PolybiusManager.dm.postJson("GET", null, flaskIP + "/users?username=bob");
            StringBuilder s = new StringBuilder(j);
            s.Remove(0, 1);
            s.Remove(s.Length - 1, 1);
            j = s.ToString();
            Debug.Log(j);
            JsonUtility.FromJsonOverwrite(j, u);

            Debug.Log(
                "Username: " + u.getUsername() + "\n" +
                "Password: " + u.getPassword()
                );
        }

        public void logoutBob() {
            WWWForm form = new WWWForm();
            form.AddField("userID", 19);
            form.AddField("isOnline", 0);

            Debug.Log("Logout: " + PolybiusManager.dm.postJson("PUT", form, flaskIP + "/users"));
        }
    }
}