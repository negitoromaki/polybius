// System
using System.Collections;
using System.Collections.Generic;
using System.Text;

// Unity
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// REST API
using Proyecto26;

// Smartfox
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;

namespace polybius {
    public class DatabaseManager : MonoBehaviour {

        // ------------
        // Flask Server
        // ------------

        private string flaskIP = "http://128.211.240.229:5000";

        [Serializable]
        public class ServerResponse {
            public bool success;
            public string message;
        }

        // login user
        public void login() {
            if (!PolybiusManager.loggedIn) {
                if (!string.IsNullOrEmpty(PolybiusManager.player.getPassword()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getUsername())) {

                    // REST: Get array of users with username
                    string url = flaskIP + "/users?username=" + PolybiusManager.player.getUsername();

                    RestClient.GetArray<User>(url).Then(respArray => {
                        if (respArray.Length > 0 &&
                            respArray[0].getPassword() == PolybiusManager.player.getPassword()) {
                            // Login successful, set player as returned user
                            PolybiusManager.player = respArray[0];
                            Debug.Log("Login successfull");

                            // Set isOnline to true
                            setOnline(true);
                        } else {
                            Debug.LogError("Login unsuccessful");
                        }
                    }).Catch(err => {
                        Debug.LogError("Error: " + err.Message);
                    });
                }
            }
        }

        // register user
        public void create() {
            if (!PolybiusManager.loggedIn) {
                if (!string.IsNullOrEmpty(PolybiusManager.player.getPassword()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getUsername()) &&
                    !string.IsNullOrEmpty(PolybiusManager.player.getEmail())) {

                    // REST: Create user
                    RestClient.Post<ServerResponse>(flaskIP + "/users", PolybiusManager.player).Then(resp => {
                        if (resp.success) {
                            PolybiusManager.loggedIn = true;
                            Debug.Log("Successfully created and logged in user");
                        } else {
                            Debug.LogError("Could not create user: " + resp.message);
                        }
                    });
                }
            }
        }

        public void logout() {
            if (!PolybiusManager.loggedIn) {
                setOnline(false);
            }
        }

		public User getUser(string username){
            User u = null;

            // REST: Get array of users with username
            string url = flaskIP + "/users?username=" + username;
            RestClient.GetArray<User>(url).Then(respArray => {
                    u = respArray[0];
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });

            return u;
        }

        [Serializable]
        public class ServerMessage {
            public int receiverID, senderID;
            public string message;

            public ServerMessage(int rID, int sID, string m) {
                receiverID = rID;
                senderID = sID;
                message = m;
            }
        }

        public void sendMessageRequest(Message m) {
            // JSON
            ServerMessage s = new ServerMessage(m.receiver.getUserID(), m.sender.getUserID(), m.message);

            // REST: Send message
            RestClient.Post<ServerResponse>(flaskIP + "/messages", s).Then(resp => {
                if (resp.success) {
                    Debug.Log("Successfully sent message");
                    PolybiusManager.sendNotification("Message sent", "Your message was sent successfully");
                } else {
                    Debug.LogError("Could not send message: " + resp.message);
                }
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });
        }

        public void sendFeedBack(string feedback, string subject) {
            // TODO: Flask send feedback query
        }

        public bool checkFriend(User friend){
            string url =    flaskIP + "/friends?user1ID=" + PolybiusManager.player.getUserID() +
                            "&user2ID=" + friend.getUserID().ToString();

            bool toReturn = false;

            // REST: Check friend
            RestClient.Get(url).Then(resp => {
                if (!resp.Text.Contains("0"))
                    toReturn = true;
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });

            return toReturn;
        }

        [Serializable]
        public class ServerFriend {
            public int user1ID, user2ID;

            public ServerFriend(int u1ID, int u2ID) {
                user1ID = u1ID;
                user2ID = u2ID;
            }
        }

        public void AddFriend(User friend) {
			if (!checkFriend(friend))
				return;

            // JSON
            ServerFriend f = new ServerFriend(PolybiusManager.player.getUserID(), friend.getUserID());

            // REST: Add friend
            RestClient.Post<ServerResponse>(flaskIP + "/friends", f).Then(resp => {
                if (resp.success) {
                    Debug.Log("Successfully added friend");
                } else {
                    Debug.LogError("Could not add friend: " + resp.message);
                }
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });
        }

		public bool checkBlocked(int blockID){
            string url =    flaskIP + "/block?user1ID=" + PolybiusManager.player.getUserID() +
                            "&user2ID=" + blockID.ToString();

            bool toReturn = false;

            // REST: Check blocked
            RestClient.Get(url).Then(resp => {
                if (!resp.Text.Contains("0"))
                    toReturn = true;
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });

            return toReturn;
        }

        public void BlockPlayer(User user) {
			if (!checkBlocked(user.getUserID()))
				return;

            // JSON
            ServerFriend f = new ServerFriend(PolybiusManager.player.getUserID(), user.getUserID());

            // REST: Block player
            RestClient.Post<ServerResponse>(flaskIP + "/block", f).Then(resp => {
                if (resp.success) {
                    Debug.Log("Successfully blocked player");
                } else {
                    Debug.LogError("Could not block player: " + resp.message);
                }
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });
        }

        [Serializable]
        public class ServerReport {
            public int userID;

            public ServerReport(int uID) {
                userID = uID;
            }
        }

