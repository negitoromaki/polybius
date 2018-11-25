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
            //byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);

            switch (method) {
                case "GET":
                    request = UnityWebRequest.Get(url);
                    break;

                case "POST":
                    request = UnityWebRequest.Post(url, json);
                    request.SetRequestHeader("accept", "application/json; charset=UTF-8");
                    request.SetRequestHeader("content-type", "application/json; charset=UTF-8");
                    break;

                case "PATCH":
                    // I don't think we ever use patch

                case "PUT":
                    request = UnityWebRequest.Put(url, json.data);
                    request.SetRequestHeader("accept", "application/json; charset=UTF-8");
                    request.SetRequestHeader("content-type", "application/json; charset=UTF-8");
                    break;

                case "DELETE":
                    request = UnityWebRequest.Delete(url);
                    request.SetRequestHeader("accept", "application/json; charset=UTF-8");
                    request.SetRequestHeader("content-type", "application/json; charset=UTF-8");
                    break;

                default:
                    Debug.LogError("Invalid HTTP Method: " + method);
                    return "{\"message\": \"Invalid HTTP Method\",\"success\": false}";
            }

            request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log(request.error);
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

					string json = "{ \"username\": \"" + PolybiusManager.player.getUsername() + "\"}";	                    
					User u = new User();	                    
					JsonUtility.FromJsonOverwrite(postJson("GET", json, flaskIP + "/users"), u);

					if (u.getPassword() == PolybiusManager.player.getPassword()) {	
						PolybiusManager.player = u;	
						// Set isOnline to true	
						json =   "{ \"userID\": \"" + PolybiusManager.player.getUserID() + "\", \"isOnline\": 1}";	
						u = new User();	
						JsonUtility.FromJsonOverwrite(postJson("PUT", json, flaskIP + "/users"), u);	
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

					string json = "{ \"username\":  \"" + PolybiusManager.player.getUsername()  + "\", " +	
						"\"password\":  \"" + PolybiusManager.player.getPassword()  + "\", " +	
						"\"email\":     \"" + PolybiusManager.player.getEmail()     + "\", " +	
						"\"dob\": \"\", " + "\"privacy\": 0, " + "}";	
					Debug.Log("Register: " + postJson("GET", json, flaskIP + "/users"));
                }
            }
        }

        public void logout() {
            if (!PolybiusManager.loggedIn) {
				string json = "{ \"userID\": \"" + PolybiusManager.player.getUserID() + "\", \"isOnline\": 0}";	
				Debug.Log("Logout: " + postJson("PUT", json, flaskIP + "/users"));	
				PolybiusManager.loggedIn = false;
            }
        }

		public User getUser(string username){
			User u = new User();
			string json="{\"username\":\""+ username+"\"}";
			JsonUtility.FromJsonOverwrite(postJson("GET", json, flaskIP + "/users"), u);
			if (!string.IsNullOrEmpty(u.getUsername())) {
				return u;
			} else {
				return null;
			}
				
		}
        // get message
        public void getMessagesRequest(string senderUsername) {
            // TODO: Flask get messages query
			User u;
			string json;
			if ((u=getUser(senderUsername)) != null) {
				json = "{ \"senderID\": \"" + u.getUserID() + "\", \"receiverID\":\"" + PolybiusManager.player.getUserID() + " }";	
			}

        }

        // send message
        public void sendMessageRequest(Message m) {
            // TODO: Flask send messages query
			User u;
			string json=null;
			if ((u=getUser(m.receiver)) != null) {
				json = "{ \"receiverID\": \"" + u.getUserID() + "\", \"senderID\":\"" + PolybiusManager.player.getUserID() + ",\"message\":\""+m.message+"\"}";	
			}
			Debug.Log("Send Message: " + postJson("POST", json, flaskIP + "/messages"));	

            PolybiusManager.sendNotification("Message sent", "Your message was sent successfully");
        }

        public void sendFeedBack(string feedback, string subject) {
            // TODO: Flask send feedback query
        }

        public void getLobbiesQuery() {
            // TODO: Flask get lobbies query
			//string json = "{ \"gameType\": \"" + gameType + "\"}";	


        }

		public bool checkFriend(int friendid){
			string json="{\"user1ID\":"+ PolybiusManager.player.getUserID()+",\"user2ID\":"+friendid+"}";
			string js = postJson ("GET", json, flaskIP + "/friends");
			if (js != null && js.Contains ("0")) {
				return true;
			} else {
				return false;
			}


		}

        public void AddFriend(string username, int friendid) {

			if (!checkFriend (friendid)) {
				return;
			}
			User u = new User();

			string json="{\"user1ID\":"+ PolybiusManager.player.getUserID()+",\"user2ID\":"+friendid+"}";
			Debug.Log("Add Friend: " + postJson("POST", json, flaskIP + "/friends"));	

        }
		public bool checkBlocked(int blockID){
			string json="{\"user1ID\":"+ PolybiusManager.player.getUserID()+",\"user2ID\":"+blockID+"}";
			string js = postJson ("GET", json, flaskIP + "/block");
			if (js != null && js.Contains ("0")) {
				return true;
			} else {
				return false;
			}


		}
        public void BlockPlayer(string username, int id) {
			if (!checkBlocked (id)) {
				return;
			}
			User u;
			if ((u = getUser (username)) != null) {
				string json = "{\"blocker\":" + PolybiusManager.player.getUserID() + ",\"blocked\":" + id + "}";
				Debug.Log ("Blocked: " + postJson ("POST", json, flaskIP + "/block"));
			}

        }
        public void ReportPlayer(string username, int id, string reason) {
			User u;
			if ((u = getUser (username)) != null) {
				string json = "{\"userID\":" + u.getUserID()+"}";
				Debug.Log ("Reported: " + postJson ("POST", json, flaskIP + "/report"));
			}
        }
        public void RemoveFriend(string username, int id) {
            // TODO: Flask remove friend query
			User u;
			if ((u = getUser (username)) != null) {
				// TODO: Add Removeable Friends

			}
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
			string json = "{\"userID\":" + userID+", \"privacy\":"+privacy+"}";
			Debug.Log ("Privacy: " + postJson ("PUT", json, flaskIP + "/users"));

        }

        public void searchUsers(string search) {
            // TODO: Flask search users query
        }

        public void getUsersQuery() {
            // TODO: Flask get users query
			string json = "{ \"userID\": \"" + PolybiusManager.player.getUserID() + "\"}";
			postJson ("GET", json, flaskIP + "/users");
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
