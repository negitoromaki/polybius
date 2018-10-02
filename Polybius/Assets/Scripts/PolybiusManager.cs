using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;


namespace polybius {
    public class PolybiusManager : MonoBehaviour
    {
        // Enums
        enum status { online, offline, invisible };

        // Variables
        public static User player;
        public static Toggle debugT;
        private static string host = "128.211.248.49";
        private static string errormsg;
        private static int port = 8080;
        private SmartFox sfs;

        void Start()
        {
            player = new User();
        }

        void Update()
        {
            if (sfs != null)
            {
                sfs.ProcessEvents();
            }
        }
        void OnApplicationQuit()
        {
            if (sfs != null && sfs.IsConnected)
                sfs.Disconnect();
        }
        // Example methods
        void authenticate()
        {
            
            if (sfs == null || !sfs.IsConnected)
            {
                sfs = new SmartFox();

                sfs.ThreadSafeMode = true;

                sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
                sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
                sfs.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, onUserVarsUpdate);


                ConfigData cfg = new ConfigData();
                cfg.Host = host;
                cfg.Port = port;
                cfg.Zone = "Polybius";
                cfg.Debug = debugT.isOn;
                sfs.Connect(cfg);
            }
            else
            {
                sfs.Disconnect();
            }

        }
        private void reset()
        {
            // Remove SFS2X listeners
            sfs.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
            sfs.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
            sfs.RemoveEventListener(SFSEvent.USER_VARIABLES_UPDATE, onUserVarsUpdate);


            sfs = null;


        }
        private void OnConnectionLost(BaseEvent evt)
        {
            trace("Connection was lost; reason is: " + (string)evt.Params["reason"]);
            reset();
        }
        
        private void OnConnection(BaseEvent evt)
        {
            if ((bool)evt.Params["success"])
            {
                sfs.Send(new Sfs2X.Requests.LoginRequest(player.username, player.password, player.email));
            }
            else
            {
                reset();
                errormsg = "Connection failed";
            }
        }
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
    public class User
    { 
        public string username;
        public int userID;
        public string password;
        public string email;
        public List<Message> messages = new List<Message>();

        public User() {
            username = null;
            password = null;
            email = null;
            userID = -1;
        }

        public User(string u, string p, string e)
        {
            username = u;
            password = p;
            email = e;
            userID = -1;
        }
    }
}