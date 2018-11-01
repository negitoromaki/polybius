using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace polybius {
    public static class PolybiusManager {
        // Variables
        public static User player = new User();
        public static bool loggedIn = false;
        public static DatabaseManager dm = null;
        public static bool mutex = false;
        public static List<User> results = new List<User>();
        public static List<Game> games = new List<Game>();
        public static Game currGame;
        public static float currLat, currLong;

        // Notifications
        public static void sendNotification(string title, string message) {
            Assets.SimpleAndroidNotifications.NotificationManager.SendWithAppIcon(
                System.TimeSpan.FromSeconds(5),
                title,
                message,
                new Color(0, 0.6f, 1),
                Assets.SimpleAndroidNotifications.NotificationIcon.Message);
        }
    }

    public class Message {
        public string sender, receiver;
        public System.DateTime timestamp;
        public string message;

        public Message(string sender, string receiver, System.DateTime timestamp, string message) {
            this.receiver = receiver;
            this.sender = sender;
            this.timestamp = timestamp;
            this.message = message;
        }
    }

    public class Game {
        // Enum
        public enum type { pong, connect4, tictactoe, none }

        // Fields
        public float coordLong, coordLat;
        public User host, player;
        public type gameType;
        public List<User> spectators = new List<User>();
        public string roomName;

        public Game() : this(0, 0, null, type.none) {}

        public Game(float coordLong, float coordLat, User host, type gameType) {
            this.coordLong = coordLong;
            this.coordLat = coordLat;
            this.host = host;
            this.gameType = gameType;
        }
    }

    public class User {
        // Enum
        public enum status { online, offline, invisible };

        // Fields
        private string username, password, email, dob;
        private status currentStatus;
        private int privacy;
        private int userID;
        private List<Message> messages = new List<Message>();
        public List<User> friends = new List<User>();

        public User() : this(null, null, null, null) {}

        public User(string username, string password, string email, string dob) : this(username, password, email, dob, -1, 0) {}

        public User(string username, string password, string email, string dob, int userID, int privacy) {
            this.username = username;
            this.password = password;
            this.email = email;
            this.dob = dob;
            this.privacy = privacy; // true is private, false is public
            currentStatus = status.offline;
            this.userID = userID;
        }

        // ------------------
        // Overridden Methods
        // ------------------

        public override bool Equals(object obj) {
            User item = obj as User;

            if (item == null) {
                return false;
            }

            return userID.Equals(item.userID);
        }

        public override int GetHashCode() {
            return this.userID.GetHashCode();
        }

        // ------------------
        // User Field Getters
        // ------------------
        public string getUsername() {
            return username;
        }

        public int getUserID() {
            return userID;
        }

        public string getPassword() {
            return password;
        }

        public string getEmail() {
            return email;
        }
		public bool containsmsg(Message m){
			bool t = false;
			foreach(Message mc in messages){
				if (mc.timestamp == m.timestamp && mc.message == m.message) {
					t = true;
					Debug.Log ("message exists!");
					break;
				}
			}
			return t;
		}

		public void resetMsg(){
			messages = new List<Message> ();
		}

        public string getDob() {
            return dob;
        }

        public int getPrivacy() {
            return privacy;
        }

        // Get Messages from a user
        public List<Message> getMessagesForUser(string otherUsername) {
            PolybiusManager.dm.getMessagesRequest(otherUsername);
            List<Message> messagesfromUser = new List<Message>();
            for (int i = 0; i < messages.Count; i++) {
                if (messages[i].sender == otherUsername) {
                    messagesfromUser.Add(messages[i]);
                    messages.RemoveAt(i);
                }
            }
            return messagesfromUser;
        }

        public status getStatus() {
            return currentStatus;
        }

        // ------------------
        // User Field Setters
        // ------------------

        // Sets user's privacy status
        // Choose to set privacy to true or not
        public void setPrivacy(int privacy) {
            this.privacy = privacy;
            PolybiusManager.dm.setPrivacy(username, privacy);
        }

        // Updates the username if it contains appropriate characters
        // Returns true if valid username
        public bool setUsername(string newUser) {
            if (!string.IsNullOrEmpty(newUser)) {
                if (newUser == username && !string.IsNullOrEmpty(username))
                    return true;
                Match m = Regex.Match(newUser, "[A-Za-z0-9]+");
                if (m.Success) {
                    username = newUser;
                    // TODO: call database username update function
                    return true;
                }
            }
            return false;
        }

        // Updates the password if it contains appropriate characters
        // Returns true if valid password
        public bool setPassword(string newPass) {
            if (!string.IsNullOrEmpty(newPass)) {
                if (newPass == password && !string.IsNullOrEmpty(password))
                    return true;
                if (newPass.Length >= 8) {
                    password = newPass;
                    // TODO: call database password update function
                    return true;
                }
            }
            return false;
        }

        // Updates the email if it contains appropriate characters
        // Returns true if valid email address
        public bool setEmail(string newEmail) {
            if (!string.IsNullOrEmpty(newEmail)) {
                if (newEmail == email && !string.IsNullOrEmpty(email))
                    return true;
                Match m = Regex.Match(newEmail, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
                if (m.Success) {
                    email = newEmail;
                    // TODO: call database email update function
                    return true;
                }
            }
            return false;
        }

        // Updates the password if it contains appropriate characters
        // Returns true if valid password
        public bool setDob(string newDob) {
            if (!string.IsNullOrEmpty(newDob)) {
                if (newDob == dob && !string.IsNullOrEmpty(dob))
                    return true;
                Match m = Regex.Match(newDob, "(1[012]|0?[1-9])\\/(3[01]|[12][0-9]|0?[1-9])\\/((?:19|20)\\d{2})");
                if (m.Success) {
                    dob = newDob;
                    // TODO: call database dob update function
                    return true;
                }
            }
            return false;
        }

        // Updates user's status
        public void setStatus(status newStatus) {
            // TODO: call database get status function
            currentStatus = newStatus;
        }

        // Sets user's ID once
        public bool setUserID(int i) {
            // Make sure user ID is only set once
            if (userID == -1) {
                userID = i;
                return true;
            } else {
                return false;
            }
        }

        public void addMessage(Message m) {
            messages.Add(m);
        }
    }
}
