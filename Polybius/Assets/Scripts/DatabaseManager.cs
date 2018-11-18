using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
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

        // ------------
        // Flask Server
        // ------------

        private string flaskIP = "http://128.211.240.229:5000";

        private string postJson(string method, string jsonStr, string url) {
            UnityWebRequest request;
            byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);
            request = UnityWebRequest.Put(url, bytes);

            switch (method) {
                case "POST":
                case "PATCH":
                case "PUT":
                    request.SetRequestHeader("X-HTTP-Method-Override", method);
                    break;

                case "GET":
                    request = UnityWebRequest.Get(url);
                    break;

                case "DELETE":
                    request = UnityWebRequest.Delete(url);
                    break;

                default:
                    Debug.LogError("Invalid HTTP Method");
                    return "";
            }

            request.SetRequestHeader("accept", "application/json; charset=UTF-8");
            request.SetRequestHeader("content-type", "application/json; charset=UTF-8");
            request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log(request.error);
                return "";
            } else {
                return request.downloadHandler.text;
            }
        }

        // login user
        class loginJson {
            public string username;
            public string password;
        }

        public void login() {
			Debug.Log ("here");
            if (!PolybiusManager.loggedIn) {
                if (!string.IsNullOrEmpty(PolybiusManager.player.getPassword()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getUsername())) {

                    // Create JSON class
                    loginJson l = new loginJson();
                    l.username = PolybiusManager.player.getUsername();

                    // Call post method
                    string json = JsonUtility.ToJson(l);
                    JsonUtility.FromJsonOverwrite(postJson("GET", json, flaskIP + "/users"), l);
                    if (l.password == PolybiusManager.player.getPassword()) {
                        // TODO: Flask send login request
                    }

					//smartfox
					SFSObject logdata = new SFSObject();
					logdata.PutUtfString ("username", PolybiusManager.player.getUsername ());
					logdata.PutUtfString ("password", PolybiusManager.player.getPassword ());
					sfs.Send (new ExtensionRequest ("UserLogin", logdata));


                }
            }
        }

        // register user
        public void create() {
            if (!PolybiusManager.loggedIn) {
                if (!string.IsNullOrEmpty(PolybiusManager.player.getPassword()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getUsername()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getEmail())) {

                    // TODO: Flask register query
                }
            }
        }

        public void logout() {
            if (!PolybiusManager.loggedIn) {
                // TODO: Flask logout query
            }
        }

        // get message
        public void getMessagesRequest(string senderUsername) {
            // TODO: Flask get messages query
        }

        // send message
        public void sendMessageRequest(Message m) {
            // TODO: Flask send messages query
            PolybiusManager.sendNotification("Message sent", "Your message was sent successfully");
        }

        public void sendFeedBack(string feedback, string subject) {
            // TODO: Flask send feedback query
        }

        public void getLobbiesQuery() {
            // TODO: Flask get lobbies query
        }

        public void AddFriend(string username, int id) {
            // TODO: Flask add friend query
        }

        public void BlockPlayer(string username, int id) {
            // TODO: Flask block player query

        }
        public void ReportPlayer(string username, int id, string reason) {
            // TODO: Flask report player query
        }
        public void RemoveFriend(string username, int id) {
            // TODO: Flask remove friend query
        }

        public void host(string name, Game.type gameType) {
            // TODO: Flask create lobby query
            PolybiusManager.currGame = new Game(roomName, gameType, PolybiusManager.player, PolybiusManager.currLat, PolybiusManager.currLong);
        }

        public void getFriendsQuery() {
            // TODO: Flask get friends query
        }
        public void getBlockQuery() {
            // TODO: Flask block user query
        }

        public void setPrivacy(int userID, int privacy) {
            // TODO: Flask set privacy query
        }

        public void searchUsers(string search) {
            // TODO: Flask search users query
        }

        public void getUsersQuery() {
            // TODO: Flask get users query
        }

        public void reportUserQuery(string u) {
            // TODO: Flask report users query
        }

        // ---------------
        // SmartFox Server
        // ---------------

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
            result = paramsa.GetUtfString("result");

            if (cmd == "UserLogin")
            {
                if (result == "success")
                {
                    // login successful
                    Debug.Log("Login successful!");
                    PolybiusManager.loggedIn = true;
                }
                else
                {
                    Debug.LogError("Error with login: " + result);
                    logout();
                }
            }
            else if (cmd2 == "sendFeedback")
            {
                if (result == "success")
                {
                    PolybiusManager.sendNotification("Thank you!", "Your feedback is important to use and we work to improve Polybius with your help!");
                }
                else
                {
                    Debug.LogError("Error with feedback: " + result);
                }
            }
            else if (cmd2 == "blockPlayer")
            {
                if (result == "success")
                {
                    Debug.Log("Block Success");
                }
                else
                {
                    Debug.LogError("Error with blocking: " + result);
                }
            }
            else if (cmd2 == "reportPlayer")
            {
                if (result == "success")
                {
                    Debug.Log("Report Success");
                }
                else
                {
                    Debug.LogError("Error with reporting: " + result);
                }
            }
            else if (cmd == "CreateUser")
            {
                if (result == "success")
                {
                    Debug.Log("Register successful!");
                    // TODO: Get User ID and fill into PolybiusManager.player.userID
                }
                else
                {
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
            else if (cmd == "Messages")
            {
                if (result == "success")
                {
                    SFSArray messages = (SFSArray)paramsa.GetSFSArray("messages");

                    for (int i = 0; i < messages.Size(); i++)
                    {
                        SFSObject messageObj = (SFSObject)messages.GetSFSObject(i);
                        Message m = new Message(messageObj.GetUtfString("sender"),
                                                PolybiusManager.player.getUsername(),
                                                System.DateTime.Now,
                                                messageObj.GetUtfString("message"));
                        PolybiusManager.player.addMessage(m);
                    }
                }
                else
                {
                    Debug.LogError("Error with Message: " + cmd2 + ", Message: " + message + ", Result: " + result);
                }
            }
            else if (cmd == "UserLogout")
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
            else if (cmd2 == "host")
            {
                if (result == "success")
                {
                    Debug.Log("Host success, switching to scene...");
                    PolybiusManager.games.Add(PolybiusManager.currGame);
                    if (PolybiusManager.currGame.gameType == Game.type.none)
                    {
                        Debug.Log("Tried to start game with type NONE");
                    }
                    else if (PolybiusManager.currGame.gameType == Game.type.pong)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Pong");
                    }
                    else if (PolybiusManager.currGame.gameType == Game.type.connect4)
                    {
                        //UnityEngine.SceneManagement.SceneManager.LoadScene("Connect4");
                    }
                    else if (PolybiusManager.currGame.gameType == Game.type.tictactoe)
                    {
                        //UnityEngine.SceneManagement.SceneManager.LoadScene("TicTacToe");
                    }
                    else
                    {
                        Debug.LogError("Gametype not found: " + PolybiusManager.currGame.gameType);
                    }
                }
                else
                {
                    PolybiusManager.currGame = null;
                    Debug.LogError("Error hosting game: " + result);
                }
            }
            else if (cmd2 == "join")
            {
                if (result == "success")
                {
                    if (PolybiusManager.currGame.gameType == Game.type.none)
                    {
                        Debug.Log("Tried to start game with type NONE");
                    }
                    else if (PolybiusManager.currGame.gameType == Game.type.pong)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Pong");
                    }
                    else if (PolybiusManager.currGame.gameType == Game.type.connect4)
                    {
                        //UnityEngine.SceneManagement.SceneManager.LoadScene("Connect4");
                    }
                    else if (PolybiusManager.currGame.gameType == Game.type.tictactoe)
                    {
                        //UnityEngine.SceneManagement.SceneManager.LoadScene("TicTacToe");
                    }
                    else
                    {
                        Debug.LogError("Gametype not found: " + PolybiusManager.currGame.gameType);
                    }
                }
                else
                {
                    Debug.LogError("Error joining game: " + result);
                }
            }
            else if (cmd2 == "getFriends")
            {
                if (result == "success")
                {
                    SFSArray returnedList = (SFSArray)paramsa.GetSFSArray("friends");
                    PolybiusManager.player.friends.Clear();
                    for (int i = 0; i < returnedList.Size(); i++)
                    {
                        SFSObject currentFriend = (SFSObject)returnedList.GetSFSObject(i);
                        User friendObj = new User(currentFriend.GetUtfString("username"),
                                                    null,
                                                    null,
                                                    null,
                                                    currentFriend.GetInt("id"),
                                                    0);
                        if (!string.IsNullOrEmpty(friendObj.getUsername()))
                            PolybiusManager.player.friends.Add(friendObj);
                    }
                    PolybiusManager.mutex = false;
                }
                else
                {
                    Debug.LogError("Error retrieving friend list: " + result);
                }
            }
            else if (cmd2 == "getBlocked")
            {
                if (result == "success")
                {
                    SFSArray returnedList = (SFSArray)paramsa.GetSFSArray("blocklist");
                    PolybiusManager.player.blockedUsers.Clear();
                    for (int i = 0; i < returnedList.Size(); i++)
                    {
                        SFSObject blocked = (SFSObject)returnedList.GetSFSObject(i);
                        if (!string.IsNullOrEmpty(blocked.GetUtfString("username")))
                            PolybiusManager.player.blockedUsers.Add(blocked.GetUtfString("username"));
                    }
                    PolybiusManager.mutex = false;
                }
                else
                {
                    Debug.LogError("Error retrieving block list: " + result);
                }
            }
            else if (cmd2 == "addFriend")
            {
                if (result == "success")
                {
                    Debug.Log("Successfully added friend!");
                }
                else
                {
                    Debug.LogError("Error adding friend: " + result);
                }
            }
            else if (cmd2 == "removeFriend")
            {
                if (result == "success")
                {
                    Debug.Log("Successfully removed friend!");
                }
                else
                {
                    Debug.LogError("Error removing friend: " + result);
                }
            }
            else if (cmd2 == "getUsers")
            {
                SFSArray returnedList = (SFSArray)paramsa.GetSFSArray("users");
                PolybiusManager.results.Clear();
                for (int i = 0; i < returnedList.Size(); i++)
                {
                    SFSObject currentUser = (SFSObject)returnedList.GetSFSObject(i);
                    PolybiusManager.results.Add(new User(currentUser.GetUtfString("username"),
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
                PolybiusManager.games.Clear();
                SFSArray roomData = (SFSArray)paramsa.GetSFSArray("roomdata");
                /*
                for (int i = 0; i < roomData.Size(); i++) {
                    SFSObject currentRoom = (SFSObject)roomData.GetSFSObject(i);
                    Debug.Log(currentRoom.GetUtfString("roomname") + ", " + currentRoom.GetUtfString("gameType"));
                    PolybiusManager.games.Add(new Game(currentRoom.GetUtfString("roomname"),
                                                        Game.stringToGameType(currentRoom.GetUtfString("gameType")),
                                                        null,
                                                        (float) currentRoom.GetDouble("latcord"),
                                                        (float) currentRoom.GetDouble("longcord")));
                }
                */

                // Debug
                for (int i = 0; i < (int)Random.Range(0f, 5f); i++)
                {
                    PolybiusManager.games.Add(new Game(i.ToString(),
                                                        Game.type.pong,
                                                        PolybiusManager.player,
                                                        40.426733f,
                                                        -86.916391f));
                }
                PolybiusManager.mutex = false;
            }
            else
            {
                Debug.LogError("cmd: " + cmd + ", result: " + result + ", message: " + message + "\ncmd2: " + cmd2);
            }
        }

        public SmartFox getConnection() {
            return sfs;
        }

        public void hostQuery(int lobbyID, Game.type gameType) {
            // TODO: Fix SmartFox server lobby
            ISFSObject o = new SFSObject();
            o.PutUtfString("cmd", "host");
            o.PutUtfString("roomname", lobbyID.ToString());
            sfs.Send(new ExtensionRequest("Lobby", o));
        }

        public void joinQuery(int lobbyID) {
            // query to join a lobby/room
            ISFSObject o = new SFSObject();
            o.PutUtfString("cmd", "join");
            o.PutUtfString("roomname", lobbyID.ToString());
            sfs.Send(new ExtensionRequest("Lobby", o));

            // TODO: Flask update currLobbyID
        }

        public void leaveQuery(string roomName) {
            ISFSObject o = new SFSObject();
            o.PutUtfString("cmd", "leave");
            o.PutUtfString("roomname", roomName);
            sfs.Send(new ExtensionRequest("Lobby", o));

            // TODO: Flask update currLobbyID
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
