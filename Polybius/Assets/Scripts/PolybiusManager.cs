using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace polybius {
    public static class PolybiusManager {
        // Enums
        enum status { online, offline, invisible };

        // Variables
        public static User player = new User();
        public static bool loggedIn = false;
        public static DatabaseManager dm = null;

    }

    public class Message
    {
        public int sender;
        public System.DateTime timestamp;
        public string message;

        public Message(int sender, System.DateTime timestamp, string message)
        {
            this.sender = sender;
            this.timestamp = timestamp;
            this.message = message;
        }
    }

    public class User {
        public string username, password, email, dob; 
        public int userID;
        public List<Message> messages = new List<Message>();

        public User() : this(null, null, null, null)
        {
        }

        public User(string u, string p, string e, string d)
		{
			username = u;
            password = p;
            email = e;
		    dob = d;
			userID = -1;

            //DEBUG: Add canned messages
            for (int i = 0; i < 5; i++) {
                messages.Add(new Message(-2, System.DateTime.Now, "This is message number " + i));
            }
		}
		
    }
    
}
