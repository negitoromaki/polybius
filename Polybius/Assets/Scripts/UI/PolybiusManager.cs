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
        // public static List<ReplaceMe> messages = new List<ReplaceMe>();

        // Example methods
        // authenticate();
        // establishDatabaseConnection();
        // getUserStatus();

        public static void getMessages() {
            //TODO: update messages object
        }

        public static void clearMessages() {
            //TODO: Empty messages list, update database if need be
        }
    }
}