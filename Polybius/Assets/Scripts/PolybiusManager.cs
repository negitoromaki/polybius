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

        // Database Fields
        public int lobbyID;
        public float latCoord, longCoord;
        public string gameType, name;

        // Client-side fields (Must populate)
        public User host, player;
        public List<User> spectators = new List<User>();

        public Game(string roomName, string gameType, User host, float coordLat, float coordLong) {
            this.name = roomName;
            this.longCoord = coordLong;
            this.latCoord = coordLat;
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
        public List<string> blockedUsers = new List<string>();

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

        public string getDob() {
            return dob;
        }

        public int getPrivacy() {
            return privacy;
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
            PolybiusManager.dm.setPrivacy(userID, privacy);
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
