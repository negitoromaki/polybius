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

        public void host(string rname, Game.type gameType) {
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
