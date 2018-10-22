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
        public bool connected = false;
        public string result="None";
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
                connected = true;
                sfs.Send(new LoginRequest("guest", "", initZone));
            } else {
                Debug.Log("Connection failed");
            }
        }

        void onLost(BaseEvent e) {
            connected = false;
            PolybiusManager.loggedIn = false;

        }

        void onLogin(BaseEvent e) {
            connected = true;
            PolybiusManager.loggedIn = true;
        }

        void onResponse(BaseEvent e) {
            string cmd = (string)e.Params["cmd"];
            ISFSObject paramsa = (SFSObject)e.Params["params"];
            string message = cmd + " " + paramsa.GetUtfString("result") + " message: " + paramsa.GetUtfString("message");
            Debug.Log(cmd + " message: " + message);
            result = paramsa.GetUtfString("result");

            if (cmd == "UserLogin") {
                if (result == "success") {
                    // login successful
                    Debug.Log("Login successful!");
                    PolybiusManager.loggedIn = true;
                } else {
                    Debug.LogError("Error with login: " + result);
                }
            } else if (cmd == "CreateUser") {
                if (result == "success") {
                    Debug.Log("Register successful!");
                    //UILogin(); // login after register is successful
                } else {
                    Debug.LogError("Error with registration: " + result);
                }
            } else if (cmd == "Messages") {
                if (result == "success") {
                    Debug.Log("Message successful!");
                } else {
                    Debug.LogError("Error with Message: " + result);
                }
            }else if (cmd == "UserLogout")
            {
                if (result == "success")
                {
                    PolybiusManager.loggedIn = false;
                    Debug.Log("Logout successful!");
                }
                else
                {
                    Debug.LogError("Error with Logout: " + result);
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
        public void logout()
        {
            PolybiusManager.loggedIn = false;
            if (!string.IsNullOrEmpty(PolybiusManager.player.username))
            {
                ISFSObject ot = new SFSObject();
                ot.PutUtfString("username", PolybiusManager.player.username);
                sfs.Send(new ExtensionRequest("UserLogout", ot));
            }
        }

        // get message
        public void getMessages(int senderID) {
            ISFSObject o = new SFSObject();
            o.PutUtfString("level", "private");
            o.PutUtfString("levelname", "Polybius");
            o.PutUtfString("mode", "get");
            o.PutUtfString("mReceiver", PolybiusManager.player.userID.ToString());
            o.PutUtfString("mSender", senderID.ToString());
            o.PutInt("amount", 1);
            sfs.Send(new ExtensionRequest("Messages", o));
        }

        // send message
        public void sendMessage(Message m) {
            ISFSObject o = new SFSObject();
            o.PutUtfString("level", "private");
            o.PutUtfString("levelname", "Polybius");
            o.PutUtfString("mode", "send");
            o.PutUtfString("mReceiver", m.receiver.ToString());
            o.PutUtfString("mSender", m.sender.ToString());
            o.PutInt("amount", 1);
            sfs.Send(new ExtensionRequest("Messages", o));
        }

        public SmartFox getConnection() {
            return sfs;
        }

        //exit handler
        void OnApplicationQuit() {
            logout();
            PolybiusManager.loggedIn = false;
            Debug.Log("exiting");
            sfs.RemoveAllEventListeners();
            if (sfs.IsConnected) {
                sfs.Disconnect();
            }
        }
    }
}