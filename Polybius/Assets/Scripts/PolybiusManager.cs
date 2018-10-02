using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;


namespace polybius
{
    public class PolybiusManager : MonoBehaviour
    {
        // Enums
        enum status { online, offline, invisible };

        // Variables
        public static User player = new User();

        // Server Configuration
        public string ip = "";
        public int port = 9933;
        public SmartFox sfs = new SmartFox();
        public string initZone = "Polybius";
        public bool logged = false;

        void Start()
        {
            sfs.AddEventListener(SFSEvent.LOGIN, onLoggin);
            sfs.AddEventListener(SFSEvent.CONNECTION, onConnect);
            sfs.AddEventListener(SFSEvent.CONNECTION_LOST, onLost);
            sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, onResponse);
            sfs.ThreadSafeMode = true;

            sfs.Connect(ip, port);

        }

        void Update()
        {
            sfs.ProcessEvents();
        }

        //event listeners
        void onConnect(BaseEvent e)
        {
            if ((bool)e.Params["success"])
            {
                Debug.Log("Connected");
                sfs.Send(new LoginRequest("guest", "", initZone));

            }
            else
            {
                Debug.Log("Connection failed");
            }
        }

        void onLost(BaseEvent e)
        {
            logged = false;
        }

        void onLoggin(BaseEvent e)
        {
            logged = true;
        }

        void onResponse(BaseEvent e)
        {

            string cmd = (string)e.Params["cmd"];
            ISFSObject paramsa = (SFSObject)e.Params["params"];
            string message = cmd + " " + paramsa.GetUtfString("result") + " message: " + paramsa.GetUtfString("message");
            Debug.Log(cmd + " message: " + message);
        }

        //public methods
        public void login(string username, string password)
        {
            ISFSObject l = new SFSObject();
            l.PutUtfString("username", username);
            l.PutUtfString("password", password);
            sfs.Send(new ExtensionRequest("UserLogin", l));
        }

        public void create(string username, string password, string email)
        {
            player = new User(username, password, email);
            ISFSObject o = new SFSObject();
            o.PutUtfString("username", username);
            o.PutUtfString("password", password);
            o.PutUtfString("email", email);
            sfs.Send(new ExtensionRequest("CreateUser", o));
        }

        public SmartFox getConnection()
        {
            return sfs;
        }

        public bool isLogged()
        {
            return logged;
        }

        //exit handler
        void OnApplicationQuit()
        {
            Debug.Log("exiting");
            sfs.RemoveAllEventListeners();
            if (sfs.IsConnected)
            {
                sfs.Disconnect();
            }
        }
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

        public class User
        {
            public string username, password, email;
            public int userID;
            public List<Message> messages = new List<Message>();

            public User() : this(null, null, null)
            {
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
