using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace polybius {
    public class jsontesting : MonoBehaviour {

        string flaskIP = "http://128.211.240.229:5000";

        void Start() {
            loginBob();
        }

        public void addBob() {
            // JSON
            WWWForm form = new WWWForm();
            form.AddField("username", "bob");
            form.AddField("password", "asdfasdf");
            form.AddField("email", "asdf@asdf.com");
            form.AddField("dob", "--/--/----");
            form.AddField("privacy", 0);

            Debug.Log("Register: " + PolybiusManager.dm.postJson("POST", form, flaskIP + "/users"));
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
    }
}