        public void reportUser(string username) {
			User u = getUser(username);
            if (u != null) {
                // JSON
                ServerReport f = new ServerReport(u.getUserID());

                // REST: Report user
                RestClient.Post<ServerResponse>(flaskIP + "/report", f).Then(resp => {
                    if (resp.success) {
                        Debug.Log("Successfully reported player");
                    } else {
                        Debug.LogError("Could not report player: " + resp.message);
                    }
                }).Catch(err => {
                    Debug.LogError("Error: " + err.Message);
                });
            } else {
                Debug.LogError("Could not report player, user null");
            }
        }

        public void RemoveFriend(User user) {
            // JSON
            ServerFriend f = new ServerFriend(PolybiusManager.player.getUserID(), user.getUserID());

            // REST: Remove Friend
            RestClient.Put<ServerResponse>(flaskIP + "/friends", f).Then(resp => {
                if (resp.success) {
                    Debug.Log("Successfully remove friend");
                } else {
                    Debug.LogError("Could not remove friend: " + resp.message);
                }
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });
        }

        [Serializable]
        public class ServerGame {
            public string name, gameType;
            public float latCoord, longCoord;

            public ServerGame(string n, string gt, float latC, float longC) {
                name = n;
                gameType = gt;
                latCoord = latC;
                longCoord = longC;
            }
        }

        public void host(string rname, string gameType) {
            // JSON
            ServerGame g = new ServerGame(rname, gameType, PolybiusManager.currLat, PolybiusManager.currLong);

            // REST: Create lobby
            RestClient.Post<ServerResponse>(flaskIP + "/lobbies", g).Then(resp => {
                if (resp.success) {
                    PolybiusManager.currGame = new Game(rname, gameType, PolybiusManager.player, PolybiusManager.currLat, PolybiusManager.currLong);
                    Debug.Log("Successfully created new lobby");
                } else {
                    Debug.LogError("Could not create new lobby: " + resp.message);
                }
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });
        }

        [Serializable]
        public class ServerPrivacy {
            public int userID, privacy;

            public ServerPrivacy(int uID, int p) {
                userID = uID;
                privacy = p;
            }
        }

        public void setPrivacy(int userID, int privacy) {
            // JSON
            ServerPrivacy p = new ServerPrivacy(userID, privacy);

            // REST: Set privacy
            RestClient.Put<ServerResponse>(flaskIP + "/users", p).Then(resp => {
                if (resp.success) {
                    Debug.Log("Successfully set user " + userID.ToString() + " privacy to " + privacy);
                } else {
                    Debug.LogError("Could not set user " + userID.ToString() + "s privacy");
                }
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });
        }

        [Serializable]
        public class ServerOnline {
            public int userID;
            public bool isOnline;

            public ServerOnline(int uID, bool o) {
                userID = uID;
                isOnline = o;
            }
        }

        public void setOnline(bool isOnline) {
            // JSON
            ServerOnline o = new ServerOnline(PolybiusManager.player.getUserID(), isOnline);

            // REST: Set privacy
            RestClient.Put<ServerResponse>(flaskIP + "/users", o).Then(resp => {
                if (resp.success) {
                    Debug.Log("Successfully set currentUser's isOnline to: " + isOnline.ToString());
                } else {
                    Debug.LogError("Could not set currentUser's isOnline: " + resp.message);
                }
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });
        }

        // Functions returning a List

        public List<User> searchUsers(string search) {
            string url = flaskIP + "/users?search=" + search;
            List<User> results = new List<User>();

            // REST: Get array of users matching search
            RestClient.GetArray<User>(url).Then(respArray => {
                for (int i = 0; i < respArray.Length; i++)
                    results.Add(respArray[i]);
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });

            return results;
        }

        public List<User> getAllUsers() {
            string url = flaskIP + "/users";
            List<User> results = new List<User>();

            // REST: Get array of all users
            RestClient.GetArray<User>(url).Then(respArray => {
                for (int i = 0; i < respArray.Length; i++)
                    results.Add(respArray[i]);
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });

            return results;
        }

        public List<User> getFriends(int userID) {
            string url = flaskIP + "/friends?user1ID=" + userID.ToString();
            List<User> results = new List<User>();

            // REST: Get array of friends
            RestClient.GetArray<User>(url).Then(respArray => {
                for (int i = 0; i < respArray.Length; i++)
                    results.Add(respArray[i]);
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });

            return results;
        }

        public List<Message> getMessages(string senderUsername) {
            User u = getUser(senderUsername);
            List<Message> results = new List<Message>();

            if (u != null) {
                string url =    flaskIP + "/messages?" +
                                "user1ID=" + PolybiusManager.player.getUserID().ToString() +
                                "&user2ID=" + u.getUserID().ToString();

                // REST: Get array of messages
                RestClient.GetArray<Message>(url).Then(respArray => {
                    for (int i = 0; i < respArray.Length; i++)
                        results.Add(respArray[i]);
                }).Catch(err => {
                    Debug.LogError("Error: " + err.Message);
                });
            } else {
                Debug.LogError("Could not get messages, other user is null");
            }

            return results;
        }

        public List<Game> getLobbies(Game.type gameType) {
            string url = flaskIP + "/lobbies?gameType=" + gameType.ToString();
            List<Game> lobbies = new List<Game>();
            
            // REST: Get array of games
            RestClient.GetArray<Game>(url).Then(respArray => {
                for (int i = 0; i < respArray.Length; i++)
                    lobbies.Add(respArray[i]);
            }).Catch(err => {
                Debug.LogError("Error: " + err.Message);
            });

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
