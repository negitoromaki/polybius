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
        public string result = "None";

        void Awake() {
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
            SFSObject paramsa = (SFSObject)e.Params["params"]; // changed ISFSObject to SFSObject, could be errors
            string cmd2 = paramsa.GetUtfString("cmd");
            string message = cmd + " " + paramsa.GetUtfString("result") + " message: " + paramsa.GetUtfString("message");
            Debug.Log(cmd + "/" + cmd2 + " message: " + message);
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
            }
            else if (cmd == "setPrivate")
            {
                if (result == "success")
                {
                    Debug.Log("Privacy set");
                }
                else
                {
                    Debug.LogError("Error with setting privacy: " + result);
                }
            }
            else if (cmd == "Messages") {
                if (result == "success") {
                    SFSArray messages = (SFSArray) paramsa.GetSFSArray("messages");

                    for (int i = 0; i < messages.Size(); i++) {
                        SFSObject messageObj = (SFSObject)messages.GetSFSObject(i);
                        Message m = new Message(messageObj.GetUtfString("sender"),
                                                PolybiusManager.player.getUsername(),
                                                System.DateTime.Now,
                                                messageObj.GetUtfString("message"));
                        	PolybiusManager.player.addMessage(m);
                    }
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
            } else if (cmd2 == "getFriends") {
                if (result == "success") {
                    SFSArray returnedList = (SFSArray)paramsa.GetSFSArray("friends");
                    PolybiusManager.player.friends.Clear();
                    for (int i = 0; i < returnedList.Size(); i++) {
                        SFSObject currentFriend = (SFSObject)returnedList.GetSFSObject(i);
                        User friendObj = new User(currentFriend.GetUtfString("username"),
                                                    null,
                                                    null,
                                                    null,
                                                    currentFriend.GetInt("id"),
                                                    currentFriend.GetInt("private"));
                        if (!string.IsNullOrEmpty(friendObj.getUsername()))
                            PolybiusManager.player.friends.Add(friendObj);
                    }
                    PolybiusManager.mutex = false;
                } else {
                    Debug.LogError("Error retrieving friend list: " + result);
                }
            } else if (cmd2 == "addFriend") {
                if (result == "success") {
                    Debug.Log("Successfully added friend!");
                } else {
                    Debug.LogError("Error adding friend: " + result);
                }
            } else if (cmd2 == "removeFriend") {
                if (result == "success") {
                    Debug.Log("Successfully removed friend!");
                } else {
                    Debug.LogError("Error removing friend: " + result);
                }
            } else if (cmd2 == "getUsers") {
                SFSArray returnedList = (SFSArray) paramsa.GetSFSArray("users");
                PolybiusManager.results.Clear();
                for (int i = 0; i < returnedList.Size(); i++) {
                    SFSObject currentUser = (SFSObject) returnedList.GetSFSObject(i);
                    PolybiusManager.results.Add(new User(   currentUser.GetUtfString("username"),
                                                            null,
                                                            null,
                                                            null,
                                                            currentUser.GetInt("id"),
                                                            currentUser.GetInt("private")));
                }
                PolybiusManager.mutex = false;
            }
            else if (cmd2 == "getRooms")
            {
                SFSArray roomData = (SFSArray)paramsa.GetSFSArray("Lobby");
                getLobbies(roomData);
            }
            else {
                Debug.LogError("Command Not found: " + cmd + " returned " + result + "\nCommand2 Not found: " + cmd2);
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
					sfs.Send(new LoginRequest(PolybiusManager.player.getUsername(), "", initZone));
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
        public void getMessagesRequest(string senderUsername) {
            ISFSObject o = new SFSObject();
            o.PutUtfString("level", "private");
            o.PutUtfString("levelname", "none");
            o.PutUtfString("mode", "get");
            o.PutUtfString("receiver", PolybiusManager.player.getUsername());
            o.PutUtfString("sender", senderUsername);
            o.PutInt("amount", 1);
			o.PutUtfString ("message", "none");
            sfs.Send(new ExtensionRequest("Messages", o));
        }

        // send message
        public void sendMessageRequest(Message m) {
            ISFSObject o = new SFSObject();
            o.PutUtfString("level", "private");
            o.PutUtfString("levelname", "none");
            o.PutUtfString("mode", "send");
            o.PutUtfString("receiver", m.receiver);
            o.PutUtfString("sender", m.sender);
            o.PutUtfString("message", m.message);
			o.PutInt ("amount", 1);
            sfs.Send(new ExtensionRequest("Messages", o));
            PolybiusManager.sendNotification("Message sent", "Your message was sent successfully");
        }

        public SmartFox getConnection() {
            return sfs;
        }

        public void getLobbiesQuery() {
            // query server for lobbies
            ISFSObject o = new SFSObject();
            o.PutUtfString("cmd", "getRooms");
            sfs.Send(new ExtensionRequest("Lobby", o));
        }

		public void AddFriend(string username, int id) {
            ISFSObject o = new SFSObject();
            o.PutUtfString("cmd", "addFriend");
            o.PutUtfString("username", PolybiusManager.player.getUsername());
            o.PutInt("id",id);
            sfs.Send(new ExtensionRequest("FriendList", o));
        }

		public void RemoveFriend(string username, int id) {
            ISFSObject o = new SFSObject();
            o.PutUtfString("cmd", "removeFriend");
            o.PutUtfString("username", PolybiusManager.player.getUsername());
            o.PutInt("id",id);
            sfs.Send(new ExtensionRequest("FriendList", o));
        }

        // Get lobbies
        public List<Game> getLobbies(SFSArray roomData) {
            // Use getLobbiesQuery to query the server
            List<Game> games = new List<Game>();
            // TODO: Parse room data
            return games;
        }

        public void getFriendsQuery() {
            ISFSObject o = new SFSObject();
            o.PutUtfString("cmd", "getFriends");
            o.PutUtfString("username", PolybiusManager.player.getUsername());
            o.PutInt("id",-1);
            sfs.Send(new ExtensionRequest("FriendList", o));
        }

        public void setPrivacy(string username, int privacy) {
            // TODO: toggle database user privacy
            ISFSObject o = new SFSObject();
            o.PutUtfString("cmd", "setPrivate");
            o.PutUtfString("username", username);
            o.PutInt("private", privacy);
            sfs.Send(new ExtensionRequest("Users", o));
        }

        public void getUsersQuery() {
            ISFSObject o = new SFSObject();
            o.PutUtfString("cmd", "getUsers");
            sfs.Send(new ExtensionRequest("Users", o));
        }

        //exit handler
        void OnDisable() {
            logout();
            Debug.Log("exiting");
            sfs.RemoveAllEventListeners();
            if (sfs.IsConnected) {
                sfs.Disconnect();
            }
        }
    }
}
