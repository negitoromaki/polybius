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

        // Notifications
        static void sendNotification(string title, string message) {
            Assets.SimpleAndroidNotifications.NotificationManager.SendWithAppIcon(
                System.TimeSpan.FromSeconds(5),
                title,
                message,
                new Color(0, 0.6f, 1),
                Assets.SimpleAndroidNotifications.NotificationIcon.Message);
        }
    }

    public class Message {
        public int sender, receiver;
        public System.DateTime timestamp;
        public string message;

        public Message(int sender, int receiver, System.DateTime timestamp, string message) {
            this.receiver = receiver;
            this.sender = sender;
            this.timestamp = timestamp;
            this.message = message;
        }
    }

    public class User {
        // Enum
        public enum status { online, offline, invisible };

        // Fields
        private string username, password, email, dob;
        private status currentStatus;
        private int userID;
        private List<Message> messages = new List<Message>();
        private List<User> friends = new List<User>();

        public User() : this(null, null, null, null) {}

        public User(string username, string password, string email, string dob) {
            this.username = username;
            this.password = password;
            this.email = email;
		    this.dob = dob;
            currentStatus = status.offline;
			userID = -1;
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

        // Get Messages from a user
        public List<Message> getMessages(int id) {
            PolybiusManager.dm.getMessagesRequest(id);
            List<Message> messagesFromID = new List<Message>();
            for (int i = 0; i < messages.Count; i++) {
                if (messages[i].sender == id) {
                    messagesFromID.Add(messages[i]);
                    messages.RemoveAt(i);
                }
            }
            return messagesFromID;
        }

        public status getStatus() {
            return currentStatus;
        }

        // Returns a list of friends
        public List<User> getFriends() {
            //return PolybiusManager.dm.updateFriends();
            
            // Debug
            friends = new List<User>();
            for (int i = 0; i < 5; i++) {
                string name = "Bob " + i.ToString();
                friends.Add(new User(name, "bobrocks", "bob@bob.com", "10/10/1901"));
            }

            return friends;
        }

        // ------------------
        // User Field Setters
        // ------------------

        // Updates the username if it contains appropriate characters
        // Returns true if valid username
        public bool setUsername(string newUser) {
            if (!string.IsNullOrEmpty(newUser) && !string.IsNullOrEmpty(username)) {
                if (newUser == username)
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
            if (!string.IsNullOrEmpty(newPass) && !string.IsNullOrEmpty(password)) {
                if (newPass == password)
                    return true;
                if (newPass.Length > 8) {
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
            if (!string.IsNullOrEmpty(newEmail) && !string.IsNullOrEmpty(email)) {
                if (newEmail == email)
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
            if (!string.IsNullOrEmpty(newDob) && !string.IsNullOrEmpty(dob)) {
                if (newDob == dob)
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

        // Sends message from current user to another
        public void sendMessage(Message m) {
            PolybiusManager.dm.sendMessageRequest(m);
            messages.Add(m);
        }
    }
}
