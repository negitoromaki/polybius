using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace polybius {
    public class DatabaseManager : MonoBehaviour {

        // Server Configuration
        public string ip = "";
        public int port = 9933;
        public SmartFox sfs = new SmartFox();
        public string initZone = "Polybius";
        public bool logged = false;

        void Start() {
            PolybiusManager.dm = this;
            sfs.AddEventListener(SFSEvent.LOGIN, onLogin);
            sfs.AddEventListener(SFSEvent.CONNECTION, onConnect);
            sfs.AddEventListener(SFSEvent.CONNECTION_LOST, onLost);
            sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, onResponse);
            sfs.ThreadSafeMode = true;

            sfs.Connect(ip, port);
        }

        void Update() {
            sfs.ProcessEvents();
        }

        //event listeners
        void onConnect(BaseEvent e) {
            if ((bool)e.Params["success"]) {
                Debug.Log("Connected");
                sfs.Send(new LoginRequest("guest", "", initZone));
            } else {
                Debug.Log("Connection failed");
            }
        }

        void onLost(BaseEvent e) {
            logged = false;
        }

        void onLogin(BaseEvent e) {
            logged = true;
        }

        void onResponse(BaseEvent e) {
            string cmd = (string)e.Params["cmd"];
            ISFSObject paramsa = (SFSObject)e.Params["params"];
            string message = cmd + " " + paramsa.GetUtfString("result") + " message: " + paramsa.GetUtfString("message");
            Debug.Log(cmd + " message: " + message);

            if (cmd == "UserLogin") {
                string result = paramsa.GetUtfString("result");
                if (result == "success") {
                    // login successful
                    Debug.Log("Login successful!");
                    PolybiusManager.loggedIn = true;
                } else {
                    Debug.LogError("Error with login: " + result);
                }
            }

            if (cmd == "CreateUser") {
                string result = paramsa.GetUtfString("result");
                if (result == "success") {
                    Debug.Log("Register successful!");
                    //UILogin(); // login after register is successful
                } else {
                    Debug.LogError("Error with registration: " + result);
                }
            }
        }

        //public methods
        // login user
        public void login() {
            PolybiusManager.loggedIn = false;
            if (!string.IsNullOrEmpty(PolybiusManager.player.password) &&
                !string.IsNullOrEmpty(PolybiusManager.player.username)) {

                ISFSObject l = new SFSObject();
                l.PutUtfString("username", PolybiusManager.player.username);
                l.PutUtfString("password", PolybiusManager.player.password);
                sfs.Send(new ExtensionRequest("UserLogin", l));
            }
        }

        // register user
        public void create() {
            PolybiusManager.loggedIn = false;
            if (!string.IsNullOrEmpty(PolybiusManager.player.password) &&
                !string.IsNullOrEmpty(PolybiusManager.player.username) &&
                !string.IsNullOrEmpty(PolybiusManager.player.email)) {

                ISFSObject o = new SFSObject();
                o.PutUtfString("username", PolybiusManager.player.username);
                o.PutUtfString("password", PolybiusManager.player.password);
                o.PutUtfString("email", PolybiusManager.player.email);
                //o.PutUtfString("dob", PolybiusManager.player.dob); // TODO: add DOB functionality
                sfs.Send(new ExtensionRequest("CreateUser", o));
            }
        }

        public SmartFox getConnection() {
            return sfs;
        }

        //exit handler
        void OnApplicationQuit() {
            PolybiusManager.loggedIn = false;
            Debug.Log("exiting");
            sfs.RemoveAllEventListeners();
            if (sfs.IsConnected) {
                sfs.Disconnect();
            }
        }
    }
}