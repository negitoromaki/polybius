﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
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


        void Awake() {
            PolybiusManager.dm = this;
            sfs.AddEventListener(SFSEvent.LOGIN, onLogin);
            sfs.AddEventListener(SFSEvent.CONNECTION, onConnect);
            sfs.AddEventListener(SFSEvent.CONNECTION_LOST, onLost);
            sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, onResponse);
            sfs.ThreadSafeMode = true;

            sfs.Connect(ip, port);

            // Messages Shim
            for (int i = 0; i < 5; i++)
                PolybiusManager.player.sendMessage(new Message(-2, -1, System.DateTime.Now, "This is message number " + i));
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
            Debug.LogError("Lost Server!");
        }

        void onLogin(BaseEvent e) {
            connected = true;
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
                    logout();
                }
            } else if (cmd == "CreateUser") {
                if (result == "success") {
                    Debug.Log("Register successful!");
                    // TODO: Get User ID and fill into PolybiusManager.player.userID
                } else {
                    Debug.LogError("Error with registration: " + result);
                }
            } else if (cmd == "Messages") {
                if (result == "success") {
                    Debug.Log("Message successful!");
                    // TODO
                    // Message m = new Message(int senderID, PolybiusManager.player.getUserID(), System.DateTime timestamp, string message)
                    // PolybiusManager.player.addMessage(m);
                } else {
                    Debug.LogError("Error with Message: " + result);
                }
            } else if (cmd == "UserLogout") {
                if (result == "success") {
                    PolybiusManager.loggedIn = false;
                    Debug.Log("Logout successful!");
                } else {
                    Debug.LogError("Error with Logout: " + result);
                }
            } else {
                Debug.LogError("Command Not found: " + cmd + " returned " + result);
            }
        }

        //public methods
        // login user
        public void login() {
            if (!PolybiusManager.loggedIn) {
                if (!string.IsNullOrEmpty(PolybiusManager.player.getPassword()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getUsername())) {

                    ISFSObject l = new SFSObject();
                    l.PutUtfString("username", PolybiusManager.player.getUsername());
                    l.PutUtfString("password", PolybiusManager.player.getPassword());
                    sfs.Send(new ExtensionRequest("UserLogin", l));
                }
            } else {
                logout();
            }
        }

        // register user
        public void create() {
            if (!PolybiusManager.loggedIn) {
                if (!string.IsNullOrEmpty(PolybiusManager.player.getPassword()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getUsername()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getEmail())) {

                    ISFSObject o = new SFSObject();
                    o.PutUtfString("username", PolybiusManager.player.getUsername());
                    o.PutUtfString("password", PolybiusManager.player.getPassword());
                    o.PutUtfString("email", PolybiusManager.player.getEmail());
                    //o.PutUtfString("dob", PolybiusManager.player.dob); // TODO: add DOB functionality
                    sfs.Send(new ExtensionRequest("CreateUser", o));
                }
            } else {
                logout();
            }
        }
        public void logout() {
            if (!string.IsNullOrEmpty(PolybiusManager.player.getUsername())) {
                ISFSObject ot = new SFSObject();
                ot.PutUtfString("username", PolybiusManager.player.getUsername());
                Debug.Log("Logging out " + PolybiusManager.player.getUsername());
                sfs.Send(new ExtensionRequest("UserLogout", ot));
            }
        }

        // get message
        public void getMessagesRequest(int senderID) {
            ISFSObject o = new SFSObject();
            o.PutUtfString("level", "private");
            o.PutUtfString("levelname", "Polybius");
            o.PutUtfString("mode", "get");
            o.PutUtfString("mReceiver", PolybiusManager.player.getUserID().ToString());
            o.PutUtfString("mSender", senderID.ToString());
            o.PutInt("amount", 1);
            sfs.Send(new ExtensionRequest("Messages", o));
        }

        // send message
        public void sendMessageRequest(Message m) {
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

        // Get lobbies
        public List<Game> getLobbies(Game.type type) {
            List<Game> games = new List<Game>();
            // TODO: get lobbies from database and add to list
            for (int i = 0; i < 10; i++)
                games.Add(new Game(1000 * i, 1000 * i, PolybiusManager.player, Game.type.pong));
            return games;
        }

        // Get Friends
        public List<User> updateFriends() {
            List<User> friends = new List<User>();
            // TODO: get friends from database and add to list
            return friends;
        }

        // Search Users
        public List<User> userSearch(string username) {
            // TODO: search for users by username

            List<User> users = new List<User>();
            // Debug
            for (int i = 0; i < 5; i++) {
                string name = username + i;
                users.Add(new User(name, "bobrocks", "bob@bob.com", "10/10/1901"));
            }
            return users;
        }

        //exit handler
        void OnApplicationQuit() {
            for (int i = 0; i < 10 && PolybiusManager.loggedIn; i++) {
                logout();
                Thread.Sleep(1);
            }
            if (PolybiusManager.loggedIn)
                Debug.LogError("Could not log out!!!");
            Debug.Log("exiting");
            sfs.RemoveAllEventListeners();
            if (sfs.IsConnected) {
                sfs.Disconnect();
            }
            
        }
    }
}