using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public static class PolybiusManager {
        // Enums
        enum status { online, offline, invisible };

        // Variables
        public static string username;
        public static int userID;
        public static List<Message> messages = new List<Message>();

        // Example methods
        // authenticate();
        // establishDatabaseConnection();
        // getUserStatus();
    }

    public class Message {
        public int sender;
        public System.DateTime timestamp;
        public string message;  

        public Message(int sender, System.DateTime timestamp, string message) {
            this.sender = sender;
            this.timestamp = timestamp;
            this.message = message;
        }
    }
}