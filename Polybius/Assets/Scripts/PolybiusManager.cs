using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        // gui handling
        private string username = "";
        private string password = "";
        private string email = "";
        public UIMain UIChanger;
        public GameObject mainPanel;

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

            if (cmd == "UserLogin")
            {
                string result = paramsa.GetUtfString("result");
                if (result == "success")
                {
                    // login successful
                    Debug.Log("Login successful!");
                    // switch to menu
                    UIChanger.ChangeMenu(mainPanel);
                }
                else
                {
                    Debug.LogError("Error with registration: " + result);
                }
            }

            if (cmd == "CreateUser")
            {
                string result = paramsa.GetUtfString("result");
                if (result == "success")
                {
                    Debug.Log("Register successful!");
                    UILogin(); // login after register is successful
                }
                else
                {
                    Debug.LogError("Error with registration: " + result);
                }
            }
        }

        //public methods
        // login user
        public void login(string username, string password)
        {
            ISFSObject l = new SFSObject();
            l.PutUtfString("username", username);
            l.PutUtfString("password", password);
            sfs.Send(new ExtensionRequest("UserLogin", l));
        }

        // register user
        public void create(string username, string password, string email, string dob)
        {
            player = new User(username, password, email,dob);
            ISFSObject o = new SFSObject();
            o.PutUtfString("username", username);
            o.PutUtfString("password", password);
            o.PutUtfString("email", email);
            //o.PutUtfString("dob", dob); // TODO: add DOB functionality
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

        // UI handling for login/register
        public void SetUsername(InputField _username)
        {
            if (!string.IsNullOrEmpty(_username.text))
            {
                username = _username.text;
                Debug.Log("Username set to: " + username);
            }
               
        }

        public void SetPassword(InputField _password)
        {
            if (!string.IsNullOrEmpty(_password.text))
            {
                password = _password.text;
                Debug.Log("Password set to: " + password);
            }
        }

        public void SetEmail(InputField _email)
        {
            if (!string.IsNullOrEmpty(_email.text))
            {
                email = _email.text;
                Debug.Log("Email set to: " + email);
            }
        }

        // logging in
        public void UILogin()
        {
            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(username))
            {
                Debug.Log("Logging in user...");
                login(username, password); // call login
                password = ""; // wipe password
            }
        }

        // register
        public void UIRegister()
        {
            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email))
            {
                Debug.Log("Registering new user...");
                create(username, password, email, ""); // call register (dob default nothing rn)
                //password = "";
            }
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
        public string username, password, email, dob;
        public int userID;
        public List<Message> messages = new List<Message>();

        public User() : this(null, null, null,null)
        {
        }

        public User(string u, string p, string e, string d)
        {
            username = u;
            password = p;
            email = e;
            dob = d;
            userID = -1;
        }
    }
    
}
