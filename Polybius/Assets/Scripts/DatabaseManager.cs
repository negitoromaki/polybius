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
        public string postJson(string method, WWWForm json, string url) {
            UnityWebRequest request;

            switch (method) {
                case "GET":
                    request = UnityWebRequest.Get(url);
                    break;

                case "POST":
                    request = UnityWebRequest.Post(url, json);
                    break;

                case "PUT":
                    request = UnityWebRequest.Put(url, json.data);
                    break;

                case "DELETE":
                    request = UnityWebRequest.Delete(url);
                    break;

                default:
                    Debug.LogError("Invalid HTTP Method: " + method);
                    return "{\"message\": \"Invalid HTTP Method\",\"success\": false}";
            }

            // Ensure headers for JSON data say JSON
            if (method == "POST" || method == "PUT") {
                request.SetRequestHeader("accept", "application/json; charset=UTF-8");
                request.SetRequestHeader("content-type", "application/json; charset=UTF-8");
            }
            request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log("Network Error: " + request.error);
                return "{\"message\": \"Network/HTTP Error\",\"success\": false}";
            } else {
                return request.downloadHandler.text;
            }
        }

        // login user
        public void login() {
            if (!PolybiusManager.loggedIn) {
                if (!string.IsNullOrEmpty(PolybiusManager.player.getPassword()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getUsername())) {

                    // Get user request
                    User u = new User();
					JsonUtility.FromJsonOverwrite(
                        postJson("GET", null, flaskIP + "/users?username=" + PolybiusManager.player.getUsername()), u);

					if (u.getPassword() == PolybiusManager.player.getPassword()) {
                        // Login successful, set player as returned user
                        PolybiusManager.player = u;	

                        // Set isOnline to true
                        WWWForm form = new WWWForm();
                        form.AddField("userID", PolybiusManager.player.getUserID());
                        form.AddField("isOnline", 1);

                        Debug.Log("Set user online: " + postJson("PUT", form, flaskIP + "/users"));
						PolybiusManager.loggedIn = true;
					}
                }
            }
        }

        // register user
        public void create() {
            if (!PolybiusManager.loggedIn) {
                if (!string.IsNullOrEmpty(PolybiusManager.player.getPassword()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getUsername()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getEmail())) {

                    // JSON
                    WWWForm form = new WWWForm();
                    form.AddField("username", PolybiusManager.player.getUsername());
                    form.AddField("password", PolybiusManager.player.getPassword());
                    form.AddField("email", PolybiusManager.player.getEmail());
                    form.AddField("dob", "");
                    form.AddField("privacy", 0);

					Debug.Log("Register: " + postJson("POST", form, flaskIP + "/users"));
                }
            }
        }

        public void logout() {
            if (!PolybiusManager.loggedIn) {
                // JSON
                WWWForm form = new WWWForm();
                form.AddField("userID", PolybiusManager.player.getUserID());
                form.AddField("isOnline", 0);

				Debug.Log("Logout: " + postJson("PUT", form, flaskIP + "/users"));	
				PolybiusManager.loggedIn = false;
            }
        }

		public User getUser(string username){
            // Get user request
            User u = new User();
            JsonUtility.FromJsonOverwrite(postJson("GET", null, flaskIP + "/users?username=" + username), u);

            if (!string.IsNullOrEmpty(u.getUsername())) {
				return u;
			} else {
				return null;
			}
		}

        public void sendMessageRequest(Message m) {
            // TODO: Flask send messages query
			User u = getUser(m.receiver);
			if (u != null) {
                // JSON
                WWWForm form = new WWWForm();
                form.AddField("receiverID", u.getUserID());
                form.AddField("senderID", PolybiusManager.player.getUserID());
                form.AddField("message", m.message);

                Debug.Log("Send Message: " + postJson("POST", form, flaskIP + "/messages"));
                PolybiusManager.sendNotification("Message sent", "Your message was sent successfully");
            } else {
                Debug.LogError("Could not send message, user null");
            }
        }

        public void sendFeedBack(string feedback, string subject) {
            // TODO: Flask send feedback query
        }

		public bool checkFriend(int friendID){
            string url =    flaskIP + "/friends?user1ID=" + PolybiusManager.player.getUserID() +
                            "&user2ID=" + friendID.ToString();
			string resp = postJson ("GET", null, url);

			return resp != null && resp.Contains("0");
        }

        public void AddFriend(string username, int friendID) {
			if (!checkFriend(friendID))
				return;

            // JSON
            WWWForm form = new WWWForm();
            form.AddField("user1ID", PolybiusManager.player.getUserID());
            form.AddField("user2ID", friendID);

            Debug.Log("Add Friend: " + postJson("POST", form, flaskIP + "/friends"));
        }

		public bool checkBlocked(int blockID){
            string url =    flaskIP + "/block?user1ID=" + PolybiusManager.player.getUserID() +
                            "&user2ID=" + blockID.ToString();
            string resp = postJson("GET", null, url);

            return resp != null && resp.Contains("0");
		}

        public void BlockPlayer(string username, int id) {
			if (!checkBlocked(id))
				return;

			User u = getUser(username);
			if (u != null) {
                // JSON
                WWWForm form = new WWWForm();
                form.AddField("blockerID", PolybiusManager.player.getUserID());
                form.AddField("blockedID", id);

				Debug.Log ("Blocked: " + postJson ("POST", form, flaskIP + "/block"));
			} else {
                Debug.LogError("Could not block player, other user null");
            }
        }

        public void reportUser(string username) {
			User u = getUser(username);
            if (u != null) {
                // JSON
                WWWForm form = new WWWForm();
                form.AddField("userID", u.getUserID());

                Debug.Log("Reported: " + postJson("POST", form, flaskIP + "/report"));
            } else {
                Debug.LogError("Could not report player, user null");
            }
        }

        public void RemoveFriend(string username, int id) {
			User u = getUser(username);
			if (u != null) {
                // JSON
                WWWForm form = new WWWForm();
                form.AddField("user1ID", PolybiusManager.player.getUserID());
                form.AddField("user2ID", u.getUserID());

                Debug.Log("Removed Friend: " + postJson("PUT", form, flaskIP + "/friend"));
            } else {
                Debug.LogError("Could not remove friend, user null");
            }
        }

        public void host(string rname, string gameType) {
            // TODO: Flask create lobby query
            PolybiusManager.currGame = new Game(rname, gameType, PolybiusManager.player, PolybiusManager.currLat, PolybiusManager.currLong);
        }

        public void getFriendsQuery() {
            // TODO: Flask get friends query
        }

        public void getBlockQuery() {
            // TODO: Flask block user query
        }

        public void setPrivacy(int userID, int privacy) {
            // JSON
            WWWForm form = new WWWForm();
            form.AddField("userID", userID);
            form.AddField("privacy", privacy);
        
			Debug.Log ("Privacy: " + postJson ("PUT", form, flaskIP + "/users"));
        }
        
        // JSON Responses with arrays

        // Helper JSON Wrapper
        private class Wrapper<T> {
            public T[] items;
        }

        public List<User> searchUsers(string search) {
            string url = flaskIP + "/users?search=" + search;
            string resp = postJson("GET", null, url);

            Wrapper<User> wrapper = JsonUtility.FromJson<Wrapper<User>>(resp);

            List<User> results = new List<User>();
            for (int i = 0; i < wrapper.items.Length; i++)
                results.Add(wrapper.items[i]);
            return results;
        }

        public List<User> getAllUsers() {
            string url = flaskIP + "/users";
            string resp = postJson("GET", null, url);

            Wrapper<User> wrapper = JsonUtility.FromJson<Wrapper<User>>(resp);

            List<User> results = new List<User>();
            for (int i = 0; i < wrapper.items.Length; i++)
                results.Add(wrapper.items[i]);
            return results;
        }

        public List<User> getFriends(int userID) {
            string url = flaskIP + "/friends?user1ID=" + userID.ToString();
            string resp = postJson("GET", null, url);

            Wrapper<User> wrapper = JsonUtility.FromJson<Wrapper<User>>(resp);

            List<User> results = new List<User>();
            for (int i = 0; i < wrapper.items.Length; i++)
                results.Add(wrapper.items[i]);
            return results;
        }

        public List<Message> getMessages(string senderUsername) {
            User u = getUser(senderUsername);
            List<Message> results = new List<Message>();

            if (u != null) {
                string url =    flaskIP + "/messages?user1ID=" + PolybiusManager.player.getUserID().ToString() +
                                "&user2ID=" + u.getUserID().ToString();
                string resp = postJson("GET", null, url);

                Wrapper<Message> wrapper = JsonUtility.FromJson<Wrapper<Message>>(resp);

                for (int i = 0; i < wrapper.items.Length; i++)
                    results.Add(wrapper.items[i]);
            } else {
                Debug.LogError("Could not get messages, other user is null");
            }

            return results;
        }

        public List<Game> getLobbies(Game.type gameType) {
            List<Game> lobbies = new List<Game>();

            string url = flaskIP + "/lobbies?gameType=" + gameType.ToString();
            string resp = postJson("GET", null, url);

            Wrapper<Game> wrapper = JsonUtility.FromJson<Wrapper<Game>>(resp);

            for (int i = 0; i < wrapper.items.Length; i++)
                lobbies.Add(wrapper.items[i]);

            return lobbies;
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
		private bool isHost = false;
		private string tmpRoomName = "";

        void Awake() {
            PolybiusManager.dm = this;
            sfs.AddEventListener(SFSEvent.LOGIN, onLogin);
            sfs.AddEventListener(SFSEvent.CONNECTION, onConnect);
            sfs.AddEventListener(SFSEvent.CONNECTION_LOST, onLost);
            sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, onResponse);
			sfs.AddEventListener(SFSEvent.ROOM_JOIN, onJoin);
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
            
			if (cmd.Equals ("Sync")) {
				onSync (paramsa);
			} else if (cmd.Equals ("ChangeHost") && paramsa.GetUtfString("result").Equals("success")) {
				onHostChange ();
			} else if (cmd.Equals ("ChangeHost") && paramsa.GetUtfString("result").Equals("last")) {
				SFSObject o = new SFSObject ();
				o.PutUtfString ("roomName", tmpRoomName);
				sfs.Send (new ExtensionRequest ("RemoveRoom", o));
			}else if (cmd.Equals ("BecameHost")) {
				becameHost ();
			} else if (cmd.Equals ("LeftRoom")) {
				onUserLeft (paramsa);
			} else if (cmd.Equals ("LeaveRoom")) {
				leaveRoomB ();
			} else if (cmd.Equals ("ReqData")) {
				sendRoomData (paramsa);
			} else if (cmd.Equals ("ReqDataRes")) {
				gotData (paramsa);
			} else if (cmd.Equals ("SetHost")) {
				becameHost ();
			} else if (cmd.Equals ("CreateRoom") && paramsa.GetUtfString ("result").Equals ("success")) {
				onCreate (tmpRoomName);
			} else if (cmd.Equals ("RemoveRoom") && paramsa.GetUtfString ("result").Equals ("success")) {
				leaveRoomB ();
			}

           
        }

        public SmartFox getConnection() {
            return sfs;
        }

		// ---------------
		// Host
		// ---------------
		public void hostLobby(string roomName){
			SFSObject o = new SFSObject ();
			o.PutUtfString ("roomName", roomName);
			sfs.Send (new ExtensionRequest ("CreateRoom",o));
		}

		void onCreate(string roomName){
			SFSObject o = new SFSObject ();
			o.PutUtfString ("roomName", roomName);
			sfs.Send(new ExtensionRequest("SetHost", o));
			isHost = true;


		}


		//TODO things when this user becomes host
		void becameHost(){
			isHost = true;
		}

		// ---------------
		// Join Room
		// ---------------
		public void joinRoom(string roomName){
			tmpRoomName = roomName;
			sfs.Send (new JoinRoomRequest (roomName));
		}



		void onJoin (BaseEvent e){
			//get initial data
			if (!isHost) {
				SFSObject o = new SFSObject ();
				o.PutUtfString ("roomName", tmpRoomName);
				sfs.Send (new ExtensionRequest ("ReqData", o));
			}

		}


		void gotData(SFSObject data){
			//TODO what to do with the data
		}

		// ---------------
		// Send data as host
		// ---------------
		void sendRoomData(SFSObject o){
			//TODO edit as needed
			SFSObject data = new SFSObject();


			o.PutSFSObject ("data", data);
			if (isHost) {

				sfs.Send (new ExtensionRequest ("SendData", o));
			}


		}

		// ---------------
		// sync data
		// ---------------
		public void syncActions(){
			SFSObject syncData = new SFSObject ();

			sfs.Send (new ExtensionRequest ("Sync", syncData));


		}

		void onSync(SFSObject data){
			//data was recieved
		}

		// ---------------
		// leave room
		// ---------------
		public void leaveRoom(string roomName){

			if (isHost) {
				//change host before leaving
				SFSObject o = new SFSObject();
				o.PutUtfString ("roomName", roomName);
				tmpRoomName = roomName;
				sfs.Send (new ExtensionRequest ("ChangeHost", o));
			} else {
				leaveRoomB ();
			}

		}

		void onHostChange(){
			broadCastLeave ();
		}

		void broadCastLeave(){
			SFSObject o = new SFSObject ();
			o.PutUtfString ("roomName", tmpRoomName);
			sfs.Send (new ExtensionRequest ("LeaveRoom", o));
			leaveRoomB ();
		}

		void leaveRoomB(){
			sfs.Send (new LeaveRoomRequest ());
		}



		void onLeaveRoom(BaseEvent e){
			
			tmpRoomName = "Lobby";
			sfs.Send (new JoinRoomRequest ("Lobby"));
		}

		//other user left
		void onUserLeft(SFSObject o){

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
